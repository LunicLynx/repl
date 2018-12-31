using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Syntax
{
    public class PrototypeSyntax : SyntaxNode
    {
        public Token IdentifierToken { get; }
        public Token OpenParenthesisToken { get; }
        public ImmutableArray<SyntaxNode> Parameters { get; }
        public Token CloseParenthesisToken { get; }
        public TypeAnnotationSyntax ReturnType { get; }

        public PrototypeSyntax(Token identifierToken, Token openParenthesisToken, ImmutableArray<SyntaxNode> parameters, Token closeParenthesisToken, TypeAnnotationSyntax returnType)
        {
            IdentifierToken = identifierToken;
            OpenParenthesisToken = openParenthesisToken;
            Parameters = parameters;
            CloseParenthesisToken = closeParenthesisToken;
            ReturnType = returnType;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return OpenParenthesisToken;
            foreach (var parameter in Parameters)
            {
                yield return parameter;
            }
            yield return CloseParenthesisToken;
            if (ReturnType != null)
            {
                yield return ReturnType;
            }
        }
    }
}