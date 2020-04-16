using System.Collections.Generic;
using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundMethodCallExpression : BoundExpression
    {
        public BoundExpression Target { get; }
        public MethodSymbol Method { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }

        public BoundMethodCallExpression(BoundExpression target, MethodSymbol method, ImmutableArray<BoundExpression> arguments)
        {
            Target = target;
            Method = method;
            Arguments = arguments;
        }

        public override TypeSymbol Type => Method.Type;
    }

    public class BoundArrayIndexExpression : BoundExpression
    {
        public BoundExpression Target { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }

        public BoundArrayIndexExpression(BoundExpression target, ImmutableArray<BoundExpression> arguments)
        {
            Target = target;
            Arguments = arguments;
        }

        public override TypeSymbol Type => Target.Type;
    }
}