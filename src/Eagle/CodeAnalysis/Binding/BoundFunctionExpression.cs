using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    internal class BoundFunctionExpression : BoundExpression
    {
        public FunctionSymbol FunctionSymbol { get; }

        public BoundFunctionExpression(FunctionSymbol functionSymbol)
        {
            FunctionSymbol = functionSymbol;
        }

        public override TypeSymbol Type => FunctionSymbol.Type;
    }
}