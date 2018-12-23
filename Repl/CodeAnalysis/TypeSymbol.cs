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

        public static TypeSymbol Void = new TypeSymbol("void", typeof(void));
        public static TypeSymbol Int32 = new TypeSymbol("int", typeof(int));
    }
}