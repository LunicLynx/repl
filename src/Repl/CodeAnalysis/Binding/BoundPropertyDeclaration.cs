using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundPropertyDeclaration : BoundMemberDeclaration
    {
        public BoundBlockStatement GetBody { get; }
        public PropertySymbol Property { get; }
        public BoundExpression Initializer { get; }

        public BoundPropertyDeclaration(PropertySymbol property, BoundExpression initializer) : base(property)
        {
            Property = property;
            Initializer = initializer;
        }

        public BoundPropertyDeclaration(PropertySymbol property, BoundBlockStatement getBody) : base(property)
        {
            Property = property;
            GetBody = getBody;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            if (Initializer != null)
                yield return Initializer;
        }
    }
}