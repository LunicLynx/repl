using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundMemberDeclaration : BoundNode
    {
        public MemberSymbol Member { get; }
        public BoundExpression Initializer { get; }

        public BoundMemberDeclaration(MemberSymbol member, BoundExpression initializer)
        {
            Member = member;
            Initializer = initializer;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            if (Initializer != null)
                yield return Initializer;
        }
    }
}