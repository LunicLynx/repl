﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Eagle.IO;
using Repl.CodeAnalysis;
using Repl.CodeAnalysis.Syntax;

namespace Eagle
{
    class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("usage: ec <source-paths>");
                return;
            }

            var paths = GetFilePaths(args);
            var syntaxTrees = new List<SyntaxTree>();
            var hasErrors = false;

            foreach (var path in paths)
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine($"error: file '{path}' doesn't exist");
                    hasErrors = true;
                    continue;
                }
                var syntaxTree = SyntaxTree.Load(path);
                syntaxTrees.Add(syntaxTree);
            }

            if (hasErrors)
                return;

            var compilation = new Compilation(syntaxTrees.ToArray());
            var result = compilation.Evaluate(new Dictionary<Symbol, object>());

            if (!result.Diagnostics.Any())
            {
                if (result.Value != null)
                    Console.WriteLine(result.Value);
            }
            else
            {
                Console.Error.WriteDiagnostics(result.Diagnostics);
            }
        }

        private static IEnumerable<string> GetFilePaths(IEnumerable<string> paths)
        {
            var result = new SortedSet<string>();

            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    result.UnionWith(Directory.EnumerateFiles(path, "*.e", SearchOption.AllDirectories));
                }
                else
                {
                    result.Add(path);
                }
            }

            return result;
        }
    }
}