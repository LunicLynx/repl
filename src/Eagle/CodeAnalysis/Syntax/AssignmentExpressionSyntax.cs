namespace Eagle.CodeAnalysis.Syntax
{
    public class AssignmentExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Target { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax Expression { get; }

        public AssignmentExpressionSyntax(SyntaxTree syntaxTree, ExpressionSyntax target, Token equalsToken, ExpressionSyntax expression)
        :base(syntaxTree)
        {
            Target = target;
            EqualsToken = equalsToken;
            Expression = expression;
        }
    }
}