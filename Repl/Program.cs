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
    internal class Program
    {
        private static void Main(string[] args)
        {
            var showTree = false;
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
                        Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees");
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
                var result = compilation.Evaluate(variables);

                if (showTree)
                {
                    Print(syntaxTree.Root.Statement);
                }

                if (!result.Diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(result.Value);
                    Console.ResetColor();

                    // TODO if (compile) - if activated we would need to re-emit the whole module
                    if(compile)
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

        private static void Print(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.Write(indent);
            Console.Write(marker);

            Console.ForegroundColor = node is Token ? ConsoleColor.Blue : ConsoleColor.Cyan;

            if (node is Token t)
            {
                Console.Write(t);
            }
            else
            {
                Console.Write(node.GetType().Name);
            }

            Console.ResetColor();

            Console.WriteLine();

            indent += isLast ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                Print(child, indent, child == lastChild);
        }
    }
}
