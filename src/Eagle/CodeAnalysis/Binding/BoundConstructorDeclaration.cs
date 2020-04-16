using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
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
    }
}