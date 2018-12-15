using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ContinueStatementSyntax : StatementSyntax
    {
        public Token ContinueKeyword { get; }

        public ContinueStatementSyntax(Token continueKeyword)
        {
            ContinueKeyword = continueKeyword;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ContinueKeyword;
        }
    }
}