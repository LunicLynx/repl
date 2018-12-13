using System;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundVariableExpression : BoundExpression
    {
        public VariableSymbol Variable { get; }
        public override Type Type => Variable.Type;

        public BoundVariableExpression(VariableSymbol variable)
        {
            Variable = variable;
        }
    }
}