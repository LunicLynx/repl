namespace Eagle.CodeAnalysis.Syntax
{
    public class SuffixCastExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Target { get; }
        public Token DotToken { get; }
        public Token OpenParenthesisToken { get; }
        public SyntaxNode Type { get; }
        public Token CloseParenthesisToken { get; }

        public SuffixCastExpressionSyntax(SyntaxTree syntaxTree, ExpressionSyntax target, Token dotToken, Token openParenthesisToken, SyntaxNode type, Token closeParenthesisToken)
            : base(syntaxTree)
        {
            Target = target;
            DotToken = dotToken;
            OpenParenthesisToken = openParenthesisToken;
            Type = type;
            CloseParenthesisToken = closeParenthesisToken;
        }
    }
}