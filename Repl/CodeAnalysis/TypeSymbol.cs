namespace Repl.CodeAnalysis
{
    public class TypeSymbol : Symbol
    {
        public override string Name { get; }

        public TypeSymbol(string name)
        {
            Name = name;
        }
    }
}