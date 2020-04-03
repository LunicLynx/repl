using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class IfStatementSyntax : StatementSyntax
    {
        public Token IfKeyword { get; }
        public ExpressionSyntax Condition { get; }
        public BlockStatementSyntax ThenBlock { get; }
        public ElseClauseSyntax ElseClause { get; }

        public IfStatementSyntax(SyntaxTree syntaxTree, Token ifKeyword, ExpressionSyntax condition, BlockStatementSyntax thenBlock, ElseClauseSyntax elseClause)
            : base(syntaxTree)
        {
            IfKeyword = ifKeyword;
            Condition = condition;
            ThenBlock = thenBlock;
            ElseClause = elseClause;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IfKeyword;
            yield return Condition;
            yield return ThenBlock;

            if (ElseClause != null)
            {
                yield return ElseClause;
            }
        }
    }
}