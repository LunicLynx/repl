using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundGlobalScope
    {
        public BoundGlobalScope Previous { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ImmutableArray<Symbol> Symbols { get; }
        public BoundStatement Statement { get; }

        public BoundGlobalScope(BoundGlobalScope previous, ImmutableArray<Diagnostic> diagnostics, ImmutableArray<Symbol> symbols, BoundStatement statement)
        {
            Previous = previous;
            Diagnostics = diagnostics;
            Symbols = symbols;
            Statement = statement;
        }
    }
}