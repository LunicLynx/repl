using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundFieldDeclaration : BoundMemberDeclaration
    {
        public FieldSymbol Field { get; }
        public BoundExpression Initializer { get; }

        public BoundFieldDeclaration(FieldSymbol field, BoundExpression initializer) : base(field)
        {
            Field = field;
            Initializer = initializer;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            if (Initializer != null)
                yield return Initializer;
        }
    }
}