using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    internal class WhileStatementSyntax : StatementSyntax
    {
        public Token WhileKeyword { get; }
        public ExpressionSyntax Condition { get; }
        public BlockStatementSyntax Block { get; }

        public WhileStatementSyntax(Token whileKeyword, ExpressionSyntax condition, BlockStatementSyntax block)
        {
            WhileKeyword = whileKeyword;
            Condition = condition;
            Block = block;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return WhileKeyword;
            yield return Condition;
            yield return Block;
        }
    }
}