using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class AssignmentExpressionSyntax : ExpressionSyntax
    {
        public Token IdentifierToken { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax Expression { get; }

        public AssignmentExpressionSyntax(Token identifierToken, Token equalsToken, ExpressionSyntax expression)
        {
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            Expression = expression;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return EqualsToken;
            yield return Expression;
        }
    }
}