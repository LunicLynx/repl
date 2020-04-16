using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundArrayIndexExpression : BoundExpression
    {
        public BoundExpression Target { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }

        public BoundArrayIndexExpression(BoundExpression target, ImmutableArray<BoundExpression> arguments)
        {
            Target = target;
            Arguments = arguments;
        }

        public override TypeSymbol Type => Target.Type.ElementType;
    }
}