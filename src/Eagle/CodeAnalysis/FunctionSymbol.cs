using System.Collections.Immutable;
using System.Linq;
using Eagle.CodeAnalysis.Syntax;

namespace Eagle.CodeAnalysis
{
    public class FunctionSymbol : Symbol, IInvokableSymbol
    {
        public TypeSymbol Type { get; }
        public bool Extern { get; }
        public override SymbolKind Kind => SymbolKind.Function;
        public ImmutableArray<ParameterSymbol> Parameters { get; }

        public FunctionSymbol(string name, ImmutableArray<ParameterSymbol> parameters, TypeSymbol returnType, bool @extern = false)
            : base(name)
        {
            Parameters = parameters;
            Type = returnType;
            Extern = @extern;
        }

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Parameters.Select(p => p.ToString()))}): {Type} ";
        }
    }
}