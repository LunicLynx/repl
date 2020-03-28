using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundVariableExpression : BoundExpression
    {
        public VariableSymbol Variable { get; }
        public override TypeSymbol Type => Variable.Type;

        public BoundVariableExpression(VariableSymbol variable)
        {
            Variable = variable;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}