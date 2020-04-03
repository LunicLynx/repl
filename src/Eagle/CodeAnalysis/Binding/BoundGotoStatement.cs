using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundGotoStatement : BoundStatement
    {
        public BoundLabel Label { get; }

        public BoundGotoStatement(BoundLabel label)
        {
            Label = label;
        }
        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}