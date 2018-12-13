using System;
using System.Collections.Generic;
using Repl.CodeAnalysis.Binding;
using Repl.CodeAnalysis.Syntax;
using XLang.Codegen;
using XLang.Codegen.Llvm;

namespace Repl.CodeAnalysis.CodeGen
{
    class Compiler
    {
        private readonly BasicBlock _basicBlock;
        private readonly Dictionary<VariableSymbol, Value> _variablePtrs;
        private readonly XModule _module;

        public Compiler()
        {
            _variablePtrs = new Dictionary<VariableSymbol, Value>();

            Statics.InitializeX86Target();
            _module = new XModule("test");
            var functionType = new FunctionType(XType.Int32);
            var function = _module.AddFunction(functionType, "__anon_expr");
            _basicBlock = function.AppendBasicBlock();
        }

        public void CompileAndRun(SyntaxTree syntaxTree, Dictionary<VariableSymbol, object> variables)
        {
            var binder = new Binder(variables);
            var expression = binder.BindExpression(syntaxTree.Root.Expression);

            Value v;
            using (var builder = new Builder())
            {
                builder.PositionAtEnd(_basicBlock);

                var codeGenerator = new CodeGenerator(builder, _variablePtrs);
                var value = codeGenerator.Generate(expression);

                v = builder.Ret(value);

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                _basicBlock.Print(Console.Out);
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