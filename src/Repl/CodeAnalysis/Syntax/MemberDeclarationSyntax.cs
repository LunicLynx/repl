using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public abstract class MemberDeclarationSyntax : SyntaxNode
    {
        public TypeAnnotationSyntax TypeAnnotation { get; }

        public Token IdentifierToken { get; }

        protected MemberDeclarationSyntax(Token identifierToken, TypeAnnotationSyntax typeAnnotation)
        {
            IdentifierToken = identifierToken;
            TypeAnnotation = typeAnnotation;
        }

        public abstract override IEnumerable<SyntaxNode> GetChildren();
    }
}