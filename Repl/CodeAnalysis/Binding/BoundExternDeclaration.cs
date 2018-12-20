using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundExternDeclaration : BoundStatement
    {
        public string Name { get; }

        public BoundExternDeclaration(string name)
        {
            Name = name;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}