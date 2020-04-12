using System.Collections.Immutable;

namespace Eagle.CodeAnalysis
{
    public class EvaluationResult
    {
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public object Value { get; }

        public EvaluationResult(ImmutableArray<Diagnostic> diagnostics, object value)
        {
            Diagnostics = diagnostics;
            Value = value;
        }
    }
}