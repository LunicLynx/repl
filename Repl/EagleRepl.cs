using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Repl.CodeAnalysis;
using Repl.CodeAnalysis.CodeGen;
using Repl.CodeAnalysis.Syntax;
using Repl.CodeAnalysis.Text;

namespace Repl
{
    public class EagleRepl : Repl
    {
        private Compilation _previous;
        private bool _showTree = false;
        private bool _showProgram = true;
        private bool _compile = false;

        private readonly Dictionary<ConstSymbol, object> _constants = new Dictionary<ConstSymbol, object>();

        private readonly Dictionary<VariableSymbol, object> _variables = new Dictionary<VariableSymbol, object>();

        private readonly Dictionary<FunctionSymbol, Delegate> _functions =
            new Dictionary<FunctionSymbol, Delegate>();

        private readonly Compiler _compiler = new Compiler();

        public override void Run()
        {
            var path = "demo.es";
            if (File.Exists(path))
                Load(path);
            base.Run();
        }

        protected override void RenderLine(string line)
        {
            var tokens = SyntaxTree.ParseTokens(line);
            foreach (var token in tokens)
            {
                var isKeyword = token.Kind.ToString().EndsWith("Keyword");
                var isNumber = token.Kind == TokenKind.Number;
                if (isKeyword)
                    Console.ForegroundColor = ConsoleColor.Blue;
                else if (!isNumber)
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.Write(token.Text);

                Console.ResetColor();
            }
        }

        protected override void EvaluateMetaCommand(string input)
        {
            switch (input)
            {
                case "#showTree":
                    _showTree = !_showTree;
                    Console.WriteLine(_showTree ? "Showing parse trees." : "Not showing parse trees.");
                    break;
                case "#showProgram":
                    _showProgram = !_showProgram;
                    Console.WriteLine(_showProgram ? "Showing bound trees." : "Not showing bound trees.");
                    break;
                case "#reset":
                    _previous = null;
                    break;
                case "#emit":
                    _compile = !_compile;
                    Console.WriteLine(_compile ? "Emit binary." : "Not emitting binary.");
                    break;
                default:
                    base.EvaluateMetaCommand(input);
                    break;
            }
        }

        protected override bool IsCompleteSubmission(string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;

            var syntaxTree = SyntaxTree.Parse(text);

            return !syntaxTree.Diagnostics.Any();
        }

        protected override void EvaluateSubmission(string text)
        {
            var syntaxTree = SyntaxTree.Parse(text);

            var compilation = _previous == null
                ? new Compilation(syntaxTree)
                : _previous.ContinueWith(syntaxTree);

            if (_showTree)
            {
                var printer = new Printer();
                foreach (var statement in syntaxTree.Root.Nodes)
                {
                    printer.Print(statement);
                }
            }

            if (_showProgram)
            {
                var printer = new Printer();
                printer.Print(compilation);
            }

            var result = compilation.Evaluate(_constants, _variables, _functions);

            if (!result.Diagnostics.Any())
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(result.Value);
                Console.ResetColor();

                // TODO if (compile) - if activated we would need to re-emit the whole module
                if (_compile)
                    _compiler.CompileAndRun(compilation, _variables);

                _previous = compilation;
            }
            else
            {
                var text1 = syntaxTree.Text;

                foreach (var diagnostic in result.Diagnostics)
                {
                    var lineIndexStart = text1.GetLineIndex(diagnostic.Span.Start);
                    var lineIndexEnd = text1.GetLineIndex(diagnostic.Span.End);
                    var lineStart = text1.Lines[lineIndexStart];
                    var lineEnd = text1.Lines[lineIndexEnd];
                    var lineNumberStart = lineIndexStart + 1;
                    var character = diagnostic.Span.Start - lineStart.Start + 1;

                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"({lineNumberStart}, {character}): ");
                    Console.WriteLine(diagnostic);
                    Console.ResetColor();

                    var prefixSpan = TextSpan.FromBounds(lineStart.Start, diagnostic.Span.Start);
                    var suffixSpan = TextSpan.FromBounds(diagnostic.Span.End, lineEnd.End);

                    var prefix = text1.ToString(prefixSpan);
                    var error = text1.ToString(diagnostic.Span);
                    var suffix = text1.ToString(suffixSpan);

                    Console.Write("    ");
                    Console.Write(prefix);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(error);
                    Console.ResetColor();

                    Console.Write(suffix);

                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }
    }
}