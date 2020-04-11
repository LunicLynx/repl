using System.Collections.Immutable;
using System.Linq;
using Repl.CodeAnalysis.Syntax;

namespace Repl.CodeAnalysis
{
    public class MethodSymbol : MemberSymbol, IInvokableSymbol
    {
        public ImmutableArray<ParameterSymbol> Parameters { get; }
        public MethodDeclarationSyntax? Declaration { get; }

        public MethodSymbol(TypeSymbol returnType, string name, ImmutableArray<ParameterSymbol> parameters, MethodDeclarationSyntax? declaration = null)
            : base(name, returnType)
        {
            Parameters = parameters;
            Declaration = declaration;
        }

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Parameters.Select(p => p.ToString()))}): {Type} ";
        }

        public override SymbolKind Kind => SymbolKind.Method;

        IInvokableDeclarationSyntax IInvokableSymbol.Declaration => Declaration;

        TypeSymbol IInvokableSymbol.ReturnType => Type;
    }
}