using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    internal class BoundContinueStatement : BoundStatement
    {
        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}