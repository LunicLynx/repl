namespace Eagle.CodeAnalysis.Syntax
{
    internal class LoopStatementSyntax : StatementSyntax
    {
        public Token LoopKeyword { get; }
        public StatementSyntax Body { get; }

        public LoopStatementSyntax(SyntaxTree syntaxTree, Token loopKeyword, StatementSyntax body)
            : base(syntaxTree)
        {
            LoopKeyword = loopKeyword;
            Body = body;
        }
    }
}