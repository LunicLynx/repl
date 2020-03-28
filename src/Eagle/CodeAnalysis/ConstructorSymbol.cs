using System.Linq;

namespace Repl.CodeAnalysis
{
    public class ConstructorSymbol : MemberSymbol
    {
        public ParameterSymbol[] Parameters { get; }

        public ConstructorSymbol(TypeSymbol type, ParameterSymbol[] parameters)
        : base(type.Name, type)
        {
            Type = type;
            Parameters = parameters;
        }
        
        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Parameters.Select(p => p.ToString()))})";
        }
    }
}