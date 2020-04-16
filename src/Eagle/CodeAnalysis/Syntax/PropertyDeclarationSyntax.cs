namespace Eagle.CodeAnalysis.Syntax
{
    public class PropertyDeclarationSyntax : MemberDeclarationSyntax
    {
        public Token IdentifierToken { get; }
        public TypeClauseSyntax TypeClause { get; }
        public SyntaxNode Body { get; }

        public PropertyDeclarationSyntax(SyntaxTree syntaxTree, Token identifierToken, TypeClauseSyntax typeClause, SyntaxNode body) : base(syntaxTree)
        {
            IdentifierToken = identifierToken;
            TypeClause = typeClause;
            Body = body;
        }
    }
}