using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public abstract class StatementSyntax : SyntaxNode
    {
        public abstract override IEnumerable<SyntaxNode> GetChildren();
    }

    public class VariableDeclarationSyntax : StatementSyntax
    {
        public Token Keyword { get; }
        public Token Identifier { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax Initializer { get; }

        public VariableDeclarationSyntax(Token keyword, Token identifier, Token equalsToken, ExpressionSyntax initializer)
        {
            Keyword = keyword;
            Identifier = identifier;
            EqualsToken = equalsToken;
            Initializer = initializer;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Keyword;
            yield return Identifier;
            yield return EqualsToken;
            yield return Initializer;
        }
    }
}