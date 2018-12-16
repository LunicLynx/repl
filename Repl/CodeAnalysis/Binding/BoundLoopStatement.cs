using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    internal class BoundLoopStatement : BoundStatement
    {
        public BoundBlockStatement Block { get; }

        public BoundLoopStatement(BoundBlockStatement block)
        {
            Block = block;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Block;
        }
    }
}