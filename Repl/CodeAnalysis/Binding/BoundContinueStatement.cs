using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundContinueStatement : BoundStatement
    {
        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}