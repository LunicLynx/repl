using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repl.CodeAnalysis;
using Repl.CodeAnalysis.CodeGen;
using Repl.CodeAnalysis.Syntax;
using Repl.CodeAnalysis.Text;

namespace Repl
{
    internal class Repl
    {
        public void Run()
        {
            var showTree = false;
            var showProgram = true;
            var compile = false;
            var compiler = new Compiler();
            var variables = new Dictionary<VariableSymbol, object>();
            var textBuilder = new StringBuilder();
            Compilation previous = null;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(textBuilder.Length == 0 ? "» " : "· ");
                Console.ResetColor();

                var input = Console.ReadLine();
                var isBlank = string.IsNullOrWhiteSpace(input);

                if (textBuilder.Length == 0)
                {
                    if (isBlank)
                        break;

                    if (input == "#showTree")
                    {
                        showTree = !showTree;
                        Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees.");
                        continue;
                    }

                    if (input == "#showProgram")
                    {
                        showProgram = !showProgram;
                        Console.WriteLine(showProgram ? "Showing bound trees." : "Not showing bound trees.");
                        continue;
                    }

                    if (input == "#cls")
                    {
                        Console.Clear();
                        continue;
                    }

                    if (input == "#reset")
                    {
                        previous = null;
                        continue;
                    }

                    if (input == "#emit")
                    {
                        compile = !compile;
                        Console.WriteLine(showTree ? "Emit binary." : "Not emitting binary.");
                        continue;
                    }
                }

                textBuilder.AppendLine(input);
                var text = textBuilder.ToString();

                var syntaxTree = SyntaxTree.Parse(text);

                if (!isBlank && syntaxTree.Diagnostics.Any())
                    continue;

                var compilation = previous == null
                        ? new Compilation(syntaxTree)
                        : previous.ContinueWith(syntaxTree);

                if (showTree)
                {
                    var printer = new Printer();
                    printer.Print(syntaxTree.Root.Statement);
                }

                if (showProgram)
                {
                    var printer = new Printer();
                    printer.Print(compilation);

                }

                var result = compilation.Evaluate(variables);

                if (!result.Diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(result.Value);
                    Console.ResetColor();

                    // TODO if (compile) - if activated we would need to re-emit the whole module
                    if (compile)
                        compiler.CompileAndRun(compilation, variables);

                    previous = compilation;
                }
                else
                {
                    var text1 = syntaxTree.Text;

                    foreach (var diagnostic in result.Diagnostics)
                    {
                        var lineIndex = text1.GetLineIndex(diagnostic.Span.Start);
                        var line = text1.Lines[lineIndex];
                        var lineNumber = lineIndex + 1;
                        var character = diagnostic.Span.Start - line.Start + 1;

                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"({lineNumber}, {character}): ");
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        var prefixSpan = TextSpan.FromBounds(line.Start, diagnostic.Span.Start);
                        var suffixSpan = TextSpan.FromBounds(diagnostic.Span.End, line.End);

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

                textBuilder.Clear();
            }

        }
    }

    internal class EagleRepl : Repl
    {

    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var repl = new Repl();
            repl.Run();
        }
    }
}
