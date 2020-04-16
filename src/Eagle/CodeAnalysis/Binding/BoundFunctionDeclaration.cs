using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundFunctionDeclaration : BoundNode
    {
        public FunctionSymbol Function { get; }
        public BoundBlockStatement Body { get; }

        public BoundFunctionDeclaration(FunctionSymbol function, BoundBlockStatement body)
        {
            Function = function;
            Body = body;
        }
    }
}