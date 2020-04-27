using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using LLVMSharp.Interop;

namespace LlvmSandbox
{
    class Program
    {
        private static LLVMTypeRef _strct;
        private static LLVMValueRef _addPoints;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Add(int a, int b);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Ca();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Entry();

        private static void Main(string[] args)
        {
            var mod = LLVMModuleRef.CreateWithName("LLVMSharpIntro");
            //LLVMBool Success = new LLVMBool(0);
            //LLVMModuleRef mod = LLVM.ModuleCreateWithName("LLVMSharpIntro");
            //var xmod = new XModule("LLVMSharpIntro");


            var sum = AddSumFunction(mod);
            var test = AddClass(mod);
            TestByValueParameterAndReturn(mod);
            var entry = AddEntry(mod);

            if (!mod.TryVerify(LLVMVerifierFailureAction.LLVMPrintMessageAction, out var error))
            {
                Console.WriteLine($"Error: {error}");
            }

            mod.Dump();


            LLVM.LinkInMCJIT();

            LLVM.InitializeX86TargetMC();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86AsmParser();
            LLVM.InitializeX86AsmPrinter();




            var options = LLVMMCJITCompilerOptions.Create();
            options.NoFramePointerElim = 1;

            if (!mod.TryCreateMCJITCompiler(out var engine, ref options, out error))
            {
                Console.WriteLine($"Error: {error}");
            }

            Emit(mod);

            using (engine)
            {
                //var addMethod =
                //    (Add)Marshal.GetDelegateForFunctionPointer(engine.GetPointerToGlobal(sum), typeof(Add));
                //var caMethod = (Ca)Marshal.GetDelegateForFunctionPointer(engine.GetPointerToGlobal(test), typeof(Ca));
                //int result = addMethod(10, 10);
                //int result1 = caMethod();

                //Console.WriteLine("Result of sum is: " + result);
                //Console.WriteLine("Result of test is: " + result1);


                //if (mod.WriteBitcodeToFile("sum.bc") != 0)
                //{
                //    Console.WriteLine("error writing bitcode to file, skipping");
                //}

                var entryFunction = (Entry)Marshal.GetDelegateForFunctionPointer(engine.GetPointerToGlobal(entry), typeof(Entry));
                Debugger.Break();
                entryFunction();

                //LLVM.DumpModule(mod);
            }

            Console.ReadLine();
        }

        private static void Emit(LLVMModuleRef mod)
        {
            var targetTriple = LLVMTargetRef.DefaultTriple;
            mod.Target = targetTriple;

            var target = LLVMTargetRef.Targets.SingleOrDefault(t => t.Name == "x86-64");

            var targetMachine = target.CreateTargetMachine(targetTriple, "generic", "",
                LLVMCodeGenOptLevel.LLVMCodeGenLevelNone,
                LLVMRelocMode.LLVMRelocDefault,
                LLVMCodeModel.LLVMCodeModelDefault);

            var dataLayout = targetMachine.CreateTargetDataLayout();
            mod.DataLayout = dataLayout;

            targetMachine.TryEmitToFile(mod, "file.asm", LLVMCodeGenFileType.LLVMAssemblyFile, out var error);
        }

        private static LLVMValueRef AddClass(LLVMModuleRef mod)
        {
            //var type = LLVM.StructType(new []{ LLVM.Int32Type() }, false);

            var ctx = mod.Context;

            var s = ctx.CreateNamedStruct("Foo");
            var ps = LLVMTypeRef.CreatePointer(s, 0);

            s.StructSetBody(new[] { LLVMTypeRef.Int32, }, false);

            LLVMValueRef fn1;
            using (var builder = ctx.CreateBuilder())
            {
                var ft = LLVMTypeRef.CreateFunction(LLVMTypeRef.Void, new[] { ps }, false);
                var fn = mod.AddFunction("Foo_ctor", ft);
                var bb = fn.AppendBasicBlock("entry");
                builder.PositionAtEnd(bb);
                var zero = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0);
                var pThis = fn.Params[0];
                var gep = builder.BuildGEP(pThis, new[] { zero, zero });
                builder.BuildStore(zero, gep);
                builder.BuildRetVoid();

                // add setter
                var ftSetter = LLVMTypeRef.CreateFunction(LLVMTypeRef.Void, new[] { ps, LLVMTypeRef.Int32 });
                var fnSetter = mod.AddFunction("Foo_Set_Length", ftSetter);
                var bbSetter = fnSetter.AppendBasicBlock("entry");
                builder.PositionAtEnd(bbSetter);
                var pThis1 = fnSetter.Params[0];
                var gep1 = builder.BuildGEP(pThis1, new[] { zero, zero });
                var value = fnSetter.Params[1];
                builder.BuildStore(value, gep1);
                builder.BuildRetVoid();

                // add getter
                var ftGetter = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, new[] { ps });
                var fnGetter = mod.AddFunction("Foo_Get_Length", ftGetter);
                var bbGetter = fnGetter.AppendBasicBlock("entry");
                builder.PositionAtEnd(bbGetter);
                var pThis2 = fnGetter.Params[0];
                var gep2 = builder.BuildGEP(pThis2, new[] { zero, zero });
                var value1 = builder.BuildLoad(gep2);
                builder.BuildRet(value1);

                // add test function
                var ft1 = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, new LLVMTypeRef[0]);
                fn1 = mod.AddFunction("Test", ft1);
                var bb1 = fn1.AppendBasicBlock("entry");
                builder.PositionAtEnd(bb1);
                var p = builder.BuildAlloca(s);
                builder.BuildCall(fn, new[] { p });
                builder.BuildCall(fnSetter, new[] { p, LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 23, false) });
                var get = builder.BuildCall(fnGetter, new[] { p });
                builder.BuildRet(get);
            }

            return fn1;
        }

        private static LLVMValueRef AddSumFunction(LLVMModuleRef mod)
        {
            LLVMTypeRef[] paramTypes = { LLVMTypeRef.Int32, LLVMTypeRef.Int32 };
            LLVMTypeRef retType = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, paramTypes);

            var sum = mod.AddFunction("sum", retType);
            var entry = sum.AppendBasicBlock("entry");

            using (var builder = mod.Context.CreateBuilder())
            {
                builder.PositionAtEnd(entry);
                var tmp = builder.BuildAdd(sum.Params[0], sum.Params[1]);
                builder.BuildRet(tmp);
            }

            return sum;
        }

        private static void TestByValueParameterAndReturn(LLVMModuleRef mod)
        {
            // create a type that is bigger than 64 bits to eliminate optimization as
            // register. At least this happens in clang++ not sure what the reason is.


            // struct Point(i32,i32,i32,i32)
            _strct = LLVMTypeRef.CreateStruct(new[]
            {
                LLVMTypeRef.Int32,
                LLVMTypeRef.Int32,
                LLVMTypeRef.Int32,
                LLVMTypeRef.Int32,
            }, false);


            // create a function
            // Point Add(Point, Point)
            var fnType = LLVMTypeRef.CreateFunction(_strct, new[] { _strct, _strct });
            _addPoints = mod.AddFunction("addPoints", fnType);

            var entry = _addPoints.AppendBasicBlock("entry");

            var paraA = _addPoints.Params[0];
            var paraB = _addPoints.Params[1];

            using (var builder = mod.Context.CreateBuilder())
            {
                builder.PositionAtEnd(entry);

                // alloc return struct
                var pr = builder.BuildAlloca(_strct);

                // store all parameters to locals
                var pa = builder.BuildAlloca(_strct);
                var pb = builder.BuildAlloca(_strct);
                builder.BuildStore(paraA, pa);
                builder.BuildStore(paraB, pb);

                var zero64 = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, 0);
                var one64 = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, 0);
                var two64 = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, 0);
                var three64 = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, 0);
                var four64 = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, 0);

                var zero32 = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0);
                var one32 = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0);
                var two32 = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0);
                var three32 = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0);
                var four32 = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0);

                // build add
                var pax = builder.BuildGEP(pa, new[] { zero64, zero32 });
                var ax = builder.BuildLoad(pax);
                var pbx = builder.BuildGEP(pb, new[] { zero64, zero32 });
                var bx = builder.BuildLoad(pbx);
                var rx = builder.BuildAdd(ax, bx);
                var prx = builder.BuildGEP(pr, new[] { zero64, zero32 });
                builder.BuildStore(rx, prx);

                var pay = builder.BuildGEP(pa, new[] { zero64, one32 });
                var ay = builder.BuildLoad(pay);
                var pby = builder.BuildGEP(pb, new[] { zero64, one32 });
                var by = builder.BuildLoad(pby);
                var ry = builder.BuildAdd(ay, by);
                var pry = builder.BuildGEP(pr, new[] { zero64, one32 });
                builder.BuildStore(ry, pry);

                var paz = builder.BuildGEP(pa, new[] { zero64, two32 });
                var az = builder.BuildLoad(paz);
                var pbz = builder.BuildGEP(pb, new[] { zero64, two32 });
                var bz = builder.BuildLoad(pbz);
                var rz = builder.BuildAdd(az, bz);
                var prz = builder.BuildGEP(pr, new[] { zero64, two32 });
                builder.BuildStore(rz, prz);

                var paw = builder.BuildGEP(pa, new[] { zero64, three32 });
                var aw = builder.BuildLoad(paw);
                var pbw = builder.BuildGEP(pb, new[] { zero64, three32 });
                var bw = builder.BuildLoad(pbw);
                var rw = builder.BuildAdd(aw, bw);
                var prw = builder.BuildGEP(pr, new[] { zero64, three32 });
                builder.BuildStore(rw, prw);

                // for now just return the first argument
                var r = builder.BuildLoad(pr);
                builder.BuildRet(r);

                
            }
        }

        public static LLVMValueRef AddEntry(LLVMModuleRef mod)
        {
            var fnt = LLVMTypeRef.CreateFunction(LLVMTypeRef.Void, Array.Empty<LLVMTypeRef>());
            var fn = mod.AddFunction("entry", fnt);
            var entry = fn.AppendBasicBlock("entry");

            using (var builder = mod.Context.CreateBuilder())
            {
                builder.PositionAtEnd(entry);

                // TODO build call to add points
                var ps1 = builder.BuildAlloca(_strct);
                var ps2 = builder.BuildAlloca(_strct);
                var pr = builder.BuildAlloca(_strct);

                var zero64 = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, 0);

                var ps1x = builder.BuildGEP(ps1, new[] { zero64, LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0) });
                builder.BuildStore(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 1), ps1x);
                var ps1y = builder.BuildGEP(ps1, new[] { zero64, LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 1) });
                builder.BuildStore(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 2), ps1y);
                var ps1z = builder.BuildGEP(ps1, new[] { zero64, LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 2) });
                builder.BuildStore(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 3), ps1z);
                var ps1w = builder.BuildGEP(ps1, new[] { zero64, LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 3) });
                builder.BuildStore(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 4), ps1w);

                var ps2x = builder.BuildGEP(ps2, new[] { zero64, LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0) });
                builder.BuildStore(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 1), ps2x);
                var ps2y = builder.BuildGEP(ps2, new[] { zero64, LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 1) });
                builder.BuildStore(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 2), ps2y);
                var ps2z = builder.BuildGEP(ps2, new[] { zero64, LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 2) });
                builder.BuildStore(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 3), ps2z);
                var ps2w = builder.BuildGEP(ps2, new[] { zero64, LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 3) });
                builder.BuildStore(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 4), ps2w);


                var s1 = builder.BuildLoad(ps1);
                var s2 = builder.BuildLoad(ps2);
                var r = builder.BuildCall(_addPoints, new[] { s1, s2 });
                builder.BuildStore(r, pr);
                builder.BuildRetVoid();
            }

            return fn;
        }
    }
}
