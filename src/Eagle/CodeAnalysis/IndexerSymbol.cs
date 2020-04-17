namespace Eagle.CodeAnalysis
{
    public class IndexerSymbol : MemberSymbol
    {
        public MethodSymbol Getter { get; }
        public MethodSymbol Setter { get; }

        public override SymbolKind Kind => SymbolKind.Indexer;

        public IndexerSymbol(TypeSymbol type, MethodSymbol getter, MethodSymbol setter) : base("Item", type)
        {
            Getter = getter;
            Setter = setter;
        }
    }
}