using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class VariableDeclarationSyntax : StatementSyntax
    {
        public Token Keyword { get; }
        public Token IdentifierToken { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax Initializer { get; }

        public VariableDeclarationSyntax(Token keyword, Token identifierToken, Token equalsToken, ExpressionSyntax initializer)
        {
            Keyword = keyword;
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            Initializer = initializer;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Keyword;
            yield return IdentifierToken;
            yield return EqualsToken;
            yield return Initializer;
        }
    }
}