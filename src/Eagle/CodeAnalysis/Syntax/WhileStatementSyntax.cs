using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    internal class WhileStatementSyntax : StatementSyntax
    {
        public Token WhileKeyword { get; }
        public ExpressionSyntax Condition { get; }
        public BlockStatementSyntax Body { get; }

        public WhileStatementSyntax(Token whileKeyword, ExpressionSyntax condition, BlockStatementSyntax body)
        {
            WhileKeyword = whileKeyword;
            Condition = condition;
            Body = body;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return WhileKeyword;
            yield return Condition;
            yield return Body;
        }
    }
}