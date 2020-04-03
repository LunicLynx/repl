using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundLoopStatement : BoundStatement
    {
        public BoundStatement Body { get; }
        public BoundLabel BreakLabel { get; }
        public BoundLabel ContinueLabel { get; }

        public BoundLoopStatement(BoundStatement body, BoundLabel breakLabel, BoundLabel continueLabel)
        {
            Body = body;
            BreakLabel = breakLabel;
            ContinueLabel = continueLabel;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Body;
        }
    }
}