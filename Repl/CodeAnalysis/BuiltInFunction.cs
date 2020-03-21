using System.Reflection;

namespace Repl.CodeAnalysis
{
    public class BuiltInFunction : IInvokable
    {
        private readonly MethodInfo _methodInfo;

        public BuiltInFunction(MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
        }

        public object Invoke(Evaluator evaluator, object target, object[] args)
        {
            return _methodInfo.Invoke(target, args);
        }
    }
}