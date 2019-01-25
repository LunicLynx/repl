using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundThisExpression : BoundExpression
    {
        public BoundThisExpression(TypeSymbol type)
        {
            Type = type;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }

        public override TypeSymbol Type { get; }
    }
}