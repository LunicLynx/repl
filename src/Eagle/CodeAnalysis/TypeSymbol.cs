using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Repl.CodeAnalysis
{
    public enum SpecialType
    {
        None,
        String,
        Void,
        I8,
        I16,
        I32,
        I64,
        U8,
        U16,
        U32,
        U64,
        Bool,
        UInt,
        Int,
        Char,
        Error
    }

    [DebuggerDisplay("{Name}")]
    public class TypeSymbol : Symbol
    {
        public TypeSymbol ElementType { get; }
        public bool IsPointer { get; }
        private bool _locked;
        private ImmutableArray<MemberSymbol> _members;
        private ImmutableArray<TypeSymbol> _baseType;

        private readonly string _name;
        public override SymbolKind Kind => SymbolKind.Type;

        public SpecialType SpecialType { get; } = SpecialType.None;

        public override string Name
        {
            get
            {
                if (IsPointer)
                    return ElementType.Name + "*";
                return _name;
            }
        }

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

        public static readonly TypeSymbol Error = new TypeSymbol("?", SpecialType.Error);
        public static TypeSymbol String = new TypeSymbol("String", SpecialType.String);
        public static TypeSymbol Char = new TypeSymbol("Char", SpecialType.Char);
        public static TypeSymbol Void = new TypeSymbol("Void", SpecialType.Void);
        public static TypeSymbol I8 = new TypeSymbol("Int8", SpecialType.I8);
        public static TypeSymbol I16 = new TypeSymbol("Int16", SpecialType.I16);
        public static TypeSymbol I32 = new TypeSymbol("Int32", SpecialType.I32);
        public static TypeSymbol I64 = new TypeSymbol("Int64", SpecialType.I64);
        public static TypeSymbol U8 = new TypeSymbol("UInt8", SpecialType.U8);
        public static TypeSymbol U16 = new TypeSymbol("UInt16", SpecialType.U16);
        public static TypeSymbol U32 = new TypeSymbol("UInt32", SpecialType.U32);
        public static TypeSymbol U64 = new TypeSymbol("UInt64", SpecialType.U64);
        public static TypeSymbol Bool = new TypeSymbol("Boolean", SpecialType.Bool);
        public static TypeSymbol UInt = new TypeSymbol("UInt", SpecialType.UInt);
        public static TypeSymbol Int = new TypeSymbol("Int", SpecialType.Int);

        private TypeSymbol(string name, SpecialType specialType) : this(name)
        {
            SpecialType = specialType;
        }

        public TypeSymbol(string name, ImmutableArray<TypeSymbol> baseType, ImmutableArray<MemberSymbol> members)
        {
            _name = name;
            BaseType = baseType;
            Members = members;
        }

        public TypeSymbol(string name)
            : this(name, ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty) { }


        private TypeSymbol(TypeSymbol elementType, bool pointer)
        {
            ElementType = elementType;
            IsPointer = pointer;
            _locked = true;
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
            if (SpecialType != SpecialType.None && SpecialType != SpecialType.Error)
                return SpecialType.ToString().ToLower();
            return Name;
        }

        public TypeSymbol MakePointer()
        {
            return new TypeSymbol(this, true);
        }

        public static bool operator ==(TypeSymbol a, TypeSymbol b)
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

        public static bool operator !=(TypeSymbol a, TypeSymbol b)
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