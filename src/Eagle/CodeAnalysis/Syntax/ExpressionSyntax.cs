using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public abstract class ExpressionSyntax : SyntaxNode
    {
        protected ExpressionSyntax(SyntaxTree syntaxTree) : base(syntaxTree)
        {
        }

        public abstract override IEnumerable<SyntaxNode> GetChildren();
    }
}