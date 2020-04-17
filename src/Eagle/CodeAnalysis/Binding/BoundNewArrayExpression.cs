using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundNewArrayExpression : BoundExpression
    {
        public override TypeSymbol Type { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }

        public BoundNewArrayExpression(TypeSymbol type, ImmutableArray<BoundExpression> arguments)
        {
            Type = type;
            Arguments = arguments;
        }
    }
}