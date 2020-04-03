using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundLabelStatement : BoundStatement
    {
        public BoundLabel Label { get; }

        public BoundLabelStatement(BoundLabel label)
        {
            Label = label;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}