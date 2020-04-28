using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Eagle.CodeAnalysis
{
    public class TypeSymbol : Symbol
    {
        public static readonly TypeSymbol Error = new TypeSymbol("?", SpecialType.Error);
        public static readonly TypeSymbol String = new TypeSymbol("String", SpecialType.String);
        public static readonly TypeSymbol Char = new TypeSymbol("Char", SpecialType.Char);
        public static readonly TypeSymbol Void = new TypeSymbol("Void", SpecialType.Void);
        public static readonly TypeSymbol I8 = new TypeSymbol("Int8", SpecialType.I8);
        public static readonly TypeSymbol I16 = new TypeSymbol("Int16", SpecialType.I16);
        public static readonly TypeSymbol I32 = new TypeSymbol("Int32", SpecialType.I32);
        public static readonly TypeSymbol I64 = new TypeSymbol("Int64", SpecialType.I64);
        public static readonly TypeSymbol U8 = new TypeSymbol("UInt8", SpecialType.U8);
        public static readonly TypeSymbol U16 = new TypeSymbol("UInt16", SpecialType.U16);
        public static readonly TypeSymbol U32 = new TypeSymbol("UInt32", SpecialType.U32);
        public static readonly TypeSymbol U64 = new TypeSymbol("UInt64", SpecialType.U64);
        public static readonly TypeSymbol Bool = new TypeSymbol("Boolean", SpecialType.Bool);
        public static readonly TypeSymbol UInt = new TypeSymbol("UInt", SpecialType.UInt);
        public static readonly TypeSymbol Int = new TypeSymbol("Int", SpecialType.Int);
        public static readonly TypeSymbol Any = new TypeSymbol("Any", SpecialType.Any);

        public TypeSymbol ElementType { get; }
        public int Dimensions { get; }
        public bool IsPointer { get; }
        public bool IsReference { get; }
        public bool IsArray { get; }

        public override SymbolKind Kind => SymbolKind.Type;

        public SpecialType SpecialType { get; } = SpecialType.None;

        private readonly string _name;
        public string Name
        {
            get
            {
                if (IsPointer)
                    return ElementType.Name + "*";
                return _name;
            }
        }

        public ImmutableArray<TypeSymbol> BaseType { get; set; }

        public ImmutableArray<MemberSymbol> Members { get; set; }

        private TypeSymbol(string name, SpecialType specialType) : this(name)
        {
            SpecialType = specialType;
        }

        public TypeSymbol(string name, ImmutableArray<TypeSymbol> baseType, ImmutableArray<MemberSymbol> members)
            : base(name)
        {
            _name = name;
            BaseType = baseType;
            Members = members;
        }

        public TypeSymbol(string name)
            : this(name, ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty) { }


        private TypeSymbol(TypeSymbol elementType, bool pointer, bool isReference)
            : base(null)
        {
            ElementType = elementType;
            IsPointer = pointer;
            IsReference = isReference;
        }

        private TypeSymbol(TypeSymbol elementType, int dimensions)
            : base(null)
        {
            ElementType = elementType;
            Dimensions = dimensions;
            IsArray = true;
        }

        public override string ToString()
        {
            if (IsPointer)
                return ElementType + "*";

            if (IsReference)
                return ElementType + "&";

            if (IsArray)
                return ElementType + "[" + new string(',', Dimensions - 1) + "]";

            if (SpecialType != SpecialType.None && SpecialType != SpecialType.Error)
                return SpecialType.ToString().ToLower();
            return Name;
        }

        public TypeSymbol MakePointer()
        {
            return new TypeSymbol(this, true, false);
        }

        public TypeSymbol MakeReference()
        {
            return new TypeSymbol(this, false, true);
        }

        public TypeSymbol MakeArray(int dimensions)
        {
            return new TypeSymbol(this, dimensions);
        }

        public static bool operator ==(TypeSymbol? a, TypeSymbol? b)
        {
            var aNull = ReferenceEquals(null, a);
            var bNull = ReferenceEquals(null, b);
            if (aNull && bNull) return true;
            if (aNull || bNull) return false;
            if (a.IsPointer && b.IsPointer)
                return a.ElementType == b.ElementType;
            if (a.IsPointer || b.IsPointer)
                return false;
            return ReferenceEquals(a, b);
        }

        public static bool operator !=(TypeSymbol? a, TypeSymbol? b)
        {
            return !(a == b);
        }

        protected bool Equals(TypeSymbol other)
        {
            return _name == other._name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TypeSymbol)obj);
        }

        public override int GetHashCode()
        {
            return (_name != null ? _name.GetHashCode() : 0);
        }


    }
}