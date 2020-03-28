using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Syntax
{
    internal class InvokeExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Target { get; }
        public Token OpenParenthesisToken { get; }
        public ImmutableArray<SyntaxNode> Arguments { get; }
        public Token CloseParenthesisToken { get; }

        public InvokeExpressionSyntax(ExpressionSyntax target, Token openParenthesisToken, ImmutableArray<SyntaxNode> arguments, Token closeParenthesisToken)
        {
            Target = target;
            OpenParenthesisToken = openParenthesisToken;
            Arguments = arguments;
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