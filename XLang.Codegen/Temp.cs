using System;
using LLVMSharp;
using XLang.Codegen.Llvm;

namespace XLang.Codegen
{
    public static class Temp
    {
        public static void AddEntryPoint(XModule mod)
        {
            var proto = new FunctionType(XType.Void);
            var mainlang = mod.AddFunction(proto, "mainlang");

            using (var builder = mainlang.OpenBody())
            {
                var str = builder.GlobalString("hello world\r\0", ".str");
                var index = Const.Int64(0);
                var cast210 = builder.GEP(str, new[] { index, index }, "cast210");
                var puts = mod.GetFunction("puts");
                builder.Call(puts, new[] { cast210 }, "");
                builder.RetVoid();
            }
        }

        public static void AddExternalPuts(XModule mod)
        {
            var proto = new FunctionType(XType.Int32, XType.Int32, XType.Int32);
            mod.AddFunction(proto, "puts");
        }


        public static void AddSumFunction(XModule mod)
        {
            var functionType = new FunctionType(
                XType.Int32, XType.Int32, XType.Int32);
            var sum = mod.AddFunction(functionType, "sum");
            using (var builder = sum.OpenBody())
            {
                var tmp = builder.Add(sum.GetParam(0), sum.GetParam(1), "tmp");
                builder.Ret(tmp);
            }
        }

        public static void Demo()
        {
            var mod = new XModule("LLVMSharpIntro");

            Temp.AddSumFunction(mod);
            Temp.AddExternalPuts(mod);
            Temp.AddEntryPoint(mod);

            if (!mod.Verify(LLVMVerifierFailureAction.LLVMPrintMessageAction, out var error))
            {
                Console.WriteLine($"Error: {error}");
            }

            LLVM.InitializeX86TargetMC();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86AsmParser();
            LLVM.InitializeX86AsmPrinter();

            mod.Dump();

            var filename = "entry.obj";
            mod.TryEmitObj(filename, out error);
            Cl.InvokeCl(filename);
            Cl.InvokeMain();

            Console.ReadKey();
        }
    }
}