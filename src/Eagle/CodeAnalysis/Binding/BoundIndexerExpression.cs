using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundIndexerExpression : BoundExpression
    {
        public override TypeSymbol Type => Indexer.Type;
        public BoundExpression Target { get; }
        public IndexerSymbol Indexer { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }

        public BoundIndexerExpression(BoundExpression target, IndexerSymbol indexer, ImmutableArray<BoundExpression> arguments)
        {
            Target = target;
            Indexer = indexer;
            Arguments = arguments;
        }
    }
}