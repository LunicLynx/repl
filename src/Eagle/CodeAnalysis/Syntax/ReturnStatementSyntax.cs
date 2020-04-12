namespace Eagle.CodeAnalysis.Syntax
{
    public class ReturnStatementSyntax : StatementSyntax
    {
        public Token ReturnKeyword { get; }
        public ExpressionSyntax? Expression { get; }

        public ReturnStatementSyntax(SyntaxTree syntaxTree, Token returnKeyword, ExpressionSyntax? expression) 
            : base(syntaxTree)
        {
            ReturnKeyword = returnKeyword;
            Expression = expression;
        }
    }
}