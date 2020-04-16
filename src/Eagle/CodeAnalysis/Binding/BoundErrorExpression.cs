using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundErrorExpression : BoundExpression
    {
        public override TypeSymbol Type => TypeSymbol.Error;
    }
}
