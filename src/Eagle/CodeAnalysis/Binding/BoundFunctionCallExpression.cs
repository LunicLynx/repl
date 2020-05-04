using System.Collections.Generic;
using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public interface IInvocation
    {
        IInvokableSymbol Invokable { get; }
        ImmutableArray<BoundExpression> Arguments { get; }
    }

    public class BoundFunctionCallExpression : BoundExpression, IInvocation
    {
        public FunctionSymbol Function { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }

        IInvokableSymbol IInvocation.Invokable => Function;

        public BoundFunctionCallExpression(FunctionSymbol function, ImmutableArray<BoundExpression> arguments)
        {
            Function = function;
            Arguments = arguments;
        }

        public override TypeSymbol Type => Function.Type;
    }
}