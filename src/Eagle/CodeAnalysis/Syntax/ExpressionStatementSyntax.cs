using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ExpressionStatementSyntax : StatementSyntax
    {
        public ExpressionSyntax Expression { get; }

        public ExpressionStatementSyntax(SyntaxTree syntaxTree, ExpressionSyntax expression)
            : base(syntaxTree)
        {
            Expression = expression;
        }
    }
}