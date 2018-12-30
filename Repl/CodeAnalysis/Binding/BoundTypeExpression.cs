using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundTypeExpression : BoundExpression
    {
        public TypeSymbol TypeSymbol { get; }

        public BoundTypeExpression(TypeSymbol typeSymbol)
        {
            TypeSymbol = typeSymbol;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }

        public override Type Type => TypeSymbol.ClrType;
    }
}