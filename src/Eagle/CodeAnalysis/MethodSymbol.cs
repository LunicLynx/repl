using System.Collections.Immutable;
using System.Linq;
using Eagle.CodeAnalysis.Syntax;

namespace Eagle.CodeAnalysis
{
    public class MethodSymbol : MemberSymbol, IInvokableSymbol
    {
        public ImmutableArray<ParameterSymbol> Parameters { get; }
        public bool Extern { get; }
        public override SymbolKind Kind => SymbolKind.Method;
        TypeSymbol IInvokableSymbol.Type => Type;

        public MethodSymbol(TypeSymbol returnType, string name, ImmutableArray<ParameterSymbol> parameters)
            : base(name, returnType)
        {
            Parameters = parameters;
        }

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Parameters.Select(p => p.ToString()))}): {Type} ";
        }
    }
}