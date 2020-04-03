using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class CastExpressionSyntax : ExpressionSyntax
    {
        public Token OpenParenthesisToken { get; }
        public SyntaxNode Type { get; }
        public Token CloseParenthesisToken { get; }
        public ExpressionSyntax Expression { get; }

        public CastExpressionSyntax(SyntaxTree syntaxTree, Token openParenthesisToken, SyntaxNode type, Token closeParenthesisToken, ExpressionSyntax expression)
            : base(syntaxTree)
        {
            OpenParenthesisToken = openParenthesisToken;
            Type = type;
            CloseParenthesisToken = closeParenthesisToken;
            Expression = expression;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParenthesisToken;
            yield return Type;
            yield return CloseParenthesisToken;
            yield return Expression;
        }
    }
}