namespace Repl.CodeAnalysis
{
    public class LabelSymbol : Symbol
    {
        public override string Name { get; }

        public LabelSymbol(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }
}