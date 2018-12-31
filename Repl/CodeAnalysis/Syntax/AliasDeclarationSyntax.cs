namespace Repl.CodeAnalysis.Syntax
{
    public class AliasDeclarationSyntax : SyntaxNode
    {
        public Token AliasKeyword { get; }
        public Token IdentifierToken { get; }
        public Token EqualsToken { get; }
        public TypeSyntax Type { get; }

        public AliasDeclarationSyntax(Token aliasKeyword, Token identifierToken, Token equalsToken, TypeSyntax type)
        {
            AliasKeyword = aliasKeyword;
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            Type = type;
        }
    }
}