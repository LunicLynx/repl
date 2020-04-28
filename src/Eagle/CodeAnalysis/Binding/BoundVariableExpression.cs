using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundVariableExpression : BoundExpression
    {
        public VariableSymbol Variable { get; }
        public override TypeSymbol Type => Variable.Type.IsReference ? Variable.Type.ElementType : Variable.Type;

        public BoundVariableExpression(VariableSymbol variable)
        {
            Variable = variable;
        }
    }
}