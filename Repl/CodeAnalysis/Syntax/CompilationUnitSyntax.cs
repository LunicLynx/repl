using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class CompilationUnitSyntax : SyntaxNode
    {
        public ExpressionSyntax Expression { get; }
        public Token EndOfFileToken { get; }

        public CompilationUnitSyntax(ExpressionSyntax expression, Token endOfFileToken)
        {
            Expression = expression;
            EndOfFileToken = endOfFileToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Expression;
            yield return EndOfFileToken;
        }
    }
}