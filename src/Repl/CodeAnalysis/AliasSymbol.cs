using System;

namespace Repl.CodeAnalysis
{
    public class AliasSymbol : Symbol
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

        private void ThrowIfLocked()
        {
            if (_locked)
                throw new Exception("Locked");
        }

        public AliasSymbol(string name, TypeSymbol type)
        {
            Name = name;
            Type = type;
        }

        public void Lock()
        {
            ThrowIfLocked();
            _locked = true;
        }

        public override string ToString()
        {
            return $"{Name} = {Type}";
        }
    }
}