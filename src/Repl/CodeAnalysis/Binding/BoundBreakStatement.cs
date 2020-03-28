using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundBreakStatement : BoundStatement
    {
        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}