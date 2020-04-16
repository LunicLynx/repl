using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundThisExpression : BoundExpression
    {
        public BoundThisExpression(TypeSymbol type)
        {
            Type = type;
        }

        public override TypeSymbol Type { get; }
    }
}