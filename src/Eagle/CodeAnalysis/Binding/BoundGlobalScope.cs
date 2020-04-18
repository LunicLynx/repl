using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundGlobalScope
    {
        public BoundGlobalScope Previous { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public FunctionSymbol? MainFunction { get; }
        public FunctionSymbol? ScriptFunction { get; }
        public ImmutableArray<Symbol> Symbols { get; }
        public ImmutableDictionary<IInvokableSymbol, BoundBlockStatement> FunctionBodies { get; }
        //public ImmutableArray<BoundStatement> Statements { get; }

        public BoundGlobalScope(BoundGlobalScope previous, 
            ImmutableArray<Diagnostic> diagnostics, 
            FunctionSymbol? mainFunction,
            FunctionSymbol? scriptFunction,
            ImmutableArray<Symbol> symbols, 
            //ImmutableArray<BoundStatement> statements
            ImmutableDictionary<IInvokableSymbol, BoundBlockStatement> functionBodies
            )
        {
            Previous = previous;
            Diagnostics = diagnostics;
            MainFunction = mainFunction;
            ScriptFunction = scriptFunction;
            Symbols = symbols;
            FunctionBodies = functionBodies;
            //Statements = statements;
        }
    }
}