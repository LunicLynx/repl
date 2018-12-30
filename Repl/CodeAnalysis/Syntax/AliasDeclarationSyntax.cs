namespace Repl.CodeAnalysis.Syntax
{
    public class AliasDeclarationSyntax : SyntaxNode
    {
        public Token AliasKeyword { get; }
        public Token IdentifierToken { get; }
        public TypeSyntax Type { get; }

        public AliasDeclarationSyntax(Token aliasKeyword, Token identifierToken, TypeSyntax type)
        {
            AliasKeyword = aliasKeyword;
            IdentifierToken = identifierToken;
            Type = type;
        }
    }
}