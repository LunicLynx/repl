using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundParameterExpression : BoundExpression
    {
        public ParameterSymbol Parameter { get; }
        public override Type Type => Parameter.Type.ClrType;

        public BoundParameterExpression(ParameterSymbol parameter)
        {
            Parameter = parameter;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}