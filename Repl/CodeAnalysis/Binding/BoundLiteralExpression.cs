using System;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundLiteralExpression : BoundExpression
    {
        public object Value { get; }

        public override Type Type => Value.GetType();

        public BoundLiteralExpression(object value)
        {
            Value = value;
        }
    }
}