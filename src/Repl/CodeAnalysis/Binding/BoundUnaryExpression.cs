using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryOperator Operator { get; }
        public BoundExpression Operand { get; }
        public override TypeSymbol Type => Operator.ResultType;

        public BoundUnaryExpression(BoundUnaryOperator @operator, BoundExpression operand)
        {
            Operator = @operator;
            Operand = operand;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Operand;
        }
    }
}