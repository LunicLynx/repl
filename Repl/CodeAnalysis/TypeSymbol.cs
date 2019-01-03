using System;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis
{
    public class TypeSymbol : Symbol
    {
        public override string Name { get; }
        public Type ClrType { get; }
        public ImmutableArray<MemberSymbol> Members { get; }

        public TypeSymbol(string name, Type clrType, ImmutableArray<MemberSymbol> members)
        {
            Name = name;
            ClrType = clrType;
            Members = members;
        }

        public override string ToString()
        {
            return Name;
        }

        public bool IsAssignableFrom(TypeSymbol other)
        {
            // TODO real implementation
            return ClrType.IsAssignableFrom(other.ClrType);
        }

        public static TypeSymbol Void = new TypeSymbol("void", typeof(void), ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol Bool = new TypeSymbol("bool", typeof(bool), ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol I8 = new TypeSymbol("i8", typeof(sbyte), ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol I16 = new TypeSymbol("i16", typeof(short), ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol I32 = new TypeSymbol("i32", typeof(int), ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol I64 = new TypeSymbol("i64", typeof(long), ImmutableArray<MemberSymbol>.Empty);
        //public static TypeSymbol I128 = new TypeSymbol("i128", typeof(Int128));
        public static TypeSymbol U8 = new TypeSymbol("u8", typeof(byte), ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol U16 = new TypeSymbol("u16", typeof(ushort), ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol U32 = new TypeSymbol("u32", typeof(uint), ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol U64 = new TypeSymbol("u64", typeof(ulong), ImmutableArray<MemberSymbol>.Empty);
        //public static TypeSymbol U128 = new TypeSymbol("u128", typeof(sbyte));
        public static TypeSymbol Int = new TypeSymbol("int", IntPtr.Size == 4 ? typeof(int) : typeof(long), ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol Uint = new TypeSymbol("uint", IntPtr.Size == 4 ? typeof(uint) : typeof(ulong), ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol String = new TypeSymbol("string", typeof(string), ImmutableArray.Create<MemberSymbol>(new PropertySymbol("Length", I32)));
    }
}