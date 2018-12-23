using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Binding
{
    internal class BoundCallExpression : BoundExpression
    {
        public FunctionSymbol Function { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }

        public BoundCallExpression(FunctionSymbol function, ImmutableArray<BoundExpression> arguments)
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

        public override Type Type => Function.ReturnType.ClrType;
    }
}