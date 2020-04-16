using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundConditionalGotoStatement : BoundStatement
    {
        public BoundLabel Label { get; }
        public BoundExpression Condition { get; }
        public bool JumpIfTrue { get; }

        public BoundConditionalGotoStatement(BoundLabel label, BoundExpression condition, bool jumpIfTrue = true)
        {
            Label = label;
            Condition = condition;
            JumpIfTrue = jumpIfTrue;
        }
    }
}