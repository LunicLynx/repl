﻿using System;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis
{
    public class TypeSymbol : Symbol
    {
        private bool _locked;
        private ImmutableArray<MemberSymbol> _members;
        private ImmutableArray<TypeSymbol> _baseType;
        public override string Name { get; }
        public Type ClrType { get; }

        public ImmutableArray<TypeSymbol> BaseType
        {
            get => _baseType;
            set
            {
                ThrowIfLocked();
                _baseType = value;
            }
        }

        public ImmutableArray<MemberSymbol> Members
        {
            get => _members;
            set
            {
                ThrowIfLocked();
                _members = value;
            }
        }

        //private readonly List<MemberSymbol> _members = new List<MemberSymbol>();

        //public MemberSymbol[] Members => _members.ToArray();

        public TypeSymbol(string name, Type clrType, ImmutableArray<TypeSymbol> baseType, ImmutableArray<MemberSymbol> members)
        {
            Name = name;
            ClrType = clrType;
            BaseType = baseType;
            Members = members;
            //_members.AddRange(members);
        }

        public void Lock()
        {
            _locked = true;
        }

        private void ThrowIfLocked()
        {
            if (_locked)
                throw new Exception("Locked");
        }

        public override string ToString()
        {
            //var sb = new StringBuilder();
            //sb.Append($"struct {Name} ");
            //if (BaseType != default)
            //    sb.Append($": {string.Join(", ", BaseType.Select(te))} ");
            //if (Members != default)
            //{
            //    sb.Append($"{{ {string.Join(" ", Members)} }}");
            //}
            //return sb.ToString();
            return Name;
        }

        public bool IsAssignableFrom(TypeSymbol other)
        {
            // TODO real implementation
            return ClrType.IsAssignableFrom(other.ClrType);
        }

        public static TypeSymbol Void = new TypeSymbol("void", typeof(void), ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol Bool = new TypeSymbol("bool", typeof(bool), ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol I8 = new TypeSymbol("i8", typeof(sbyte), ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol I16 = new TypeSymbol("i16", typeof(short), ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol I32 = new TypeSymbol("i32", typeof(int), ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol I64 = new TypeSymbol("i64", typeof(long), ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty);
        //public static TypeSymbol I128 = new TypeSymbol("i128", typeof(Int128));
        public static TypeSymbol U8 = new TypeSymbol("u8", typeof(byte), ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol U16 = new TypeSymbol("u16", typeof(ushort), ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol U32 = new TypeSymbol("u32", typeof(uint), ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol U64 = new TypeSymbol("u64", typeof(ulong), ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty);
        //public static TypeSymbol U128 = new TypeSymbol("u128", typeof(sbyte));
        public static TypeSymbol Int = new TypeSymbol("int", IntPtr.Size == 4 ? typeof(int) : typeof(long), ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol Uint = new TypeSymbol("uint", IntPtr.Size == 4 ? typeof(uint) : typeof(ulong), ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty);
        public static TypeSymbol String = new TypeSymbol("String", typeof(string), ImmutableArray<TypeSymbol>.Empty, ImmutableArray.Create<MemberSymbol>(new PropertySymbol("Length", I32)));
    }
}