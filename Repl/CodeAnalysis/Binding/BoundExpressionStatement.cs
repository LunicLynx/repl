namespace Repl.CodeAnalysis.Binding
{
    public class BoundExpressionStatement : BoundStatement
    {
        public BoundExpression Expression { get; }

        public BoundExpressionStatement(BoundExpression expression)
        {
            Expression = expression;
        }
    }
}