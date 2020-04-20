namespace Eagle.CodeAnalysis
{
    public class IndexerSymbol : MemberSymbol
    {
        public TypeSymbol DeclaringType { get; }
        public MethodSymbol Getter { get; }
        public MethodSymbol Setter { get; }

        public override SymbolKind Kind => SymbolKind.Indexer;

        public IndexerSymbol(TypeSymbol declaringType, TypeSymbol type, MethodSymbol getter, MethodSymbol setter) : base("Item", type)
        {
            DeclaringType = declaringType;
            Getter = getter;
            Setter = setter;
        }
    }
}