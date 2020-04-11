using System;

namespace Repl.CodeAnalysis
{
    public class AliasSymbol : Symbol
    {
        public override SymbolKind Kind => SymbolKind.Alias;
        public TypeSymbol Type { get; }

        public AliasSymbol(string name, TypeSymbol type)
            : base(name)
        {
            Type = type;
        }

        public override string ToString()
        {
            return $"{Name} = {Type}";
        }
    }
}