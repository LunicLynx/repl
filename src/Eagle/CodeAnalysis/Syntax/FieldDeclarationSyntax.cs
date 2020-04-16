namespace Eagle.CodeAnalysis.Syntax
{
    public class FieldDeclarationSyntax : MemberDeclarationSyntax
    {
        public Token IdentifierToken { get; }
        public TypeClauseSyntax TypeClause { get; }
        public InitializerSyntax Initializer { get; }

        public FieldDeclarationSyntax(SyntaxTree syntaxTree, Token identifierToken, TypeClauseSyntax typeClause, InitializerSyntax initializer)
            : base(syntaxTree)
        {
            IdentifierToken = identifierToken;
            TypeClause = typeClause;
            Initializer = initializer;
        }
    }
}