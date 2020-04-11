using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ParenthesizedExpressionSyntax : ExpressionSyntax
    {
        public Token OpenParenthesisToken { get; }
        public ExpressionSyntax Expression { get; }
        public Token CloseParenthesisToken { get; }

        public ParenthesizedExpressionSyntax(SyntaxTree syntaxTree, Token openParenthesisToken, ExpressionSyntax expression, Token closeParenthesisToken)
            : base(syntaxTree)
        {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }
    }
}