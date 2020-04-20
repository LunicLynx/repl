using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundProgram
    {
        public BoundProgram(BoundProgram previous,
            ImmutableArray<Diagnostic> diagnostics,
            FunctionSymbol? mainFunction,
            FunctionSymbol? scriptFunction,
            ImmutableDictionary<IInvokableSymbol, BoundBlockStatement> functions)
        {
            Previous = previous;
            Diagnostics = diagnostics;
            MainFunction = mainFunction;
            ScriptFunction = scriptFunction;
            Functions = functions;
        }

        public BoundProgram Previous { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public FunctionSymbol? MainFunction { get; }
        public FunctionSymbol? ScriptFunction { get; }

        public ImmutableDictionary<IInvokableSymbol, BoundBlockStatement> Functions { get; }
    }
}