namespace Repl.CodeAnalysis
{
    public class ParameterSymbol : Symbol
    {
        public TypeSymbol Type { get; }
        public override string Name { get; }
        public int Index { get; }

        public ParameterSymbol(TypeSymbol type, string name, int index)
        {
            Type = type;
            Name = name;
            Index = index;
        }

        public override string ToString()
        {
            return $"{Type} {Name}";
        }
    }
}