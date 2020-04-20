﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using Eagle.CodeAnalysis.Binding;
using Eagle.CodeAnalysis.Syntax;
using ReflectionBindingFlags = System.Reflection.BindingFlags;

namespace Eagle.CodeAnalysis
{
    public class Compilation
    {
        public bool IsScript { get; }
        private BoundGlobalScope _globalScope;
        public Compilation Previous { get; }
        public ImmutableArray<SyntaxTree> SyntaxTrees { get; }

        private Compilation(bool isScript, Compilation previous, params SyntaxTree[] syntaxTrees)
        {
            IsScript = isScript;
            Previous = previous;
            SyntaxTrees = syntaxTrees.ToImmutableArray();
        }

        public static Compilation Create(params SyntaxTree[] syntaxTrees)
        {
            return new Compilation(isScript: false, previous: null, syntaxTrees);
        }

        public static Compilation CreateScript(Compilation previous, params SyntaxTree[] syntaxTrees)
        {
            return new Compilation(isScript: true, previous, syntaxTrees);
        }

        internal BoundGlobalScope GlobalScope
        {
            get
            {
                if (_globalScope == null)
                {
                    var globalScope = Binder.BindGlobalScope(IsScript, Previous?.GlobalScope, SyntaxTrees);
                    Interlocked.CompareExchange(ref _globalScope, globalScope, null);
                }

                return _globalScope;
            }
        }

        public IEnumerable<Symbol> GetSymbols()
        {
            var submission = this;
            var seenSymbolNames = new HashSet<string>();

            while (submission != null)
            {
                const ReflectionBindingFlags bindingFlags =
                    ReflectionBindingFlags.Static |
                    ReflectionBindingFlags.Public |
                    ReflectionBindingFlags.NonPublic;
                //var builtinFunctions = typeof(BuiltinFunctions)
                //    .GetFields(bindingFlags)
                //    .Where(fi => fi.FieldType == typeof(FunctionSymbol))
                //    .Select(fi => (FunctionSymbol)fi.GetValue(obj: null))
                //    .ToList();

                var functions = submission.GlobalScope.Symbols.OfType<FunctionSymbol>();
                foreach (var function in functions)
                    if (seenSymbolNames.Add(function.Name))
                        yield return function;

                var variables = submission.GlobalScope.Symbols.OfType<VariableSymbol>();
                foreach (var variable in variables)
                    if (seenSymbolNames.Add(variable.Name))
                        yield return variable;

                //foreach (var builtin in builtinFunctions)
                //    if (seenSymbolNames.Add(builtin.Name))
                //        yield return builtin;

                submission = submission.Previous;
            }
        }

        private BoundProgram GetProgram()
        {
            var previous = Previous == null ? null : Previous.GetProgram();
            return Binder.BindProgram(IsScript, previous, GlobalScope);
        }

        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables)
        {
            var parseDiagnostics = SyntaxTrees.SelectMany(st => st.Diagnostics);

            var diagnostics = parseDiagnostics.Concat(GlobalScope.Diagnostics).ToImmutableArray();
            if (diagnostics.Any())
                return new EvaluationResult(diagnostics, null);

            var program = GetProgram();

            //var appPath = Environment.GetCommandLineArgs()[0];
            //var appDirectory = Path.GetDirectoryName(appPath);
            //var cfgPath = Path.Combine(appDirectory, "cfg.dot");
            //var cfgStatement = !program.Statement.Statements.Any() && program.Functions.Any()
            //    ? program.Functions.Last().Value
            //    : program.Statement;
            //var cfg = ControlFlowGraph.Create(cfgStatement);
            //using (var streamWriter = new StreamWriter(cfgPath))
            //    cfg.WriteTo(streamWriter);

            if (program.Diagnostics.Any())
                return new EvaluationResult(program.Diagnostics.ToImmutableArray(), null);

            var evaluator = new Evaluator(program, variables);
            var value = evaluator.Evaluate();
            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
        }

        public void EmitBinary()
        {
            var parseDiagnostics = SyntaxTrees.SelectMany(st => st.Diagnostics);

            var diagnostics = parseDiagnostics.Concat(GlobalScope.Diagnostics).ToImmutableArray();
            if (diagnostics.Any())
                return;

            var program = GetProgram();
            if (program.Diagnostics.Any())
                return;

            var generator = new CodeGen.CodeGenerator(program, GlobalScope);
            generator.Generate();
        }

        public void EmitTree(TextWriter writer)
        {
            if (GlobalScope.MainFunction != null)
                EmitTree(GlobalScope.MainFunction, writer);
            else if (GlobalScope.ScriptFunction != null)
                EmitTree(GlobalScope.ScriptFunction, writer);
        }

        public void EmitTree(FunctionSymbol symbol, TextWriter writer)
        {
            var program = GetProgram();
            //symbol.WriteTo(writer);
            writer.WriteLine();
            if (!program.Functions.TryGetValue(symbol, out var body))
                return;
            //body.WriteTo(writer);
        }
    }
}