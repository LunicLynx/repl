using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundFieldExpression : BoundExpression
    {
        public FieldSymbol Field { get; }
        public override TypeSymbol Type => Field.Type;

        public BoundFieldExpression(FieldSymbol field)
        {
            Field = field;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}