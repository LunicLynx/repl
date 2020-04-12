namespace Eagle.CodeAnalysis.Syntax
{
    public class IfStatementSyntax : StatementSyntax
    {
        public Token IfKeyword { get; }
        public ExpressionSyntax Condition { get; }
        public StatementSyntax ThenStatement { get; }
        public ElseClauseSyntax ElseClause { get; }

        public IfStatementSyntax(SyntaxTree syntaxTree, Token ifKeyword, ExpressionSyntax condition, StatementSyntax thenStatement, ElseClauseSyntax elseClause)
            : base(syntaxTree)
        {
            IfKeyword = ifKeyword;
            Condition = condition;
            ThenStatement = thenStatement;
            ElseClause = elseClause;
        }
    }
}