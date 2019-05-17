using System;
using System.Runtime.InteropServices;
using LLVMSharp;
using XLang.Codegen.Llvm;

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
            LLVMBool Success = new LLVMBool(0);
            LLVMModuleRef mod = LLVM.ModuleCreateWithName("LLVMSharpIntro");
            var xmod = new XModule("LLVMSharpIntro");


            var sum = AddSumFunction(mod);
            var xsum = XAddSumFunction(xmod);
            var test = AddClass(mod);
            var xtest = XAddClass(xmod);

            if (!xmod.Verify(LLVMVerifierFailureAction.LLVMPrintMessageAction, out var error1))
            {
                Console.WriteLine($"Error: {error1}");
            }

            if (LLVM.VerifyModule(mod, LLVMVerifierFailureAction.LLVMPrintMessageAction, out var error) != Success)
            {
                Console.WriteLine($"Error: {error}");
            }

            xmod.Dump();
            LLVM.DumpModule(mod);

            LLVM.LinkInMCJIT();

            LLVM.InitializeX86TargetMC();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86AsmParser();
            LLVM.InitializeX86AsmPrinter();

            LLVMMCJITCompilerOptions options = new LLVMMCJITCompilerOptions { NoFramePointerElim = 1 };
            LLVM.InitializeMCJITCompilerOptions(options);
            if (LLVM.CreateMCJITCompilerForModule(out var engine, mod, options, out error) != Success)
            {
                Console.WriteLine($"Error: {error}");
            }

            var addMethod = (Add)Marshal.GetDelegateForFunctionPointer(LLVM.GetPointerToGlobal(engine, sum), typeof(Add));
            var caMethod = (Ca)Marshal.GetDelegateForFunctionPointer(LLVM.GetPointerToGlobal(engine, test), typeof(Ca));
            int result = addMethod(10, 10);
            int result1 = caMethod();

            Console.WriteLine("Result of sum is: " + result);
            Console.WriteLine("Result of test is: " + result1);

            if (LLVM.WriteBitcodeToFile(mod, "sum.bc") != 0)
            {
                Console.WriteLine("error writing bitcode to file, skipping");
            }

            //LLVM.DumpModule(mod);


            LLVM.DisposeExecutionEngine(engine);

            Console.ReadLine();
        }

        private static LLVMValueRef AddClass(LLVMModuleRef mod)
        {
            //var type = LLVM.StructType(new []{ LLVM.Int32Type() }, false);

            var ctx = LLVM.GetModuleContext(mod);
            var s = LLVM.StructCreateNamed(ctx, "Foo");

            var ps = LLVM.PointerType(s, 0);

            LLVM.StructSetBody(s, new[] { LLVM.Int32Type() }, false);

            var builder = LLVM.CreateBuilder();

            // add ctor
            var ft = LLVM.FunctionType(ps, new LLVMTypeRef[0], false);
            var fn = LLVM.AddFunction(mod, "Foo_ctor", ft);
            var bb = LLVM.AppendBasicBlock(fn, "entry");
            LLVM.PositionBuilderAtEnd(builder, bb);
            var pThis = LLVM.BuildAlloca(builder, s, "this");
            var zero = LLVM.ConstInt(LLVM.Int32Type(), 0, false);
            var gep = LLVM.BuildGEP(builder, pThis, new[] { zero, zero }, "");
            LLVM.BuildStore(builder, zero, gep);
            LLVM.BuildRet(builder, pThis);

            // add setter
            var ftSetter = LLVM.FunctionType(LLVM.VoidType(), new[] { ps, LLVM.Int32Type() }, false);
            var fnSetter = LLVM.AddFunction(mod, "Foo_Set_Length", ftSetter);
            var bbSetter = LLVM.AppendBasicBlock(fnSetter, "entry");
            LLVM.PositionBuilderAtEnd(builder, bbSetter);
            var pThis1 = LLVM.GetParam(fnSetter, 0);
            var gep1 = LLVM.BuildGEP(builder, pThis1, new[] { zero, zero }, "");
            var value = LLVM.GetParam(fnSetter, 1);
            LLVM.BuildStore(builder, value, gep1);
            LLVM.BuildRetVoid(builder);

            // add getter
            var ftGetter = LLVM.FunctionType(LLVM.Int32Type(), new[] { ps }, false);
            var fnGetter = LLVM.AddFunction(mod, "Foo_Get_Length", ftGetter);
            var bbGetter = LLVM.AppendBasicBlock(fnGetter, "entry");
            LLVM.PositionBuilderAtEnd(builder, bbGetter);
            var pThis2 = LLVM.GetParam(fnGetter, 0);
            var gep2 = LLVM.BuildGEP(builder, pThis2, new[] { zero, zero }, "");
            var value1 = LLVM.BuildLoad(builder, gep2, "");
            LLVM.BuildRet(builder, value1);

            // add test function
            var ft1 = LLVM.FunctionType(LLVM.Int32Type(), new LLVMTypeRef[0], false);
            var fn1 = LLVM.AddFunction(mod, "Test", ft1);
            var bb1 = LLVM.AppendBasicBlock(fn1, "entry");
            LLVM.PositionBuilderAtEnd(builder, bb1);
            var instance = LLVM.BuildCall(builder, fn, new LLVMValueRef[0], "instance");
            LLVM.BuildCall(builder, fnSetter, new[] { instance, LLVM.ConstInt(LLVM.Int32Type(), 23, false) }, "");
            var get = LLVM.BuildCall(builder, fnGetter, new[] { instance }, "get");
            LLVM.BuildRet(builder, get);


            LLVM.DisposeBuilder(builder);
            return fn1;
        }

        private static Function XAddClass(XModule mod)
        {
            var s = XType.Struct(new[] { XType.Int32 });
            var ps = XType.Pointer(s);

            var zero1 = Value.UInt32(0);
            var zero2 = Value.UInt32(0);
            var ctor = mod.AddFunction(new FunctionType(ps), "Foo_ctor");
            using (var builder = ctor.OpenBody())
            {
                var pThis = builder.Alloca(s);
                var gep = builder.GEP(pThis, new[] { zero1, zero2 });
                builder.Store(zero2, gep);
                builder.Ret(pThis);
            }

            var setter = mod.AddFunction(new FunctionType(XType.Void, ps, XType.Int32), "Foo_Set_Length");
            using (var builder = setter.OpenBody())
            {
                var pThis = setter.GetParam(0);
                var gep = builder.GEP(pThis, new[] { zero1, zero2 });
                var value = setter.GetParam(1);
                builder.Store(value, gep);
                builder.RetVoid();
            }

            var getter = mod.AddFunction(new FunctionType(XType.Int32, ps), "Foo_Get_Length");
            using (var builder = getter.OpenBody())
            {
                var pThis = getter.GetParam(0);
                var gep = builder.GEP(pThis, new[] { zero1, zero2 });
                var value = builder.Load(gep);
                builder.Ret(value);
            }


            var test = mod.AddFunction(new FunctionType(XType.Int32), "Test");
            using (var builder = test.OpenBody())
            {
                var instance = builder.Call(test, new Value[0]);
                builder.Call(setter, new[] { instance, Value.Int32(23) });
                var get = builder.Call(getter, new[] { instance });
                builder.Ret(get);
            }

            return test;
        }

        private static LLVMValueRef AddSumFunction(LLVMModuleRef mod)
        {
            LLVMTypeRef[] param_types = { LLVM.Int32Type(), LLVM.Int32Type() };
            LLVMTypeRef ret_type = LLVM.FunctionType(LLVM.Int32Type(), param_types, false);
            LLVMValueRef sum = LLVM.AddFunction(mod, "sum", ret_type);

            LLVMBasicBlockRef entry = LLVM.AppendBasicBlock(sum, "entry");

            var builder = LLVM.CreateBuilder();
            LLVM.PositionBuilderAtEnd(builder, entry);
            LLVMValueRef tmp = LLVM.BuildAdd(builder, LLVM.GetParam(sum, 0), LLVM.GetParam(sum, 1), "tmp");
            LLVM.BuildRet(builder, tmp);
            LLVM.DisposeBuilder(builder);
            return sum;
        }

        private static Function XAddSumFunction(XModule mod)
        {
            var ft = new FunctionType(XType.Int32, XType.Int32, XType.Int32);
            var function = mod.AddFunction(ft, "sum");

            using (var builder = function.OpenBody())
            {
                var p0 = function.GetParam(0);
                var p1 = function.GetParam(1);
                var tmp = builder.Add(p0, p1, "tmp");
                builder.Ret(tmp);
            }

            return function;
        }
    }
}
