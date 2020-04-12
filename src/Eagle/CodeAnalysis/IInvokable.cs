namespace Eagle.CodeAnalysis
{
    public interface IInvokable
    {
        object Invoke(Evaluator evaluator, object target, object[] args);
    }
}