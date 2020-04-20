namespace Eagle.CodeAnalysis.Syntax
{
    internal class IndexerDeclarationSyntax : MemberDeclarationSyntax
    {
        public Token OpenBracketToken { get; }
        public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        public Token CloseBracketToken { get; }
        public TypeClauseSyntax TypeClause { get; }
        public SyntaxNode Body { get; }

        public IndexerDeclarationSyntax(SyntaxTree syntaxTree, Token openBracketToken, SeparatedSyntaxList<ParameterSyntax> parameters, Token closeBracketToken, TypeClauseSyntax typeClause, SyntaxNode body) : base(syntaxTree)
        {
            OpenBracketToken = openBracketToken;
            Parameters = parameters;
            CloseBracketToken = closeBracketToken;
            TypeClause = typeClause;
            Body = body;
        }
    }
}