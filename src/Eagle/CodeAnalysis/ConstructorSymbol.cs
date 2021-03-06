﻿using System.Collections.Immutable;
using System.Linq;

namespace Eagle.CodeAnalysis
{
    public class ConstructorSymbol : MemberSymbol, IInvokableSymbol
    {
        public ImmutableArray<ParameterSymbol> Parameters { get; }
        public bool Extern => false;

        public ConstructorSymbol(TypeSymbol type, ImmutableArray<ParameterSymbol> parameters)
            : base(type.Name, type)
        {
            Parameters = parameters;
        }

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Parameters.Select(p => p.ToString()))})";
        }

        public override SymbolKind Kind => SymbolKind.Constructor;
    }
}