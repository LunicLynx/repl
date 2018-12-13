using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class AssignmentExpressionSyntax : ExpressionSyntax
    {
        public Token IdentifierToken { get; }
        public Token OperatorToken { get; }
        public ExpressionSyntax Expression { get; }

        public AssignmentExpressionSyntax(Token identifierToken, Token operatorToken, ExpressionSyntax expression)
        {
            IdentifierToken = identifierToken;
            OperatorToken = operatorToken;
            Expression = expression;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return OperatorToken;
            yield return Expression;
        }
    }
}