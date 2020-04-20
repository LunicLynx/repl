using System.Reflection;
using Eagle.CodeAnalysis.Syntax;

namespace Eagle.CodeAnalysis
{
    public class PropertySymbol : MemberSymbol
    {
        public PropertySymbol(TypeSymbol declaringType, string name, TypeSymbol type, MethodSymbol getter, MethodSymbol setter) : base(name, type)
        {
            DeclaringType = declaringType;
            Getter = getter;
            Setter = setter;
        }

        public TypeSymbol DeclaringType { get; }
        public MethodSymbol Getter { get; }
        public MethodSymbol Setter { get; }
        public override SymbolKind Kind => SymbolKind.Property;
    }
}