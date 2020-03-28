using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundForStatement : BoundStatement
    {
        public VariableSymbol Variable { get; }
        public BoundExpression LowerBound { get; }
        public BoundExpression UpperBound { get; }
        public BoundBlockStatement Body { get; }

        public BoundForStatement(VariableSymbol variable, BoundExpression lowerBound, BoundExpression upperBound, BoundBlockStatement body)
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
}