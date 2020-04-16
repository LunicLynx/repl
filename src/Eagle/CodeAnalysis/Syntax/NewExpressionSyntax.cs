namespace Eagle.CodeAnalysis.Syntax
{
    public class NewInstanceExpressionSyntax : ExpressionSyntax, IInvocationExpressionSyntax
    {
        public Token NewKeyword { get; }
        public SyntaxNode Type { get; }
        public Token OpenParenthesisToken { get; }
        public SeparatedSyntaxList<ExpressionSyntax> Arguments { get; }
        public Token CloseParenthesisToken { get; }

        Token IInvocationExpressionSyntax.CloseToken => CloseParenthesisToken;

        public NewInstanceExpressionSyntax(SyntaxTree syntaxTree, Token newKeyword, SyntaxNode type, Token openParenthesisToken, SeparatedSyntaxList<ExpressionSyntax> arguments, Token closeParenthesisToken) : base(syntaxTree)
        {
            NewKeyword = newKeyword;
            Type = type;
            OpenParenthesisToken = openParenthesisToken;
            Arguments = arguments;
            CloseParenthesisToken = closeParenthesisToken;
        }
    }
}