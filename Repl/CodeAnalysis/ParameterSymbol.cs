namespace Repl.CodeAnalysis
{
    public class ParameterSymbol : Symbol
    {
        public TypeSymbol Type { get; }
        public override string Name { get; }

        public ParameterSymbol(TypeSymbol type, string name)
        {
            Type = type;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Type} {Name}";
        }
    }
}