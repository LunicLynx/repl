﻿namespace Repl.CodeAnalysis
{
    public class AliasSymbol : Symbol
    {
        public override string Name { get; }
        public TypeSymbol Type { get; }

        public AliasSymbol(string name, TypeSymbol type)
        {
            Name = name;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Name} = {Type}";
        }
    }
}