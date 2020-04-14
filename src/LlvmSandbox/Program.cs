using System;
using System.Runtime.InteropServices;
using LLVMSharp.Interop;

namespace LlvmSandbox
{
    class Program
    {

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Add(int a, int b);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Ca();

        private static void Main(string[] args)
        {
            var mod = LLVMModuleRef.CreateWithName("LLVMSharpIntro");
            //LLVMBool Success = new LLVMBool(0);
            //LLVMModuleRef mod = LLVM.ModuleCreateWithName("LLVMSharpIntro");
            //var xmod = new XModule("LLVMSharpIntro");


            var sum = AddSumFunction(mod);
            var test = AddClass(mod);

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

            using (engine)
            {
                var addMethod =
                    (Add)Marshal.GetDelegateForFunctionPointer(engine.GetPointerToGlobal(sum), typeof(Add));
                var caMethod = (Ca)Marshal.GetDelegateForFunctionPointer(engine.GetPointerToGlobal(test), typeof(Ca));
                int result = addMethod(10, 10);
                int result1 = caMethod();

                Console.WriteLine("Result of sum is: " + result);
                Console.WriteLine("Result of test is: " + result1);


                if (mod.WriteBitcodeToFile("sum.bc") != 0)
                {
                    Console.WriteLine("error writing bitcode to file, skipping");
                }

                //LLVM.DumpModule(mod);
            }

            Console.ReadLine();
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

    }
}
