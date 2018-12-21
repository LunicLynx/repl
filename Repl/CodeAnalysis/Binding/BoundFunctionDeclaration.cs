using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundFunctionDeclaration : BoundStatement
    {
        public FunctionSymbol Function { get; }
        public BoundBlockStatement Body { get; }

        public BoundFunctionDeclaration(FunctionSymbol function, BoundBlockStatement body)
        {
            Function = function;
            Body = body;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Body;
        }
    }
}