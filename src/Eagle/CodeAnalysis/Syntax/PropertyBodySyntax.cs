namespace Eagle.CodeAnalysis.Syntax
{
    public class PropertyBodySyntax : SyntaxNode
    {
        public Token OpenBraceToken { get; }
        public GetterClauseSyntax? GetterClause { get; }
        public SetterClauseSyntax? SetterClause { get; }
        public Token CloseBraceToken { get; }

        public PropertyBodySyntax(SyntaxTree syntaxTree, Token openBraceToken, GetterClauseSyntax? getterClause, SetterClauseSyntax? setterClause, Token closeBraceToken) : base(syntaxTree)
        {
            OpenBraceToken = openBraceToken;
            GetterClause = getterClause;
            SetterClause = setterClause;
            CloseBraceToken = closeBraceToken;
        }
    }
}