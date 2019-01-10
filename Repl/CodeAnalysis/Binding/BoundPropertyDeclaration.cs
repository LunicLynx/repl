using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundPropertyDeclaration : BoundMemberDeclaration
    {
        public PropertySymbol Property { get; }
        public BoundExpression Initializer { get; }

        public BoundPropertyDeclaration(PropertySymbol property, BoundExpression initializer) : base(property)
        {
            Property = property;
            Initializer = initializer;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            if (Initializer != null)
                yield return Initializer;
        }
    }
}