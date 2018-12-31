using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class TypeAnnotationSyntax : SyntaxNode
    {
        public Token ColonToken { get; }
        public TypeSyntax Type { get; }

        public TypeAnnotationSyntax(Token colonToken, TypeSyntax type)
        {
            ColonToken = colonToken;
            Type = type;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ColonToken;
            yield return Type;
        }
    }
}