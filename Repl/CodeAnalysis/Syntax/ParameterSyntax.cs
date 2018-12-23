using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ParameterSyntax : SyntaxNode
    {
        public TypeSyntax Type { get; }
        public Token IdentifierToken { get; }

        public ParameterSyntax(TypeSyntax type, Token identifierToken)
        {
            Type = type;
            IdentifierToken = identifierToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Type;
            yield return IdentifierToken;
        }
    }
}