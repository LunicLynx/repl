using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundConstDeclaration : BoundNode

    {
        public ConstSymbol Const { get; }
        public object Value { get; }

        public BoundConstDeclaration(ConstSymbol @const, object value)
        {
            Const = @const;
            Value = value;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}