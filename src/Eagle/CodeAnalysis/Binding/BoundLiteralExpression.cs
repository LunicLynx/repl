using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundLiteralExpression : BoundExpression
    {
        public override TypeSymbol Type { get; }

        public object Value { get; }

        public BoundLiteralExpression(TypeSymbol type, object value)
        {
            Type = type;
            Value = value;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}