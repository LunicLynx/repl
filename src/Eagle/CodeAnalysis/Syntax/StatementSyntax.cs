using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public abstract class StatementSyntax : SyntaxNode
    {
        protected StatementSyntax(SyntaxTree syntaxTree) : base(syntaxTree)
        {
        }

        public abstract override IEnumerable<SyntaxNode> GetChildren();
    }
}