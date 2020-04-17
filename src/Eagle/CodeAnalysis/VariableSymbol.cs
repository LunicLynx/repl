namespace Eagle.CodeAnalysis
{
    public abstract class VariableSymbol : Symbol
    {
        public override SymbolKind Kind => SymbolKind.Variable;
        public bool IsReadOnly { get; }
        public TypeSymbol Type { get; }

        protected VariableSymbol(string name, bool isReadOnly, TypeSymbol type)
            : base(name)
        {
            IsReadOnly = isReadOnly;
            Type = type;
        }

        public override string ToString() => Name;
    }
}