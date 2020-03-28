using System;

namespace Repl.CodeAnalysis.Binding
{
    public abstract class BoundExpression : BoundNode
    {
        public abstract TypeSymbol Type { get; }
    }
}