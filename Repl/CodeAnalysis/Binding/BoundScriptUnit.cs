using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundScriptUnit : BoundUnit
    {
        public ImmutableArray<BoundNode> Nodes { get; }

        public BoundScriptUnit(ImmutableArray<BoundNode> nodes)
        {
            Nodes = nodes;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            return Nodes;
        }
    }
}