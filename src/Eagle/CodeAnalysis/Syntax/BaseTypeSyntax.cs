using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Syntax
{
    public class BaseTypeSyntax : SyntaxNode
    {
        public Token ColonToken { get; }
        public ImmutableArray<SyntaxNode> Types { get; }

        public BaseTypeSyntax(SyntaxTree syntaxTree, Token colonToken, ImmutableArray<SyntaxNode> types)
            : base(syntaxTree)
        {
            ColonToken = colonToken;
            Types = types;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ColonToken;
            foreach (var syntaxNode in Types)
            {
                yield return syntaxNode;
            }
        }
    }
}