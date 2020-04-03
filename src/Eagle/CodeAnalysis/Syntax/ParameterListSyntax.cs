using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Syntax
{
    public class ParameterListSyntax : SyntaxNode
    {
        public Token OpenParenthesisToken { get; }
        public ImmutableArray<SyntaxNode> Parameters { get; }
        public Token CloseParenthesisToken { get; }

        public ParameterListSyntax(SyntaxTree syntaxTree, Token openParenthesisToken,
            ImmutableArray<SyntaxNode> parameters, Token closeParenthesisToken)
            : base(syntaxTree)
        {
            OpenParenthesisToken = openParenthesisToken;
            Parameters = parameters;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParenthesisToken;
            foreach (var parameter in Parameters)
            {
                yield return parameter;
            }
            yield return CloseParenthesisToken;
        }
    }
}