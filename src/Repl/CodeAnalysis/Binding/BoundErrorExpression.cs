using System;
using System.Collections.Generic;
using System.Text;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundErrorExpression : BoundExpression
    {
        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }

        public override TypeSymbol Type => TypeSymbol.Error;
    }
}
