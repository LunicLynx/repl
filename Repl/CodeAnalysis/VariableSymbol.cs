using System;

namespace Repl.CodeAnalysis
{
    public class VariableSymbol : Symbol
    {
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
}