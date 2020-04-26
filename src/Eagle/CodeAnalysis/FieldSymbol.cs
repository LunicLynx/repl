namespace Eagle.CodeAnalysis
{
    public class FieldSymbol : MemberSymbol
    {
        public TypeSymbol DeclaringType { get; }
        public int Index { get; }

        public FieldSymbol(TypeSymbol declaringType, string name, TypeSymbol type, int index) : base(name, type)
        {
            DeclaringType = declaringType;
            Index = index;
        }

        public override SymbolKind Kind => SymbolKind.Field;
    }
}