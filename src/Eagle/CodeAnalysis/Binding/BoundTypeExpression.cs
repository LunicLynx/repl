using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundTypeExpression : BoundExpression
    {
        public BoundTypeExpression(TypeSymbol type)
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