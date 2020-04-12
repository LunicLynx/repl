namespace Eagle.CodeAnalysis.Syntax
{
    public class ThisExpressionSyntax : ExpressionSyntax
    {
        public Token ThisKeyword { get; }

        public ThisExpressionSyntax(SyntaxTree syntaxTree, Token thisKeyword)
            : base(syntaxTree)
        {
            ThisKeyword = thisKeyword;
        }
    }
}