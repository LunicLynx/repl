using System.Collections.Generic;
using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundScope : IScope
    {
        public IScope Parent { get; }
        private readonly Dictionary<string, Symbol> _symbols = new Dictionary<string, Symbol>();

        public BoundScope(IScope parent)
        {
            Parent = parent;
        }

        public bool TryDeclare(Symbol symbol)
        {
            if (_symbols.ContainsKey(symbol.Name))
                return false;

            _symbols.Add(symbol.Name, symbol);
            return true;
        }

        public bool TryLookup(string name, out Symbol symbol)
        {
            if (_symbols.TryGetValue(name, out symbol))
                return true;

            if (Parent == null)
                return false;

            return Parent.TryLookup(name, out symbol);
        }

        public ImmutableArray<Symbol> GetDeclaredSymbols()
        {
            return _symbols.Values.ToImmutableArray();
        }
    }
}