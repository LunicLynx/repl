﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Repl.CodeAnalysis.Binding;
using Repl.CodeAnalysis.Lowering;
using Repl.CodeAnalysis.Syntax;

namespace Repl.CodeAnalysis
{
    public class Compilation
    {
        private BoundGlobalScope _globalScope;
        public Compilation Previous { get; }
        public SyntaxTree SyntaxTree { get; }

        public Compilation(SyntaxTree syntaxTree)
        : this(null, syntaxTree) { }

        private Compilation(Compilation previous, SyntaxTree syntaxTree)
        {
            Previous = previous;
            SyntaxTree = syntaxTree;
        }

        internal BoundGlobalScope GlobalScope
        {
            get
            {
                if (_globalScope == null)
                {
                    var globalScope = Binder.BindGlobalScope(Previous?.GlobalScope, SyntaxTree.Root);
                    Interlocked.CompareExchange(ref _globalScope, globalScope, null);
                }

                return _globalScope;
            }
        }

        public Compilation ContinueWith(SyntaxTree syntaxTree)
        {
            return new Compilation(this, syntaxTree);
        }

        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables, Dictionary<FunctionSymbol, Delegate> functions )
        {
            var diagnostics = SyntaxTree.Diagnostics.Concat(GlobalScope.Diagnostics).ToImmutableArray();
            if (diagnostics.Any())
                return new EvaluationResult(diagnostics, null);

            var statement = GetStatement();
            var evaluator = new Evaluator(statement, variables, functions);
            var value = evaluator.Evaluate();
            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
        }

        public void Emit(string fileName)
        {

        }

        private BoundBlockStatement GetStatement()
        {
            var statement = Lowerer.Lower(GlobalScope.Statement);
            return statement;
        }

        public void Print(Action<BoundNode> print)
        {
            print(GetStatement());
        }
    }
}