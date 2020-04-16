using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundReturnStatement : BoundStatement
    {
        public BoundExpression? Expression { get; }

        public BoundReturnStatement(BoundExpression? expression)
        {
            Expression = expression;
        }
    }
}