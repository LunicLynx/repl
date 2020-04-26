using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundScope : IScope
    {
        public IScope Parent { get; }
        private readonly List<Symbol> _symbols = new List<Symbol>();

        public BoundScope(IScope parent)
        {
            Parent = parent;
        }

        public bool TryDeclare(Symbol symbol)
        {
            // types with same name can only be declared once
            // aliases with same name can only be declared once
            // functions with same name and same parameters can only be declared once
            // variables can be redeclared

            if (!(symbol is VariableSymbol))
            {
                var existingSymbols = _symbols.Where(s => s.Name == symbol.Name).ToList();
                if (existingSymbols.Any())
                {
                    // only if it is a function symbol we allow the same name twice
                    if (!(symbol is FunctionSymbol functionSymbol))
                        return false;

                    // if any existing is not a function then this cannot be declared
                    if (existingSymbols.Any(e => e.GetType() != typeof(FunctionSymbol)))
                        return false;

                    // if all functions have different parameters we can declare it
                    var functions = existingSymbols.Cast<FunctionSymbol>();
                    return functions.All(f => !functionSymbol.Parameters.SequenceEqual(f.Parameters));
                }
            }

            _symbols.Add(symbol);
            return true;
        }

        public bool TryLookup(SymbolKind[] kinds, string name, out Symbol[] symbols)
        {
            var allKinds = !kinds.Any();
            symbols = _symbols.Where(m => m.Name == name && (allKinds || kinds.Contains(m.Kind))).ToArray();
            if (symbols.Any())
                return true;

            if (Parent == null)
                return false;

            return Parent.TryLookup(kinds, name, out symbols);
        }

        public ImmutableArray<Symbol> GetDeclaredSymbols()
        {
            return _symbols.ToImmutableArray();
        }
    }
}