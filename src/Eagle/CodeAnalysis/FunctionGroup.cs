namespace Eagle.CodeAnalysis
{
    public class FunctionGroup : Symbol
    {
        public override SymbolKind Kind => SymbolKind.FunctionGroup;

        public FunctionGroup(string name) : base(name)
        {
        }
    }
}