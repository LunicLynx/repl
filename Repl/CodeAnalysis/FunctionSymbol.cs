namespace Repl.CodeAnalysis
{
    public class FunctionSymbol : Symbol
    {
        public TypeSymbol ReturnType { get; }
        public override string Name { get; }

        public FunctionSymbol(TypeSymbol returnType, string name)
        {
            ReturnType = returnType;
            Name = name;
        }
    }
}