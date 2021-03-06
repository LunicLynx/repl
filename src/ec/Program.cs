﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Eagle.CodeAnalysis;
using Eagle.CodeAnalysis.Syntax;
using Eagle.IO;

namespace Eagle
{
    class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("usage: ec <source-paths>");
                return 1;
            }

            var paths = GetFilePaths(args);
            var syntaxTrees = new List<SyntaxTree>();
            var hasErrors = false;

            // TODO load core.e into syntax tree
            syntaxTrees.Add(SyntaxTree.Load("C:\\Users\\Florian\\Source\\repos\\repl\\core\\core.e"));

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
                return 1;

            var compilation = Compilation.Create(syntaxTrees.ToArray());

            var diagnostics = compilation.EmitBinary("C:\\Users\\Florian\\Source\\repos\\repl\\samples\\hello");
            if (diagnostics.Any())
            {
                Console.Error.WriteDiagnostics(diagnostics);
                return 1;
            }

            return 0;
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
