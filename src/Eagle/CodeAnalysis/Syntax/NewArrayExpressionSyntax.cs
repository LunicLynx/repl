namespace Eagle.CodeAnalysis.Syntax
{
    public class NewArrayExpressionSyntax : ExpressionSyntax
    {
        public Token NewKeyword { get; }
        public SyntaxNode Type { get; }
        public Token OpenBracketToken { get; }
        public SeparatedSyntaxList<ExpressionSyntax> Arguments { get; }
        public Token CloseBracketToken { get; }

        public NewArrayExpressionSyntax(SyntaxTree syntaxTree, Token newKeyword, SyntaxNode type, Token openBracketToken, SeparatedSyntaxList<ExpressionSyntax> arguments, Token closeBracketToken) : base(syntaxTree)
        {
            NewKeyword = newKeyword;
            Type = type;
            OpenBracketToken = openBracketToken;
            Arguments = arguments;
            CloseBracketToken = closeBracketToken;
        }
    }
}