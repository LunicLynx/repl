namespace Eagle.CodeAnalysis.Syntax
{
    public class ExpressionBodySyntax : SyntaxNode
    {
        public Token EqualsGreaterToken { get; }
        public ExpressionSyntax Expression { get; }

        public ExpressionBodySyntax(SyntaxTree syntaxTree, Token equalsGreaterToken, ExpressionSyntax expression)
            : base(syntaxTree)
        {
            EqualsGreaterToken = equalsGreaterToken;
            Expression = expression;
        }
    }
}