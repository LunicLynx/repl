using System.Collections.Generic;
using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundNewInstanceExpression : BoundExpression
    {
        public override TypeSymbol Type { get; }
        public ConstructorSymbol ConstructorSymbol { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }

        public BoundNewInstanceExpression(TypeSymbol type, ConstructorSymbol constructorSymbol, ImmutableArray<BoundExpression> arguments)
        {
            Type = type;
            ConstructorSymbol = constructorSymbol;
            Arguments = arguments;
        }
    }
}