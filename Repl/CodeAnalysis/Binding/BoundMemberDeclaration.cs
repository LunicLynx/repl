using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
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

    public class BoundMethodDeclaration : BoundMemberDeclaration
    {
        public MethodSymbol Method { get; }
        public BoundBlockStatement Body { get; }

        public BoundMethodDeclaration(MethodSymbol method, BoundBlockStatement body) : base(method)
        {
            Method = method;
            Body = body;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Body;
        }
    }

    public class BoundPropertyDeclaration : BoundMemberDeclaration
    {
        public PropertySymbol Property { get; }

        public BoundPropertyDeclaration(PropertySymbol property) : base(property)
        {
            Property = property;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }

    public class BoundFieldDeclaration : BoundMemberDeclaration
    {
        public FieldSymbol Field { get; }
        public BoundExpression Initializer { get; }

        public BoundFieldDeclaration(FieldSymbol field, BoundExpression initializer) : base(field)
        {
            Field = field;
            Initializer = initializer;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            if (Initializer != null)
                yield return Initializer;
        }
    }
}