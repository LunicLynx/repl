using System;
using System.Collections.Generic;
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
}