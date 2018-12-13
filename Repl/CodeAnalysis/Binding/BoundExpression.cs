using System;

namespace Repl.CodeAnalysis.Binding
{
    public abstract class BoundExpression
    {
        public abstract Type Type { get; }
    }
}