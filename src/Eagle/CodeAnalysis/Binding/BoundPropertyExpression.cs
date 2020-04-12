using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundPropertyExpression : BoundExpression
    {
        public override TypeSymbol Type => Property.Type;
        public BoundExpression Target { get; }
        public PropertySymbol Property { get; }

        public BoundPropertyExpression(BoundExpression target, PropertySymbol property)
        {
            Target = target;
            Property = property;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}