using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundFieldExpression : BoundExpression
    {
        public BoundExpression Target { get; }
        public FieldSymbol Field { get; }
        public override TypeSymbol Type => Field.Type;

        public BoundFieldExpression(BoundExpression target, FieldSymbol field)
        {
            Target = target;
            Field = field;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}