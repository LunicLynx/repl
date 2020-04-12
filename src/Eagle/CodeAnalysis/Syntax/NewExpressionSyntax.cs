namespace Eagle.CodeAnalysis.Syntax
{
    public class NewExpressionSyntax : ExpressionSyntax
    {
        public Token NewKeyword { get; }
        public NameExpressionSyntax TypeName { get; }

        public NewExpressionSyntax(SyntaxTree syntaxTree, Token newKeyword, NameExpressionSyntax typeName)
            : base(syntaxTree)
        {
            NewKeyword = newKeyword;
            TypeName = typeName;
        }
    }
}