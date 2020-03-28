using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundClassDeclaration : BoundNode
    {
        public TypeSymbol Type { get; }
        public ImmutableArray<BoundMemberDeclaration> Members { get; }

        public BoundClassDeclaration(TypeSymbol type, ImmutableArray<BoundMemberDeclaration> members)
        {
            Type = type;
            Members = members;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            foreach (var member in Members)
            {
                yield return member;
            }
        }
    }
}