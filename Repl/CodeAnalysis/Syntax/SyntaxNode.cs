using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public abstract class SyntaxNode
    {
        public virtual IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>();
        }
    }
}