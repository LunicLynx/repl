using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
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
}