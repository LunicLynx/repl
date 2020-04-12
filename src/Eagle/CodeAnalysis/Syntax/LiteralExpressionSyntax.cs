namespace Eagle.CodeAnalysis.Syntax
{
    public class LiteralExpressionSyntax : ExpressionSyntax
    {
        public Token LiteralToken { get; }

        public LiteralExpressionSyntax(SyntaxTree syntaxTree, Token literalToken)
            : base(syntaxTree)
        {
            LiteralToken = literalToken;
        }
    }
}