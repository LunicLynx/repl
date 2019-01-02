using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundMemberAccessExpression : BoundExpression
    {
        public override TypeSymbol Type => Member.Type;
        public BoundExpression Target { get; }
        public MemberSymbol Member { get; }

        public BoundMemberAccessExpression(BoundExpression target, MemberSymbol member)
        {
            Target = target;
            Member = member;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}