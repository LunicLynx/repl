namespace Eagle.CodeAnalysis.Syntax
{
    public class FunctionDeclarationSyntax : MemberSyntax, IInvokableDeclarationSyntax
    {
        public Token IdentifierToken { get; }
        public Token OpenParenthesisToken { get; }
        public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        public Token CloseParenthesisToken { get; }
        public TypeClauseSyntax Type { get; }
        public BlockStatementSyntax Body { get; }

        public FunctionDeclarationSyntax(SyntaxTree syntaxTree, Token identifierToken, Token openParenthesisToken, SeparatedSyntaxList<ParameterSyntax> parameters, Token closeParenthesisToken, TypeClauseSyntax? type, BlockStatementSyntax body) 
            : base(syntaxTree)
        {
            IdentifierToken = identifierToken;
            OpenParenthesisToken = openParenthesisToken;
            Parameters = parameters;
            CloseParenthesisToken = closeParenthesisToken;
            Type = type;
            Body = body;
        }
    }
}