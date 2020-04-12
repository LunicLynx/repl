namespace Eagle.CodeAnalysis.Syntax
{
    public class NameExpressionSyntax : ExpressionSyntax
    {
        public Token IdentifierToken { get; }

        public NameExpressionSyntax(SyntaxTree syntaxTree, Token identifierToken)
            : base(syntaxTree)
        {
            IdentifierToken = identifierToken;
        }
    }
}