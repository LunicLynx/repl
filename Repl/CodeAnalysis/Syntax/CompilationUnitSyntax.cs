using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Syntax
{
    public class CompilationUnitSyntax : SyntaxNode
    {
        public ImmutableArray<StatementSyntax> Statements { get; }
        public Token EndOfFileToken { get; }

        public CompilationUnitSyntax(ImmutableArray<StatementSyntax> statements, Token endOfFileToken)
        {
            Statements = statements;
            EndOfFileToken = endOfFileToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            foreach (var statement in Statements)
            {
                yield return statement;
            }
            yield return EndOfFileToken;
        }
    }
}