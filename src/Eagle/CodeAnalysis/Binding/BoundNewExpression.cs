using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundNewExpression : BoundExpression
    {
        public BoundNewExpression(TypeSymbol type)
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