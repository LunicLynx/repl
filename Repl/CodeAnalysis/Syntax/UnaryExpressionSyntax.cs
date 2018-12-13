using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class UnaryExpressionSyntax : ExpressionSyntax
    {
        public Token OperatorToken { get; }
        public ExpressionSyntax Operand { get; }

        public UnaryExpressionSyntax(Token operatorToken, ExpressionSyntax operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }


        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OperatorToken;
            yield return Operand;
        }
    }
}