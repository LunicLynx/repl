using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundGlobalScope
    {
        public BoundGlobalScope Previous { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public FunctionSymbol MainFunction { get; }
        public FunctionSymbol ScriptFunction { get; }
        public ImmutableArray<Symbol> Symbols { get; }
        public ImmutableArray<BoundStatement> Statements { get; }

        public BoundGlobalScope(BoundGlobalScope previous, 
            ImmutableArray<Diagnostic> diagnostics, 
            FunctionSymbol mainFunction,
            FunctionSymbol scriptFunction,
            ImmutableArray<Symbol> symbols, 
            ImmutableArray<BoundStatement> statements)
        {
            Previous = previous;
            Diagnostics = diagnostics;
            MainFunction = mainFunction;
            ScriptFunction = scriptFunction;
            Symbols = symbols;
            Statements = statements;
        }
    }
}