using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ExpressionStatementSyntax : StatementSyntax
    {
        public ExpressionSyntax Expression { get; }

        public ExpressionStatementSyntax(ExpressionSyntax expression)
        {
            Expression = expression;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Expression;
        }
    }
}