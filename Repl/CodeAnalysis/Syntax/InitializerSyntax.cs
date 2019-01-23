using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class InitializerSyntax : SyntaxNode
    {
        public Token EqualsToken { get; }
        public ExpressionSyntax Expression { get; }

        public InitializerSyntax(Token equalsToken, ExpressionSyntax expression)
        {
            EqualsToken = equalsToken;
            Expression = expression;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return EqualsToken;
            yield return Expression;
        }
    }
}