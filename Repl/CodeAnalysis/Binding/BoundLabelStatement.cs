using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundLabelStatement : BoundStatement
    {
        public LabelSymbol Label { get; }

        public BoundLabelStatement(LabelSymbol label)
        {
            Label = label;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}