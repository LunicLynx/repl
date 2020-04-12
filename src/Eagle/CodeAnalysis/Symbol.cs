namespace Eagle.CodeAnalysis
{
    public abstract class Symbol
    {
        public abstract SymbolKind Kind { get; }
        // Type ?
        public string Name { get; }

        protected Symbol(string name)
        {
            Name = name;
        }
    }
}