namespace Repl.CodeAnalysis
{
    public abstract class Symbol
    {
        public abstract SymbolKind Kind { get; }
        // Type ?
        public abstract string Name { get; }
    }
}