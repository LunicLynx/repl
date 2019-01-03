using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class PropertyDeclarationSyntax : MemberDeclarationSyntax
    {
        public PropertyDeclarationSyntax(Token identifierToken, TypeAnnotationSyntax typeAnnotation) : base(identifierToken, typeAnnotation)
        {
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            if (TypeAnnotation != null)
                yield return TypeAnnotation;
        }
    }
}