using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class TypeSyntax : SyntaxNode
    {
        public Token TypeOrIdentifierToken { get; }

        public TypeSyntax(Token typeOrIdentifierToken)
        {
            TypeOrIdentifierToken = typeOrIdentifierToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return TypeOrIdentifierToken;
        }
    }
}