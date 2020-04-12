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

        public override IEnumerable<BoundNode> GetChildren()
        {
            foreach (var argument in Arguments)
            {
                yield return argument;
            }
        }

        public override TypeSymbol Type => Method.Type;
    }
}