using System.Linq;

namespace Repl.CodeAnalysis
{
    public class FunctionSymbol : Symbol
    {
        public TypeSymbol ReturnType { get; }
        public override string Name { get; }
        public ParameterSymbol[] Parameters { get; }

        public FunctionSymbol(TypeSymbol returnType, string name, ParameterSymbol[] parameters)
        {
            ReturnType = returnType;
            Name = name;
            Parameters = parameters;
        }

        public override string ToString()
        {
            return $"{ReturnType} {Name}({string.Join(", ", Parameters.Select(p => p.ToString()))})";
        }
    }
}