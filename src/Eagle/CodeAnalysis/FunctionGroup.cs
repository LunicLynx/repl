namespace Repl.CodeAnalysis
{
    public class FunctionGroup : Symbol
    {
        public override SymbolKind Kind => SymbolKind.FunctionGroup;
        public override string Name { get; }
    }
}