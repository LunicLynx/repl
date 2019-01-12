using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Binding
{
    internal class BoundFunctionCallExpression : BoundExpression
    {
        public FunctionSymbol Function { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }

        public BoundFunctionCallExpression(FunctionSymbol function, ImmutableArray<BoundExpression> arguments)
        {
            Function = function;
            Arguments = arguments;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            foreach (var argument in Arguments)
            {
                yield return argument;
            }
        }

        public override TypeSymbol Type => Function.ReturnType;
    }
}