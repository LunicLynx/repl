using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundGlobalScope
    {
        public BoundGlobalScope Previous { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ImmutableArray<Symbol> Symbols { get; }
        public BoundUnit Unit { get; }

        public BoundGlobalScope(BoundGlobalScope previous, ImmutableArray<Diagnostic> diagnostics, ImmutableArray<Symbol> symbols, BoundUnit unit)
        {
            Previous = previous;
            Diagnostics = diagnostics;
            Symbols = symbols;
            Unit = unit;
        }
    }
}