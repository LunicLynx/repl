using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    internal class WhileStatementSyntax : StatementSyntax
    {
        public Token WhileKeyword { get; }
        public ExpressionSyntax Condition { get; }
        public BlockStatementSyntax Body { get; }

        public WhileStatementSyntax(SyntaxTree syntaxTree, Token whileKeyword, ExpressionSyntax condition, BlockStatementSyntax body)
            : base(syntaxTree)
        {
            WhileKeyword = whileKeyword;
            Condition = condition;
            Body = body;
        }
    }
}