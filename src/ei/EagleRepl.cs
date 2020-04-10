using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Eagle.IO;
using Repl.CodeAnalysis;
using Repl.CodeAnalysis.CodeGen;
using Repl.CodeAnalysis.Syntax;
using Repl.CodeAnalysis.Text;

namespace Repl
{
    public class EagleRepl : Repl
    {
        private Compilation _previous;
        private NCompilation _nPrevious;
        private bool _showTree = false;
        private bool _showProgram = true;
        private bool _compile = false;

        private readonly Dictionary<Symbol, object> _variables = new Dictionary<Symbol, object>();

        private readonly Compiler _compiler = new Compiler();

        public override void Run()
        {
            var path = "guess.e";
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
                var isIdentifier = token.Kind == TokenKind.Identifier;
                var isNumber = token.Kind == TokenKind.NumberLiteral;
                var isString = token.Kind == TokenKind.StringLiteral;

                if (isKeyword)
                    Console.ForegroundColor = ConsoleColor.Blue;
                else if (isIdentifier)
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                else if (isNumber)
                    Console.ForegroundColor = ConsoleColor.Cyan;
                else if (isString)
                    Console.ForegroundColor = ConsoleColor.Magenta;
                else
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
                case "#cls":
                    Console.Clear();
                    break;
                case "#reset":
                    _previous = null;
                    _variables.Clear();
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
            var nCompilation = _nPrevious == null
                ? new NCompilation(compilation)
                : _nPrevious.ContinueWith(compilation);

            if (_showTree)
            {
                var printer = new Printer();
                foreach (var statement in syntaxTree.Root.Members)
                {
                    printer.Print(statement);
                }
            }

            if (_showProgram)
            {
                var printer = new Printer();
                printer.Print(compilation);
            }

            var result = compilation.Evaluate(_variables);

            if (!result.Diagnostics.Any())
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(result.Value);
                Console.ResetColor();

                // TODO if (compile) - if activated we would need to re-emit the whole module
                //if (_compile)
                //    _compiler.CompileAndRun(compilation, _variables);

                _previous = compilation;
            }
            else
            {
                Console.Out.WriteDiagnostics(result.Diagnostics);
            }
        }
    }
}