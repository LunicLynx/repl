using System.Reflection;
using Eagle.CodeAnalysis.Syntax;

namespace Eagle.CodeAnalysis
{
    public class PropertySymbol : MemberSymbol
    {
        public PropertySymbol(string name, TypeSymbol type, MethodSymbol getter, MethodSymbol setter, PropertyDeclarationSyntax syntax) : base(name, type)
        {
            Getter = getter;
            Setter = setter;
            Syntax = syntax;
        }

        public MethodSymbol Getter { get; }
        public MethodSymbol Setter { get; }
        public PropertyDeclarationSyntax Syntax { get; }
        public override SymbolKind Kind => SymbolKind.Property;
    }
}