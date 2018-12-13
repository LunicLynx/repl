using System;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryOperator Operator { get; }
        public BoundExpression Operand { get; }
        public override Type Type => Operator.ResultType;

        public BoundUnaryExpression(BoundUnaryOperator @operator, BoundExpression operand)
        {
            Operator = @operator;
            Operand = operand;
        }
    }
}