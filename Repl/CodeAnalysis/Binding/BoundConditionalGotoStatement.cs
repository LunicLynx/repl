using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundConditionalGotoStatement : BoundStatement
    {
        public LabelSymbol Label { get; }
        public BoundExpression Condition { get; }
        public bool JumpIfFalse { get; }

        public BoundConditionalGotoStatement(LabelSymbol label, BoundExpression condition, bool jumpIfFalse = false)
        {
            Label = label;
            Condition = condition;
            JumpIfFalse = jumpIfFalse;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Condition;
        }
    }
}