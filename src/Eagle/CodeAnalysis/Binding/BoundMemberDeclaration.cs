using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public abstract class BoundMemberDeclaration : BoundNode
    {
        public MemberSymbol Member { get; }

        protected BoundMemberDeclaration(MemberSymbol member)
        {
            Member = member;
        }

        public abstract override IEnumerable<BoundNode> GetChildren();

    }
}