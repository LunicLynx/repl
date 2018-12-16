using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundAssignmentExpression : BoundExpression
    {
        public VariableSymbol Variable { get; }
        public BoundExpression Expression { get; }
        public override Type Type => Expression.Type;

        public BoundAssignmentExpression(VariableSymbol variable, BoundExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Expression;
        }
    }
}