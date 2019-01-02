using System;

namespace Repl.CodeAnalysis
{
    public class MemberSymbol : Symbol
    {
        public override string Name { get; }
        public TypeSymbol Type { get; }

        public MemberSymbol(string name, TypeSymbol type)
        {
            Name = name;
            Type = type;
        }
    }
}