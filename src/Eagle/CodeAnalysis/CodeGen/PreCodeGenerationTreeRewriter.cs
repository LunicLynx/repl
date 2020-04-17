using Eagle.CodeAnalysis.Binding;

namespace Eagle.CodeAnalysis.CodeGen
{
    public class PreCodeGenerationTreeRewriter : BoundTreeRewriter
    {
        protected override BoundExpression RewriteBinaryExpression(BoundBinaryExpression node)
        {
            if (node.Operator.Kind == BoundBinaryOperatorKind.Concatenation)
            {
                // turn this into a call to concat of the string
            }

            return base.RewriteBinaryExpression(node);
        }
    }
}