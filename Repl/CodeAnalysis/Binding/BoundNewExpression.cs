using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundNewExpression : BoundExpression
    {
        public TypeSymbol TypeSymbol { get; }

        public BoundNewExpression(TypeSymbol type)
        {
            TypeSymbol = type;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }

        public override Type Type => TypeSymbol?.ClrType ?? typeof(int);
    }
}