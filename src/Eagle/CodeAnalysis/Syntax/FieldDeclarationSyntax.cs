using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class FieldDeclarationSyntax : MemberDeclarationSyntax
    {
        public InitializerSyntax Initializer { get; }

        public FieldDeclarationSyntax(SyntaxTree syntaxTree, Token identifierToken, TypeAnnotationSyntax typeAnnotation, InitializerSyntax initializer)
            : base(syntaxTree, identifierToken, typeAnnotation)
        {
            Initializer = initializer;
        }
    }
}