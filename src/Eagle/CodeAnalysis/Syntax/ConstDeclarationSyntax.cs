namespace Eagle.CodeAnalysis.Syntax
{
    public class ConstDeclarationSyntax : MemberSyntax
    {
        public Token ConstKeyword { get; }
        public Token IdentifierToken { get; }
        public TypeClauseSyntax TypeClause { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax Initializer { get; }

        public ConstDeclarationSyntax(SyntaxTree syntaxTree, Token constKeyword, Token identifierToken,
            TypeClauseSyntax typeClause, Token equalsToken, ExpressionSyntax initializer)
            : base(syntaxTree)
        {
            ConstKeyword = constKeyword;
            IdentifierToken = identifierToken;
            TypeClause = typeClause;
            EqualsToken = equalsToken;
            Initializer = initializer;
        }
    }
}