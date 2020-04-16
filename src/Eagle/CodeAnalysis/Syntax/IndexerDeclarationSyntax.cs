namespace Eagle.CodeAnalysis.Syntax
{
    internal class IndexerDeclarationSyntax : MemberDeclarationSyntax
    {
        public Token OpenBracketToken { get; }
        public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        public Token CloseBracketToken { get; }
        public TypeClauseSyntax TypeClause { get; }
        public object O { get; }

        public IndexerDeclarationSyntax(SyntaxTree syntaxTree, Token openBracketToken, SeparatedSyntaxList<ParameterSyntax> parameters, Token closeBracketToken, TypeClauseSyntax typeClause, object o) : base(syntaxTree)
        {
            OpenBracketToken = openBracketToken;
            Parameters = parameters;
            CloseBracketToken = closeBracketToken;
            TypeClause = typeClause;
            O = o;
        }
    }
}