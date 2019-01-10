using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis.Syntax
{
    public abstract class SyntaxNode
    {
        public virtual IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>();
        }

        public virtual TextSpan Span
        {
            get
            {
                var first = GetChildren().First().Span;
                var last = GetChildren().Last().Span;
                return TextSpan.FromBounds(first.Start, last.End);
            }
        }
    }

    public class BaseTypeSyntax : SyntaxNode
    {
        public Token ColonToken { get; }
        public ImmutableArray<SyntaxNode> Types { get; }

        public BaseTypeSyntax(Token colonToken, ImmutableArray<SyntaxNode> types)
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