namespace Repl.CodeAnalysis
{
    public class ParameterSymbol : LocalVariableSymbol
    {
        public override SymbolKind Kind => SymbolKind.Parameter;
        public int Index { get; }

        public ParameterSymbol(TypeSymbol type, string name, int index)
            : base(name, isReadOnly: true, type)
        {
            Index = index;
        }

        public override string ToString()
        {
            return $"{Name}: {Type}";
        }
    }
}