using System.Reflection;

namespace Eagle.CodeAnalysis
{
    public class PropertySymbol : MemberSymbol
    {
        public PropertySymbol(string name, TypeSymbol type, MethodSymbol getter, MethodSymbol setter) : base(name, type)
        {
            Getter = getter;
            Setter = setter;
        }

        public MethodSymbol Getter { get; }
        public MethodSymbol Setter { get; }
        public override SymbolKind Kind => SymbolKind.Property;
    }
}