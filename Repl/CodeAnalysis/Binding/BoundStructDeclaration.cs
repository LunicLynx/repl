using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundStructDeclaration : BoundNode
    {
        public TypeSymbol Type { get; }
        public ImmutableArray<BoundMemberDeclaration> Members { get; }

        public BoundStructDeclaration(TypeSymbol type, ImmutableArray<BoundMemberDeclaration> members)
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