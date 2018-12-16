using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundWhileStatement : BoundStatement
    {
        public BoundExpression Condition { get; }
        public BoundBlockStatement Body { get; }

        public BoundWhileStatement(BoundExpression condition, BoundBlockStatement body)
        {
            Condition = condition;
            Body = body;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Condition;
            yield return Body;
        }
    }
}