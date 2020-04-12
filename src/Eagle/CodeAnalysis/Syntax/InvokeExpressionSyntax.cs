namespace Eagle.CodeAnalysis.Syntax
{
    internal class InvokeExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Target { get; }
        public Token OpenParenthesisToken { get; }
        public SeparatedSyntaxList<ExpressionSyntax> Arguments { get; }
        public Token CloseParenthesisToken { get; }

        public InvokeExpressionSyntax(SyntaxTree syntaxTree, ExpressionSyntax target, Token openParenthesisToken, SeparatedSyntaxList<ExpressionSyntax> arguments, Token closeParenthesisToken)
            : base(syntaxTree)
        {
            Target = target;
            OpenParenthesisToken = openParenthesisToken;
            Arguments = arguments;
            CloseParenthesisToken = closeParenthesisToken;
        }
    }
}