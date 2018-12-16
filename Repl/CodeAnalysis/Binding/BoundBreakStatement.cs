using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    internal class BoundBreakStatement : BoundStatement
    {
        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}