namespace Eagle.CodeAnalysis.Syntax
{
    public class MemberAccessExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Target { get; }
        public Token DotToken { get; }
        public Token IdentifierToken { get; }

        public MemberAccessExpressionSyntax(SyntaxTree syntaxTree, ExpressionSyntax target, Token dotToken, Token identifierToken)
            : base(syntaxTree)
        {
            Target = target;
            DotToken = dotToken;
            IdentifierToken = identifierToken;
        }
    }
}