using System.Collections.Generic;
using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
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
    }
}