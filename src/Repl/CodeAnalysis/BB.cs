using Repl.CodeAnalysis.Binding;

namespace Repl.CodeAnalysis
{
    public class BB : IInvokable
    {
        private readonly BoundBlockStatement _bbs;
        private readonly ParameterSymbol[] _parameters;

        public BB(BoundBlockStatement bbs, ParameterSymbol[] parameters)
        {
            _bbs = bbs;
            _parameters = parameters;
        }

        public object Invoke(Evaluator evaluator, object target, object[] args)
        {
            return evaluator.RunBlock(_parameters, target, args, _bbs);
        }
    }
}