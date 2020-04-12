namespace Eagle.CodeAnalysis.Syntax
{
    internal class BreakStatementSyntax : StatementSyntax
    {
        public Token BreakKeyword { get; }

        public BreakStatementSyntax(SyntaxTree syntaxTree, Token breakKeyword)
            : base(syntaxTree)
        {
            BreakKeyword = breakKeyword;
        }
    }
}