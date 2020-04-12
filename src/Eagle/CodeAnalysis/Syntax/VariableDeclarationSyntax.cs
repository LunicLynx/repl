namespace Eagle.CodeAnalysis.Syntax
{
    public class VariableDeclarationSyntax : StatementSyntax
    {
        public Token Keyword { get; }
        public Token IdentifierToken { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax Initializer { get; }

        public VariableDeclarationSyntax(SyntaxTree syntaxTree, Token keyword, Token identifierToken, Token equalsToken, ExpressionSyntax initializer)
            : base(syntaxTree)
        {
            Keyword = keyword;
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            Initializer = initializer;
        }
    }
}