using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class FieldDeclarationSyntax : MemberDeclarationSyntax
    {
        public InitializerSyntax Initializer { get; }

        public FieldDeclarationSyntax(Token identifierToken, TypeAnnotationSyntax typeAnnotation, InitializerSyntax initializer)
            : base(identifierToken, typeAnnotation)
        {
            Initializer = initializer;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            if (TypeAnnotation != null)
                yield return TypeAnnotation;
            if (Initializer != null)
                yield return Initializer;
        }
    }
}