using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundTypeExpression : BoundExpression
    {
        public BoundTypeExpression(TypeSymbol type)
        {
            Type = type;
        }

        public override TypeSymbol Type { get; }
    }
}