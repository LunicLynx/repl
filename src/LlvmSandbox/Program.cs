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

        private static LLVMValueRef _addPoints;
        private static LLVMValueRef _addPoints2;

        private static LLVMTypeRef _i1 = LLVMTypeRef.Int1;
        private static LLVMTypeRef _i8 = LLVMTypeRef.Int8;
        private static LLVMTypeRef _i16 = LLVMTypeRef.Int16;
        private static LLVMTypeRef _i32 = LLVMTypeRef.Int32;
        private static LLVMTypeRef _i64 = LLVMTypeRef.Int64;

        private static LLVMTypeRef _pi8 = LLVMTypeRef.CreatePointer(_i8, 0);

        private static LLVMTypeRef _void = LLVMTypeRef.Void;


        // memcpy
        private static LLVMTypeRef _llvmMemcpyP0I8P0I8I32Type = LLVMTypeRef.CreateFunction(_void, new[] { _pi8, _pi8, _i32, _i1 });
        private static LLVMTypeRef _llvmMemcpyP0I8P0I8I64Type = LLVMTypeRef.CreateFunction(_void, new[] { _pi8, _pi8, _i64, _i1 });

        // memset
        private static LLVMTypeRef _llvmMemsetP0I8I64Type = LLVMTypeRef.CreateFunction(_void, new[] {_pi8, _i8, _i64, _i1});

        // create a type that is bigger than 64 bits to eliminate optimization as
        // register. At least this happens in clang++ not sure what the reason is.

        // struct Point(i32,i32,i32,i32)
        private static LLVMTypeRef _struct = LLVMTypeRef.CreateStruct(new[] { _i32, _i32, _i32, _i32, }, false);
        private static LLVMTypeRef _pstruct = LLVMTypeRef.CreatePointer(_struct, 0);

        private static LLVMValueRef _false = LLVMValueRef.CreateConstInt(_i1, 0);
        private static LLVMValueRef _true = LLVMValueRef.CreateConstInt(_i1, 1);

        private static LLVMValueRef _n0i8 = LLVMValueRef.CreateConstInt(_i8, 0);
        private static LLVMValueRef _n1i8 = LLVMValueRef.CreateConstInt(_i8, 0);
        private static LLVMValueRef _n2i8 = LLVMValueRef.CreateConstInt(_i8, 0);
        private static LLVMValueRef _n3i8 = LLVMValueRef.CreateConstInt(_i8, 0);
        private static LLVMValueRef _n4i8 = LLVMValueRef.CreateConstInt(_i8, 0);

        private static LLVMValueRef _n0i16 = LLVMValueRef.CreateConstInt(_i16, 0);
        private static LLVMValueRef _n1i16 = LLVMValueRef.CreateConstInt(_i16, 0);
        private static LLVMValueRef _n2i16 = LLVMValueRef.CreateConstInt(_i16, 0);
        private static LLVMValueRef _n3i16 = LLVMValueRef.CreateConstInt(_i16, 0);
        private static LLVMValueRef _n4i16 = LLVMValueRef.CreateConstInt(_i16, 0);


        private static LLVMValueRef _n0i32 = LLVMValueRef.CreateConstInt(_i32, 0);
        private static LLVMValueRef _n1i32 = LLVMValueRef.CreateConstInt(_i32, 0);
        private static LLVMValueRef _n2i32 = LLVMValueRef.CreateConstInt(_i32, 0);
        private static LLVMValueRef _n3i32 = LLVMValueRef.CreateConstInt(_i32, 0);
        private static LLVMValueRef _n4i32 = LLVMValueRef.CreateConstInt(_i32, 0);

        private static LLVMValueRef _n0i64 = LLVMValueRef.CreateConstInt(_i64, 0);
        private static LLVMValueRef _n1i64 = LLVMValueRef.CreateConstInt(_i64, 0);
        private static LLVMValueRef _n2i64 = LLVMValueRef.CreateConstInt(_i64, 0);
        private static LLVMValueRef _n3i64 = LLVMValueRef.CreateConstInt(_i64, 0);
        private static LLVMValueRef _n4i64 = LLVMValueRef.CreateConstInt(_i64, 0);
        private static LLVMValueRef _n16I64 = LLVMValueRef.CreateConstInt(_i64, 16);
        
        private static LLVMValueRef _llvmMemcpyP0I8P0I8I32;
        private static LLVMValueRef _llvmMemcpyP0I8P0I8I64;
        private static LLVMValueRef _llvmMemsetP0I8I64;
        

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

            // build memcpy intrinsic

            _llvmMemcpyP0I8P0I8I32 = mod.AddFunction("llvm.memcpy.p0i8.p0i8.i32", _llvmMemcpyP0I8P0I8I64Type);
            _llvmMemcpyP0I8P0I8I64 = mod.AddFunction("llvm.memcpy.p0i8.p0i8.i64", _llvmMemcpyP0I8P0I8I64Type);

            _llvmMemsetP0I8I64 = mod.AddFunction("llvm.memset.p0i8.i64", _llvmMemsetP0I8I64Type);

            var sum = AddSumFunction(mod);
            var test = AddClass(mod);
            TestByValueParameterAndReturn(mod);
            TestByValueParameterAndReturn2(mod);
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
            // create a function
            // Point Add(Point, Point)
            var fnType = LLVMTypeRef.CreateFunction(_struct, new[] { _struct, _struct });
            _addPoints = mod.AddFunction("addPoints", fnType);

            var entry = _addPoints.AppendBasicBlock("entry");

            var paraA = _addPoints.Params[0];
            var paraB = _addPoints.Params[1];

            using (var builder = mod.Context.CreateBuilder())
            {
                builder.PositionAtEnd(entry);

                // alloc return struct
                var pr = builder.BuildAlloca(_struct);

                // store all parameters to locals
                var pa = builder.BuildAlloca(_struct);
                var pb = builder.BuildAlloca(_struct);
                builder.BuildStore(paraA, pa);
                builder.BuildStore(paraB, pb);

                // build add
                var pax = builder.BuildGEP(pa, new[] { _n0i64, _n0i32 });
                var ax = builder.BuildLoad(pax);
                var pbx = builder.BuildGEP(pb, new[] { _n0i64, _n0i32 });
                var bx = builder.BuildLoad(pbx);
                var rx = builder.BuildAdd(ax, bx);
                var prx = builder.BuildGEP(pr, new[] { _n0i64, _n0i32 });
                builder.BuildStore(rx, prx);

                var pay = builder.BuildGEP(pa, new[] { _n0i64, _n1i32 });
                var ay = builder.BuildLoad(pay);
                var pby = builder.BuildGEP(pb, new[] { _n0i64, _n1i32 });
                var by = builder.BuildLoad(pby);
                var ry = builder.BuildAdd(ay, by);
                var pry = builder.BuildGEP(pr, new[] { _n0i64, _n1i32 });
                builder.BuildStore(ry, pry);

                var paz = builder.BuildGEP(pa, new[] { _n0i64, _n2i32 });
                var az = builder.BuildLoad(paz);
                var pbz = builder.BuildGEP(pb, new[] { _n0i64, _n2i32 });
                var bz = builder.BuildLoad(pbz);
                var rz = builder.BuildAdd(az, bz);
                var prz = builder.BuildGEP(pr, new[] { _n0i64, _n2i32 });
                builder.BuildStore(rz, prz);

                var paw = builder.BuildGEP(pa, new[] { _n0i64, _n3i32 });
                var aw = builder.BuildLoad(paw);
                var pbw = builder.BuildGEP(pb, new[] { _n0i64, _n3i32 });
                var bw = builder.BuildLoad(pbw);
                var rw = builder.BuildAdd(aw, bw);
                var prw = builder.BuildGEP(pr, new[] { _n0i64, _n3i32 });
                builder.BuildStore(rw, prw);

                // for now just return the first argument
                var r = builder.BuildLoad(pr);
                builder.BuildRet(r);
            }
        }

        private static void TestByValueParameterAndReturn2(LLVMModuleRef mod)
        {
            // create a function
            // Point Add(Point, Point)
            var fnType = LLVMTypeRef.CreateFunction(_void, new[] { _pstruct, _pstruct, _pstruct });
            _addPoints2 = mod.AddFunction("addPoints2", fnType);

            var entry = _addPoints2.AppendBasicBlock("entry");

            var paraReturn = _addPoints2.Params[0];
            var paraA = _addPoints2.Params[1];
            var paraB = _addPoints2.Params[2];

            using (var builder = mod.Context.CreateBuilder())
            {
                builder.PositionAtEnd(entry);

                // alloc storage for return pointer
                var ppr = builder.BuildAlloca(_pi8);
                var pr = builder.BuildBitCast(paraReturn, _pi8);
                builder.BuildStore(pr, ppr);

                // set memory for return value to zero
                pr = builder.BuildBitCast(paraReturn, _pi8);
                builder.BuildCall(_llvmMemsetP0I8I64, new[] {pr, _n0i8, _n16I64, _false});

                // build add
                var pax = builder.BuildInBoundsGEP(paraA, new[] { _n0i64, _n0i32 });
                var ax = builder.BuildLoad(pax);
                var pbx = builder.BuildInBoundsGEP(paraB, new[] { _n0i64, _n0i32 });
                var bx = builder.BuildLoad(pbx);
                var rx = builder.BuildAdd(ax, bx);
                var prx = builder.BuildInBoundsGEP(paraReturn, new[] { _n0i64, _n0i32 });
                builder.BuildStore(rx, prx);

                var pay = builder.BuildInBoundsGEP(paraA, new[] { _n0i64, _n1i32 });
                var ay = builder.BuildLoad(pay);
                var pby = builder.BuildInBoundsGEP(paraB, new[] { _n0i64, _n1i32 });
                var by = builder.BuildLoad(pby);
                var ry = builder.BuildAdd(ay, by);
                var pry = builder.BuildInBoundsGEP(paraReturn, new[] { _n0i64, _n1i32 });
                builder.BuildStore(ry, pry);

                var paz = builder.BuildInBoundsGEP(paraA, new[] { _n0i64, _n2i32 });
                var az = builder.BuildLoad(paz);
                var pbz = builder.BuildInBoundsGEP(paraB, new[] { _n0i64, _n2i32 });
                var bz = builder.BuildLoad(pbz);
                var rz = builder.BuildAdd(az, bz);
                var prz = builder.BuildInBoundsGEP(paraReturn, new[] { _n0i64, _n2i32 });
                builder.BuildStore(rz, prz);

                var paw = builder.BuildInBoundsGEP(paraA, new[] { _n0i64, _n3i32 });
                var aw = builder.BuildLoad(paw);
                var pbw = builder.BuildInBoundsGEP(paraB, new[] { _n0i64, _n3i32 });
                var bw = builder.BuildLoad(pbw);
                var rw = builder.BuildAdd(aw, bw);
                var prw = builder.BuildInBoundsGEP(paraReturn, new[] { _n0i64, _n3i32 });
                builder.BuildStore(rw, prw);

                // for now just return the first argument
                builder.BuildRetVoid();
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
                var ps1 = builder.BuildAlloca(_struct);
                var ps2 = builder.BuildAlloca(_struct);
                var pr = builder.BuildAlloca(_struct);

                var ps1x = builder.BuildGEP(ps1, new[] { _n0i64, _n0i32 });
                builder.BuildStore(_n1i32, ps1x);
                var ps1y = builder.BuildGEP(ps1, new[] { _n0i64, _n1i32 });
                builder.BuildStore(_n2i32, ps1y);
                var ps1z = builder.BuildGEP(ps1, new[] { _n0i64, _n2i32 });
                builder.BuildStore(_n3i32, ps1z);
                var ps1w = builder.BuildGEP(ps1, new[] { _n0i64, _n3i32 });
                builder.BuildStore(_n4i32, ps1w);

                var ps2x = builder.BuildGEP(ps2, new[] { _n0i64, _n0i32 });
                builder.BuildStore(_n1i32, ps2x);
                var ps2y = builder.BuildGEP(ps2, new[] { _n0i64, _n1i32 });
                builder.BuildStore(_n2i32, ps2y);
                var ps2z = builder.BuildGEP(ps2, new[] { _n0i64, _n2i32 });
                builder.BuildStore(_n3i32, ps2z);
                var ps2w = builder.BuildGEP(ps2, new[] { _n0i64, _n3i32 });
                builder.BuildStore(_n4i32, ps2w);

                var s1 = builder.BuildLoad(ps1);
                var s2 = builder.BuildLoad(ps2);
                var r = builder.BuildCall(_addPoints, new[] { s1, s2 });
                builder.BuildStore(r, pr);

                // a
                var pa = builder.BuildAlloca(_struct);
                var pai8 = builder.BuildBitCast(pa, _pi8);
                var ps1i8 = builder.BuildBitCast(ps1, _pi8);
                builder.BuildCall(_llvmMemcpyP0I8P0I8I64, new[]
                {
                    pai8, ps1i8, _n16I64, _false
                });

                // b
                var pb = builder.BuildAlloca(_struct);
                var pbi8 = builder.BuildBitCast(pb, _pi8);
                var ps2i8 = builder.BuildBitCast(ps2, _pi8);
                builder.BuildCall(_llvmMemcpyP0I8P0I8I64, new[]
                {
                    pbi8, ps2i8, _n16I64, _false
                });

                // return value
                var pr2 = builder.BuildAlloca(_struct);

                builder.BuildCall(_addPoints2, new[] {pr2, pa, pb});

                builder.BuildRetVoid();
            }

            return fn;
        }
    }
}
