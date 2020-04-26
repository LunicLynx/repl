using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Eagle.CodeAnalysis.Binding
{
    public interface IScope
    {
        IScope Parent { get; }

        bool TryDeclare(Symbol symbol);
        bool TryLookup(SymbolKind[] kinds, string name, out Symbol[] symbols);

        ImmutableArray<Symbol> GetDeclaredSymbols();
    }
}