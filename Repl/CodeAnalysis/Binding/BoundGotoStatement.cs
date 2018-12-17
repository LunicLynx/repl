using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundGotoStatement : BoundStatement
    {
        public LabelSymbol Label { get; }

        public BoundGotoStatement(LabelSymbol label)
        {
            Label = label;
        }
        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}