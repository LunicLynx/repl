using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class UnaryExpressionSyntax : ExpressionSyntax
    {
        public Token OperatorToken { get; }
        public ExpressionSyntax Operand { get; }

        public UnaryExpressionSyntax(SyntaxTree syntaxTree, Token operatorToken, ExpressionSyntax operand)
            : base(syntaxTree)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }
    }
}