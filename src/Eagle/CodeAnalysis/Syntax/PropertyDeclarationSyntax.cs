namespace Eagle.CodeAnalysis.Syntax
{
    public class PropertyDeclarationSyntax : MemberDeclarationSyntax
    {
        public Token IdentifierToken { get; }
        public TypeClauseSyntax TypeClause { get; }
        public ExpressionBodySyntax ExpressionBody { get; }

        public PropertyDeclarationSyntax(SyntaxTree syntaxTree, Token identifierToken, TypeClauseSyntax typeClause, ExpressionBodySyntax expressionBody) : base(syntaxTree)
        {
            IdentifierToken = identifierToken;
            TypeClause = typeClause;
            ExpressionBody = expressionBody;
        }
    }
}