using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundConstructorDeclaration : BoundMemberDeclaration
    {
        public ConstructorSymbol Constructor { get; }
        public BoundBlockStatement Body { get; }

        public BoundConstructorDeclaration(ConstructorSymbol constructor, BoundBlockStatement body) : base(constructor)
        {
            Constructor = constructor;
            Body = body;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Body;
        }
    }
}