﻿using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundGlobalScope
    {
        public BoundGlobalScope Previous { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ImmutableArray<VariableSymbol> Variables { get; }
        public BoundStatement Statement { get; }

        public BoundGlobalScope(BoundGlobalScope previous, ImmutableArray<Diagnostic> diagnostics, ImmutableArray<VariableSymbol> variables, BoundStatement statement)
        {
            Previous = previous;
            Diagnostics = diagnostics;
            Variables = variables;
            Statement = statement;
        }
    }
}