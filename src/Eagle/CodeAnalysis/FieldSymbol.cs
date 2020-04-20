namespace Eagle.CodeAnalysis
{
    public class FieldSymbol : MemberSymbol
    {
        public TypeSymbol DeclaringType { get; }

        public FieldSymbol(TypeSymbol declaringType, string name, TypeSymbol type) : base(name, type)
        {
            DeclaringType = declaringType;
        }

        public override SymbolKind Kind => SymbolKind.Field;
    }
}