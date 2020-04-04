using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundIfStatement : BoundStatement
    {
        public BoundExpression Condition { get; }
        public BoundStatement ThenStatement { get; }
        public BoundStatement ElseStatement { get; }

        public BoundIfStatement(BoundExpression condition, BoundStatement thenStatement, BoundStatement elseStatement)
        {
            Condition = condition;
            ThenStatement = thenStatement;
            ElseStatement = elseStatement;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Condition;
            yield return ThenStatement;
            yield return ElseStatement;
        }
    }
}