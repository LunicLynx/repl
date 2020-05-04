using System.Collections.Generic;
using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundConstructorCallExpression : BoundExpression, IInvocation
    {
        public override TypeSymbol Type => Constructor.Type;
        public ConstructorSymbol Constructor { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }

        IInvokableSymbol IInvocation.Invokable => Constructor;

        public BoundConstructorCallExpression(ConstructorSymbol constructor, ImmutableArray<BoundExpression> arguments)
        {
            Constructor = constructor;
            Arguments = arguments;
        }
    }
}