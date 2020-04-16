namespace Eagle.CodeAnalysis.Syntax
{
    public class ParameterSyntax : SyntaxNode
    {
        public Token IdentifierToken { get; }
        public TypeClauseSyntax Type { get; }

        public ParameterSyntax(SyntaxTree syntaxTree, Token identifierToken, TypeClauseSyntax type)
            : base(syntaxTree)
        {
            IdentifierToken = identifierToken;
            Type = type;
        }
    }
}