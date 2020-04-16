namespace Eagle.CodeAnalysis.Syntax
{
    public class ExternDeclarationSyntax : MemberSyntax
    {
        public Token ExternKeyword { get; }
        public Token IdentifierToken { get; }
        public Token OpenParenthesisToken { get; }
        public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        public Token CloseParenthesisToken { get; }
        public TypeClauseSyntax? Type { get; }

        public ExternDeclarationSyntax(SyntaxTree syntaxTree, Token externKeyword, Token identifierToken, Token openParenthesisToken, SeparatedSyntaxList<ParameterSyntax> parameters, Token closeParenthesisToken, TypeClauseSyntax? type)
            : base(syntaxTree)
        {
            ExternKeyword = externKeyword;
            IdentifierToken = identifierToken;
            OpenParenthesisToken = openParenthesisToken;
            Parameters = parameters;
            CloseParenthesisToken = closeParenthesisToken;
            Type = type;
        }
    }
}