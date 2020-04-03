namespace Repl.CodeAnalysis.Syntax
{
    public class AliasDeclarationSyntax : SyntaxNode
    {
        public Token AliasKeyword { get; }
        public Token IdentifierToken { get; }
        public Token EqualsToken { get; }
        public SyntaxNode Type { get; }

        public AliasDeclarationSyntax(SyntaxTree syntaxTree, Token aliasKeyword, Token identifierToken, Token equalsToken, SyntaxNode type)
        :base(syntaxTree)
        {
            AliasKeyword = aliasKeyword;
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            Type = type;
        }
    }
}