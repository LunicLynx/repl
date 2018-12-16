using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundLiteralExpression : BoundExpression
    {
        public override Type Type => Value.GetType();

        public object Value { get; }

        public BoundLiteralExpression(object value)
        {
            Value = value;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}