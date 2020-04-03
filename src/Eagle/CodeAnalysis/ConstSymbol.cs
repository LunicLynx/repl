using System;

namespace Repl.CodeAnalysis
{
    public class ConstSymbol : Symbol
    {
        public override SymbolKind Kind => SymbolKind.Constant;
        public override string Name { get; }
        public TypeSymbol Type { get; }

        public ConstSymbol(string name, TypeSymbol type)
        {
            Name = name;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Name}: {Type}";
        }
    }
}