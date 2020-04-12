namespace Eagle.CodeAnalysis
{
    public class FieldSymbol : MemberSymbol
    {
        public FieldSymbol(string name, TypeSymbol type) : base(name, type)
        {
        }

        public override SymbolKind Kind => SymbolKind.Field;
    }
}