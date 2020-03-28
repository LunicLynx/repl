using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public abstract class StatementSyntax : SyntaxNode
    {
        public abstract override IEnumerable<SyntaxNode> GetChildren();
    }
}