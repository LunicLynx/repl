using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ParenthesizedExpressionSyntax : ExpressionSyntax
    {
        public Token OpenParenthesisToken { get; }
        public ExpressionSyntax Expression { get; }
        public Token CloseParenthesisToken { get; }

        public ParenthesizedExpressionSyntax(Token openParenthesisToken, ExpressionSyntax expression, Token closeParenthesisToken)
        {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParenthesisToken;
            yield return Expression;
            yield return CloseParenthesisToken;
        }
    }
}