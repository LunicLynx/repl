using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Repl.CodeAnalysis
{
    [DebuggerDisplay("{Name}")]
    public class TypeSymbol : Symbol
    {
        public TypeSymbol ElementType { get; }
        public bool IsPointer { get; }
        private bool _locked;
        private ImmutableArray<MemberSymbol> _members;
        private ImmutableArray<TypeSymbol> _baseType;

        private string _name;
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

        public static readonly TypeSymbol Error = new TypeSymbol("?");
        public static TypeSymbol String { get; set; }
        public static TypeSymbol Void { get; set; }
        public static TypeSymbol I8 { get; set; }
        public static TypeSymbol I16 { get; set; }
        public static TypeSymbol I32 { get; set; }
        public static TypeSymbol I64 { get; set; }
        public static TypeSymbol U8 { get; set; }
        public static TypeSymbol U16 { get; set; }
        public static TypeSymbol U32 { get; set; }
        public static TypeSymbol U64 { get; set; }
        public static TypeSymbol Bool { get; set; }
        public static TypeSymbol Uint { get; set; }
        public static TypeSymbol Int { get; set; }

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