using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ThisExpressionSyntax : ExpressionSyntax
    {
        public Token ThisKeyword { get; }

        public ThisExpressionSyntax(SyntaxTree syntaxTree, Token thisKeyword)
            : base(syntaxTree)
        {
            ThisKeyword = thisKeyword;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ThisKeyword;
        }
    }
}