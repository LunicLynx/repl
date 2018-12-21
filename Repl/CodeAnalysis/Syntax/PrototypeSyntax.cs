using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class PrototypeSyntax : SyntaxNode
    {
        public TypeSyntax ReturnType { get; }
        public Token IdentifierToken { get; }
        public Token OpenParenthesisToken { get; }
        public Token CloseParenthesisToken { get; }

        public PrototypeSyntax(TypeSyntax returnType, Token identifierToken, Token openParenthesisToken, Token closeParenthesisToken)
        {
            ReturnType = returnType;
            IdentifierToken = identifierToken;
            OpenParenthesisToken = openParenthesisToken;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return OpenParenthesisToken;
            yield return CloseParenthesisToken;
        }
    }
}