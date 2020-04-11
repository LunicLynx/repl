using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class BinaryExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Left { get; }
        public Token OperatorToken { get; }
        public ExpressionSyntax Right { get; }

        public BinaryExpressionSyntax(SyntaxTree syntaxTree, ExpressionSyntax left, Token operatorToken, ExpressionSyntax right)
            : base(syntaxTree)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }
    }
}