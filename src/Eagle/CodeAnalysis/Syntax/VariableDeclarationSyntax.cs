namespace Eagle.CodeAnalysis.Syntax
{
    public class VariableDeclarationSyntax : StatementSyntax
    {
        public Token Keyword { get; }
        public Token? AmpersandToken { get; }
        public Token IdentifierToken { get; }
        public TypeClauseSyntax? TypeClause { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax Initializer { get; }

        public VariableDeclarationSyntax(SyntaxTree syntaxTree, Token keyword, Token? ampersandToken, Token identifierToken, TypeClauseSyntax? typeClause, Token equalsToken, ExpressionSyntax initializer)
            : base(syntaxTree)
        {
            Keyword = keyword;
            AmpersandToken = ampersandToken;
            IdentifierToken = identifierToken;
            TypeClause = typeClause;
            EqualsToken = equalsToken;
            Initializer = initializer;
        }
    }
}