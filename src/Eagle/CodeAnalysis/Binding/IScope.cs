﻿using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public interface IScope
    {
        IScope Parent { get; }

        bool TryDeclare(Symbol symbol);


        bool TryLookup(string name, out Symbol symbol);


        ImmutableArray<Symbol> GetDeclaredSymbols();

    }
}