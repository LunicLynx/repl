namespace Eagle.CodeAnalysis.Syntax
{
    public class ElseClauseSyntax : SyntaxNode
    {
        public Token ElseKeyword { get; }
        public StatementSyntax ElseStatement { get; }

        public ElseClauseSyntax(SyntaxTree syntaxTree, Token elseKeyword, StatementSyntax elseStatement)
            : base(syntaxTree)
        {
            ElseKeyword = elseKeyword;
            ElseStatement = elseStatement;
        }
    }
}