using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundWhileStatement : BoundStatement
    {
        public BoundExpression Condition { get; }
        public BoundStatement Body { get; }
        public BoundLabel BreakLabel { get; }
        public BoundLabel ContinueLabel { get; }

        public BoundWhileStatement(BoundExpression condition, BoundStatement body, BoundLabel breakLabel, BoundLabel continueLabel)
        {
            Condition = condition;
            Body = body;
            BreakLabel = breakLabel;
            ContinueLabel = continueLabel;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Condition;
            yield return Body;
        }
    }
}