namespace Eagle.CodeAnalysis.Syntax
{
    public class LiteralExpressionSyntax : ExpressionSyntax
    {
        public Token LiteralToken { get; }
        public object Value { get; }

        public LiteralExpressionSyntax(SyntaxTree syntaxTree, Token literalToken)
            : this(syntaxTree, literalToken, literalToken.Value)
        {
        }

        public LiteralExpressionSyntax(SyntaxTree syntaxTree, Token literalToken, object value)
            : base(syntaxTree)
        {
            LiteralToken = literalToken;
            Value = value;
        }
    }
}