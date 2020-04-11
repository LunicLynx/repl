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
        public StatementSyntax Body { get; }

        public ForStatementSyntax(SyntaxTree syntaxTree, Token forKeyword, Token identifierToken, Token equalsToken, ExpressionSyntax lowerBound, Token toKeyword, ExpressionSyntax upperBound, StatementSyntax body)
            : base(syntaxTree)
        {
            ForKeyword = forKeyword;
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            LowerBound = lowerBound;
            ToKeyword = toKeyword;
            UpperBound = upperBound;
            Body = body;
        }
    }
}