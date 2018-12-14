using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundBlockStatement : BoundStatement
    {
        public ImmutableArray<BoundStatement> Statements { get; }

        public BoundBlockStatement(ImmutableArray<BoundStatement> statements)
        {
            Statements = statements;
        }
    }
}