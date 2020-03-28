using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundAliasDeclaration : BoundNode
    {
        public AliasSymbol Alias { get; }

        public BoundAliasDeclaration(AliasSymbol alias)
        {
            Alias = alias;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}