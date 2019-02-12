using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Binding
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

        public override IEnumerable<BoundNode> GetChildren()
        {
            foreach (var argument in Arguments)
            {
                yield return argument;
            }
        }
    }
}