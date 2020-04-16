namespace Eagle.CodeAnalysis.Syntax
{
    public class PropertyBodySyntax : SyntaxNode
    {
        public Token OpenBraceToken { get; }
        public SyntaxNode? GetterClause { get; }
        public SyntaxNode? SetterClause { get; }
        public Token CloseBraceToken { get; }

        public PropertyBodySyntax(SyntaxTree syntaxTree, Token openBraceToken, SyntaxNode? getterClause, SyntaxNode? setterClause, Token closeBraceToken) : base(syntaxTree)
        {
            OpenBraceToken = openBraceToken;
            GetterClause = getterClause;
            SetterClause = setterClause;
            CloseBraceToken = closeBraceToken;
        }
    }
}