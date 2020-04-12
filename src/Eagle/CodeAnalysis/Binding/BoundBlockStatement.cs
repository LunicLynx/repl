using System.Collections.Generic;
using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundBlockStatement : BoundStatement
    {
        public ImmutableArray<BoundStatement> Statements { get; }

        public BoundBlockStatement(ImmutableArray<BoundStatement> statements)
        {
            Statements = statements;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            foreach (var statement in Statements)
            {
                yield return statement;
            }
        }
    }
}