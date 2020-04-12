namespace Eagle.CodeAnalysis
{
    public class MethodGroup : MemberSymbol
    {
        public MethodGroup(string name, TypeSymbol type) : base(name, type)
        {
        }

        public override SymbolKind Kind => SymbolKind.MethodGroup;
    }
}