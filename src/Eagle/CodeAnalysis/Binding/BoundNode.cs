using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public abstract class BoundNode
    {
        public abstract IEnumerable<BoundNode> GetChildren();
    }
}