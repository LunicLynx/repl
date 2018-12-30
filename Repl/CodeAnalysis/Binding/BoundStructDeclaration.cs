using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundStructDeclaration : BoundNode
    {
        public TypeSymbol Type { get; }

        public BoundStructDeclaration(TypeSymbol type)
        {
            Type = type;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}