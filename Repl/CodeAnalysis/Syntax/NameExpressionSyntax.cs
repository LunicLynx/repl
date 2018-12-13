using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class NameExpressionSyntax : ExpressionSyntax
    {
        public Token IdentifierToken { get; }

        public NameExpressionSyntax(Token identifierToken)
        {
            IdentifierToken = identifierToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
        }
    }
}