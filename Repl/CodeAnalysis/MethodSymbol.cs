using System.Linq;

namespace Repl.CodeAnalysis
{
    public class MethodSymbol : MemberSymbol
    {
        public ParameterSymbol[] Parameters { get; }

        public MethodSymbol(TypeSymbol returnType, string name, ParameterSymbol[] parameters) : base(name, returnType)
        {
            Parameters = parameters;
        }

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Parameters.Select(p => p.ToString()))}): {Type} ";
        }
    }
}