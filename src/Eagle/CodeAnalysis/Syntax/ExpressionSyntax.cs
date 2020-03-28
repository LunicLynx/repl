using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public abstract class ExpressionSyntax : SyntaxNode
    {
        public abstract override IEnumerable<SyntaxNode> GetChildren();
    }
}