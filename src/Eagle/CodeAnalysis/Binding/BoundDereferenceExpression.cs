namespace Eagle.CodeAnalysis.Binding
{
    public class BoundDereferenceExpression : BoundExpression
    {
        public BoundExpression Expression { get; }

        public BoundDereferenceExpression(BoundExpression expression)
        {
            Expression = expression;
        }

        public override TypeSymbol Type => Expression.Type.ElementType;
    }
}