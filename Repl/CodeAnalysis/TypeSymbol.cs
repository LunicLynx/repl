using System;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis
{
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

        public TypeSymbol(string name, ImmutableArray<TypeSymbol> baseType, ImmutableArray<MemberSymbol> members)
        {
            Name = name;
            BaseType = baseType;
            Members = members;
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
    }
}