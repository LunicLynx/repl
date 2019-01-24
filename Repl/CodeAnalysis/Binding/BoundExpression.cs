using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public abstract class BoundExpression : BoundNode
    {
        public abstract TypeSymbol Type { get; }
    }

    public class BoundThisExpression : BoundExpression
    {
        public BoundThisExpression()
        {
            
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            throw new NotImplementedException();
        }

        public override TypeSymbol Type { get; }
    }
}