using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    internal class ForStatementSyntax : StatementSyntax
    {
        public Token ForKeyword { get; }
        public Token IdentifierToken { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax LowerBound { get; }
        public Token ToKeyword { get; }
        public ExpressionSyntax UpperBound { get; }
        public BlockStatementSyntax Body { get; }

        public ForStatementSyntax(Token forKeyword, Token identifierToken, Token equalsToken, ExpressionSyntax lowerBound, Token toKeyword, ExpressionSyntax upperBound, BlockStatementSyntax body)
        {
            ForKeyword = forKeyword;
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            LowerBound = lowerBound;
            ToKeyword = toKeyword;
            UpperBound = upperBound;
            Body = body;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ForKeyword;
            yield return IdentifierToken;
            yield return EqualsToken;
            yield return LowerBound;
            yield return ToKeyword;
            yield return UpperBound;
            yield return Body;
        }
    }
}