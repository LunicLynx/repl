using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    internal class BoundInvokeExpression : BoundExpression
    {
        public FunctionSymbol Function { get; }

        public BoundInvokeExpression(FunctionSymbol function)
        {
            Function = function;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }

        public override Type Type => Function.ReturnType.ClrType;
    }
}