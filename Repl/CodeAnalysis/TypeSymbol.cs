using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Repl.CodeAnalysis
{
    [DebuggerDisplay("{Name}")]
    public class TypeSymbol : Symbol
    {
        private bool _locked;
        private ImmutableArray<MemberSymbol> _members;
        private ImmutableArray<TypeSymbol> _baseType;
        public override string Name { get; }

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
            Name = name;
            BaseType = baseType;
            Members = members;
        }

        public TypeSymbol(string name)
            : this(name, ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty) { }

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
    }
}