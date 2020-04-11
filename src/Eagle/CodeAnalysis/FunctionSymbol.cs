using System.Collections.Immutable;
using System.Linq;
using Repl.CodeAnalysis.Syntax;

namespace Repl.CodeAnalysis
{
    public interface IInvokableSymbol
    {
        IInvokableDeclarationSyntax Declaration { get; }
        TypeSymbol ReturnType { get; }
    }

    public class FunctionSymbol : Symbol, IInvokableSymbol
    {
        public TypeSymbol ReturnType { get; }
        public FunctionDeclarationSyntax Declaration { get; }
        public bool Extern { get; }
        public override SymbolKind Kind => SymbolKind.Function;
        public ImmutableArray<ParameterSymbol> Parameters { get; }

        IInvokableDeclarationSyntax IInvokableSymbol.Declaration => Declaration;

        public FunctionSymbol(string name, ImmutableArray<ParameterSymbol> parameters, TypeSymbol returnType, FunctionDeclarationSyntax declaration = null, bool @extern = false)
            : base(name)
        {
            Parameters = parameters;
            ReturnType = returnType;
            Declaration = declaration;
            Extern = @extern;
        }

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Parameters.Select(p => p.ToString()))}): {ReturnType} ";
        }
    }
}