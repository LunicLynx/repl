using System;
using System.Collections.Generic;
using XLang.Codegen;
using XLang.Codegen.Llvm;

namespace Repl.CodeAnalysis.CodeGen
{
    internal class Compiler
    {
        private BasicBlock _basicBlock;
        private readonly Dictionary<VariableSymbol, Value> _variablePtrs;
        private readonly XModule _module;
        private readonly Function _function;

        public Compiler()
        {
            _variablePtrs = new Dictionary<VariableSymbol, Value>();

            Statics.InitializeX86Target();
            _module = new XModule("test");
            var functionType = new FunctionType(XType.Int32);
            _function = _module.AddFunction(functionType, "__anon_expr");
            _basicBlock = _function.AppendBasicBlock();
        }

        public void CompileAndRun(Compilation compilation, Dictionary<VariableSymbol, object> variables)
        {
            var globalScope = compilation.GlobalScope;

            Value v;
            using (var builder = new Builder())
            {
                builder.PositionAtEnd(_basicBlock);

                var codeGenerator = new CodeGenerator(builder, _variablePtrs);
                var value = codeGenerator.Generate(globalScope.Statement);
                _basicBlock = builder.GetInsertBlock();

                v = builder.Ret(value);

                //Function.ViewCfg();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                _function.Print(Console.Out);
                Console.WriteLine();
                Console.ResetColor();

                var objFilename = "entry.obj";
                if (_module.TryEmitObj(objFilename, out var error))
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Cl.InvokeCl(objFilename);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Cl.InvokeMain();
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(error);
                    Console.ResetColor();
                }

                v.RemoveFromParent();

            }
        }
    }
}