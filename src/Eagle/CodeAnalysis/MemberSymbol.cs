namespace Eagle.CodeAnalysis
{
    public abstract class MemberSymbol : Symbol
    {
        public TypeSymbol Type { get; }

        protected MemberSymbol(string name, TypeSymbol type)
            : base(name)
        {
            Type = type;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}