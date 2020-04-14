using System.Collections.Immutable;
using System.Linq;
using Eagle.CodeAnalysis.Syntax;

namespace Eagle.CodeAnalysis
{
    public interface IInvokableSymbol
    {
        string Name { get; }
        ImmutableArray<ParameterSymbol> Parameters { get; }
        TypeSymbol Type { get; }

        bool Extern { get; }

        IInvokableDeclarationSyntax Declaration { get; }
    }

    public class FunctionSymbol : Symbol, IInvokableSymbol
    {
        public TypeSymbol Type { get; }
        public FunctionDeclarationSyntax Declaration { get; }
        public bool Extern { get; }
        public override SymbolKind Kind => SymbolKind.Function;
        public ImmutableArray<ParameterSymbol> Parameters { get; }

        IInvokableDeclarationSyntax IInvokableSymbol.Declaration => Declaration;

        public FunctionSymbol(string name, ImmutableArray<ParameterSymbol> parameters, TypeSymbol returnType, FunctionDeclarationSyntax declaration = null, bool @extern = false)
            : base(name)
        {
            Parameters = parameters;
            Type = returnType;
            Declaration = declaration;
            Extern = @extern;
        }

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Parameters.Select(p => p.ToString()))}): {Type} ";
        }
    }
}