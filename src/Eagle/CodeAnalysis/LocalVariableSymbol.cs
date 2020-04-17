namespace Eagle.CodeAnalysis
{
    public class LocalVariableSymbol : VariableSymbol
    {
        public LocalVariableSymbol(string name, bool isReadOnly, TypeSymbol type)
            : base(name, isReadOnly, type)
        {
        }
    }
}