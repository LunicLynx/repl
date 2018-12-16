using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundBinaryExpression : BoundExpression
    {
        public BoundExpression Left { get; }
        public BoundBinaryOperator Operator { get; }
        public BoundExpression Right { get; }
        public override Type Type => Operator.ResultType;

        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator @operator, BoundExpression right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Left;
            yield return Right;
        }
    }
}