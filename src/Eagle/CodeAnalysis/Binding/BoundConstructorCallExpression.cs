using System.Collections.Generic;
using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundConstructorCallExpression : BoundExpression
    {
        public override TypeSymbol Type => Constructor.Type;
        public ConstructorSymbol Constructor { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }

        public BoundConstructorCallExpression(ConstructorSymbol constructor, ImmutableArray<BoundExpression> arguments)
        {
            Constructor = constructor;
            Arguments = arguments;
        }
    }

    public class BoundIndexerExpression : BoundExpression
    {
        public override TypeSymbol Type => Indexer.Type;
        public IndexerSymbol Indexer { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }

        public BoundIndexerExpression(BoundExpression target, IndexerSymbol indexer, ImmutableArray<BoundExpression> arguments)
        {
            Indexer = indexer;
            Arguments = arguments;
        }
    }
}