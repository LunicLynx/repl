namespace Repl.CodeAnalysis
{
    public abstract class MemberSymbol : Symbol
    {
        public override string Name { get; }
        public TypeSymbol Type { get; }

        protected MemberSymbol(string name, TypeSymbol type)
        {
            Name = name;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}