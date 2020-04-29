using System.Collections.Immutable;
using System.Linq;
using Eagle.CodeAnalysis.Syntax;

namespace Eagle.CodeAnalysis
{
    public class MethodSymbol : MemberSymbol, IInvokableSymbol
    {
        public TypeSymbol DeclaringType { get; }
        public ImmutableArray<ParameterSymbol> Parameters { get; }
        public bool IsStatic { get; }
        public bool Extern { get; }
        public override SymbolKind Kind => SymbolKind.Method;
        TypeSymbol IInvokableSymbol.Type => Type;

        public MethodSymbol(TypeSymbol declaringType, TypeSymbol returnType, string name, ImmutableArray<ParameterSymbol> parameters, bool isStatic = false)
            : base(name, returnType)
        {
            DeclaringType = declaringType;
            Parameters = parameters;
            IsStatic = isStatic;
        }

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Parameters.Select(p => p.ToString()))}): {Type} ";
        }
    }
}