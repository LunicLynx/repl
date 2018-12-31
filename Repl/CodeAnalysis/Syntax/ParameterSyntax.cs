using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ParameterSyntax : SyntaxNode
    {
        public Token IdentifierToken { get; }
        public TypeAnnotationSyntax Type { get; }

        public ParameterSyntax(Token identifierToken, TypeAnnotationSyntax type)
        {
            IdentifierToken = identifierToken;
            Type = type;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return Type;
        }
    }
}