using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundAssignmentExpression : BoundExpression
    {
        public BoundExpression Target { get; }
        public BoundExpression Expression { get; }
        public override TypeSymbol Type => Expression.Type;

        public BoundAssignmentExpression(BoundExpression target, BoundExpression expression)
        {
            Target = target;
            Expression = expression;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Target;
            yield return Expression;
        }
    }
}