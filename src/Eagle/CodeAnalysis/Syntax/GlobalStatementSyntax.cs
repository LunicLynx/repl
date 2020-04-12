namespace Eagle.CodeAnalysis.Syntax
{
    public class GlobalStatementSyntax : MemberSyntax
    {
        public StatementSyntax Statement { get; }

        public GlobalStatementSyntax(SyntaxTree syntaxTree, StatementSyntax statement) : base(syntaxTree)
        {
            Statement = statement;
        }
    }
}