using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Syntax
{
    public class PrototypeSyntax : SyntaxNode
    {
        public TypeSyntax ReturnType { get; }
        public Token IdentifierToken { get; }
        public Token OpenParenthesisToken { get; }
        public ImmutableArray<SyntaxNode> Parameters { get; }
        public Token CloseParenthesisToken { get; }

        public PrototypeSyntax(TypeSyntax returnType, Token identifierToken, Token openParenthesisToken, ImmutableArray<SyntaxNode> parameters, Token closeParenthesisToken)
        {
            ReturnType = returnType;
            IdentifierToken = identifierToken;
            OpenParenthesisToken = openParenthesisToken;
            Parameters = parameters;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ReturnType;
            yield return IdentifierToken;
            yield return OpenParenthesisToken;
            foreach (var parameter in Parameters)
            {
                yield return parameter;
            }
            yield return CloseParenthesisToken;
        }
    }
}