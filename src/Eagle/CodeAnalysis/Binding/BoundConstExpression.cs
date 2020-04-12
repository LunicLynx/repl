using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundConstExpression : BoundExpression
    {
        public ConstSymbol Const { get; }

        public BoundConstExpression(ConstSymbol @const)
        {
            Const = @const;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }

        public override TypeSymbol Type => Const.Type;
    }
}