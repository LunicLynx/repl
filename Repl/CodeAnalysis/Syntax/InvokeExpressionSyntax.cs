using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    internal class InvokeExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Target { get; }
        public Token OpenParenthesisToken { get; }
        public Token CloseParenthesisToken { get; }

        public InvokeExpressionSyntax(ExpressionSyntax target, Token openParenthesisToken, Token closeParenthesisToken)
        {
            Target = target;
            OpenParenthesisToken = openParenthesisToken;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Target;
            yield return OpenParenthesisToken;
            yield return CloseParenthesisToken;
        }
    }
}