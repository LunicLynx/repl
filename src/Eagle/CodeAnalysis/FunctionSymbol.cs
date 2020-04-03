using System.Collections.Immutable;
using System.Linq;

namespace Repl.CodeAnalysis
{
    public class FunctionSymbol : Symbol
    {
        public TypeSymbol ReturnType { get; }
        public override SymbolKind Kind => SymbolKind.Function;
        public override string Name { get; }
        public ImmutableArray<ParameterSymbol> Parameters { get; }

        public FunctionSymbol(string name, ImmutableArray<ParameterSymbol> parameters, TypeSymbol returnType)
        {
            Name = name;
            Parameters = parameters;
            ReturnType = returnType;
        }

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Parameters.Select(p => p.ToString()))}): {ReturnType} ";
        }
    }
}