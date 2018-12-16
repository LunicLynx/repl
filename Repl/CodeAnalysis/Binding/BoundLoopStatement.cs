using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundLoopStatement : BoundStatement
    {
        public BoundBlockStatement Body { get; }

        public BoundLoopStatement(BoundBlockStatement body)
        {
            Body = body;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Body;
        }
    }
}