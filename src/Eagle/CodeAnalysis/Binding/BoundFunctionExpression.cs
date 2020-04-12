using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    internal class BoundFunctionExpression : BoundExpression
    {
        public FunctionSymbol FunctionSymbol { get; }

        public BoundFunctionExpression(FunctionSymbol functionSymbol)
        {
            FunctionSymbol = functionSymbol;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }

        public override TypeSymbol Type => FunctionSymbol.Type;
    }
}