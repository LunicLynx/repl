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
        public static TypeSymbol Bool = new TypeSymbol("bool", typeof(bool));
        public static TypeSymbol I8 = new TypeSymbol("i8", typeof(sbyte));
        public static TypeSymbol I16 = new TypeSymbol("i16", typeof(short));
        public static TypeSymbol I32 = new TypeSymbol("i32", typeof(int));
        public static TypeSymbol I64 = new TypeSymbol("i64", typeof(long));
        //public static TypeSymbol I128 = new TypeSymbol("i128", typeof(Int128));
        public static TypeSymbol U8 = new TypeSymbol("u8", typeof(byte));
        public static TypeSymbol U16 = new TypeSymbol("u16", typeof(ushort));
        public static TypeSymbol U32 = new TypeSymbol("u32", typeof(uint));
        public static TypeSymbol U64 = new TypeSymbol("u64", typeof(ulong));
        //public static TypeSymbol U128 = new TypeSymbol("u128", typeof(sbyte));
        public static TypeSymbol Int = new TypeSymbol("int", IntPtr.Size == 4 ? typeof(int) : typeof(long));
        public static TypeSymbol Uint = new TypeSymbol("uint", IntPtr.Size == 4 ? typeof(uint) : typeof(ulong));
        public static TypeSymbol String = new TypeSymbol("string", typeof(string));
    }
}