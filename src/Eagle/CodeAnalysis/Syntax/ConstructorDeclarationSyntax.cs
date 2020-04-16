namespace Eagle.CodeAnalysis.Syntax
{
    internal class ConstructorDeclarationSyntax : MemberDeclarationSyntax
    {
        public Token IdentifierToken { get; }
        public Token OpenParenthesisToken { get; }
        public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        public Token CloseParenthesisToken { get; }

        public BlockStatementSyntax Body { get; }

        public ConstructorDeclarationSyntax(SyntaxTree syntaxTree, Token identifierToken, Token openParenthesisToken, SeparatedSyntaxList<ParameterSyntax> parameters, Token closeParenthesisToken, BlockStatementSyntax body)
            : base(syntaxTree)
        {
            IdentifierToken = identifierToken;
            OpenParenthesisToken = openParenthesisToken;
            Parameters = parameters;
            CloseParenthesisToken = closeParenthesisToken;
            Body = body;
        }
    }
}