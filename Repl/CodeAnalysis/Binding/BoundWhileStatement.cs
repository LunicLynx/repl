using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    internal class BoundWhileStatement : BoundStatement
    {
        public BoundExpression Condition { get; }
        public BoundBlockStatement Block { get; }

        public BoundWhileStatement(BoundExpression condition, BoundBlockStatement block)
        {
            Condition = condition;
            Block = block;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Condition;
            yield return Block;
        }
    }
}