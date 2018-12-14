using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class CompilationUnitSyntax : SyntaxNode
    {
        public StatementSyntax Statement { get; }
        public Token EndOfFileToken { get; }

        public CompilationUnitSyntax(StatementSyntax statement, Token endOfFileToken)
        {
            Statement = statement;
            EndOfFileToken = endOfFileToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Statement;
            yield return EndOfFileToken;
        }
    }
}