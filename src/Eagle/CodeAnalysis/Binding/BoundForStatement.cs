using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundForStatement : BoundLoopStatementBase
    {
        public VariableSymbol Variable { get; }
        public BoundExpression LowerBound { get; }
        public BoundExpression UpperBound { get; }
        public BoundStatement Body { get; }

        public BoundForStatement(VariableSymbol variable, BoundExpression lowerBound, BoundExpression upperBound, BoundStatement body, BoundLabel breakLabel, BoundLabel continueLabel) : base(breakLabel, continueLabel)
        {
            Variable = variable;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Body = body;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return LowerBound;
            yield return UpperBound;
            yield return Body;
        }
    }

    public abstract class BoundLoopStatementBase : BoundStatement
    {
        protected BoundLoopStatementBase(BoundLabel breakLabel, BoundLabel continueLabel)
        {
            BreakLabel = breakLabel;
            ContinueLabel = continueLabel;
        }

        public BoundLabel BreakLabel { get; }
        public BoundLabel ContinueLabel { get; }
    }
}