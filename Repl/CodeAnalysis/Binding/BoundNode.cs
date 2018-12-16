using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public abstract class BoundNode
    {
        public abstract IEnumerable<BoundNode> GetChildren();
    }
}