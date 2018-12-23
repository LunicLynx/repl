using System;

namespace Repl.CodeAnalysis
{
    public class TypeSymbol : Symbol
    {
        public override string Name { get; }
        public Type ClrType { get; }

        public TypeSymbol(string name, Type clrType)
        {
            Name = name;
            ClrType = clrType;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}