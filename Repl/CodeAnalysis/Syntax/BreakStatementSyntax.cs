using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    internal class BreakStatementSyntax : StatementSyntax
    {
        public Token BreakKeyword { get; }

        public BreakStatementSyntax(Token breakKeyword)
        {
            BreakKeyword = breakKeyword;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return BreakKeyword;
        }
    }
}