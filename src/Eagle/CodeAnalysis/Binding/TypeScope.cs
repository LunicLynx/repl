using System.Collections.Immutable;
using System.Linq;

namespace Eagle.CodeAnalysis.Binding
{
    public class TypeScope : IScope
    {
        public IScope Parent { get; }
        public TypeSymbol Type { get; }

        public TypeScope(IScope parent, TypeSymbol type)
        {
            Parent = parent;
            Type = type;
        }

        public bool TryDeclare(Symbol symbol)
        {
            if (Type.Members.Any(m => m.Name == symbol.Name))
                return false;

            Type.Members = Type.Members.Add((MemberSymbol)symbol);
            return true;
        }

        public bool TryLookup(string name, out Symbol symbol)
        {
            var member = Type.Members.FirstOrDefault(m => m.Name == name);
            if (member != null)
            {
                symbol = member;
                return true;
            }

            if (Parent != null)
                return Parent.TryLookup(name, out symbol);

            symbol = null;
            return false;
        }

        public ImmutableArray<Symbol> GetDeclaredSymbols()
        {
            return Type.Members.ToImmutableArray<Symbol>();
        }
    }
}