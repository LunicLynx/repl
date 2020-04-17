using System.Collections.Immutable;
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
}