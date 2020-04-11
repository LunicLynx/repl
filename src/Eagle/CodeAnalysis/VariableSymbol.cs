using System;

namespace Repl.CodeAnalysis
{
    public abstract class VariableSymbol : Symbol
    {
        public override SymbolKind Kind => SymbolKind.Variable;
        public bool IsReadOnly { get; }
        public TypeSymbol Type { get; }

        protected VariableSymbol(string name, bool isReadOnly, TypeSymbol type)
            : base(name)
        {
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