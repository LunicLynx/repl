using System;

namespace Repl.CodeAnalysis
{
    public abstract class MemberSymbol : Symbol
    {
        private bool _locked;
        private TypeSymbol _type;

        public override string Name { get; }

        public TypeSymbol Type
        {
            get => _type;
            set
            {
                ThrowIfLocked();
                _type = value;
            }
        }

        protected MemberSymbol(string name, TypeSymbol type)
        {
            Name = name;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public void Lock()
        {
            ThrowIfLocked();
            _locked = true;
        }

        private void ThrowIfLocked()
        {
            if (_locked) throw new Exception("Locked");
        }
    }
}