using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundExternDeclaration : BoundNode
    {
        public FunctionSymbol Function { get; }

        public BoundExternDeclaration(FunctionSymbol function)
        {
            Function = function;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}