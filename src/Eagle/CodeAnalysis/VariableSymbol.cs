using System;

namespace Repl.CodeAnalysis
{
    public class VariableSymbol : Symbol
    {
        public override SymbolKind Kind => SymbolKind.Variable;
        public override string Name { get; }
        public bool IsReadOnly { get; }
        public TypeSymbol Type { get; }

        public VariableSymbol(string name, bool isReadOnly, TypeSymbol type)
        {
            Name = name;
            IsReadOnly = isReadOnly;
            Type = type;
        }

        public override string ToString() => Name;
    }

    public class LocalVariableSymbol : VariableSymbol
    {
        public LocalVariableSymbol(string name, bool isReadOnly, TypeSymbol type) 
            : base(name, isReadOnly, type)
        {
        }
    }

    public class GlobalVariableSymbol : VariableSymbol
    {
        public GlobalVariableSymbol(string name, bool isReadOnly, TypeSymbol type)
            : base(name, isReadOnly, type)
        {
        }
    }
}