using System.Collections.Immutable;

namespace Eagle.CodeAnalysis
{
    public class IndexerSymbol : MemberSymbol
    {
        public TypeSymbol DeclaringType { get; }
        public ImmutableArray<ParameterSymbol> Parameters { get; }
        public MethodSymbol? Getter { get; }
        public MethodSymbol? Setter { get; }

        public override SymbolKind Kind => SymbolKind.Indexer;

        public IndexerSymbol(TypeSymbol declaringType, TypeSymbol type, ImmutableArray<ParameterSymbol> parameters, MethodSymbol? getter, MethodSymbol? setter) : base("Item", type)
        {
            DeclaringType = declaringType;
            Parameters = parameters;
            Getter = getter;
            Setter = setter;
        }
    }
}