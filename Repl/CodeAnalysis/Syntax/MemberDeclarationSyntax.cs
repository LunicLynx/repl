using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class MemberDeclarationSyntax : SyntaxNode
    {
        public TypeAnnotationSyntax TypeAnnotation { get; }
        public ExpressionSyntax Initializer { get; }
        public Token IdentifierToken { get; }

        public MemberDeclarationSyntax(Token identifierToken, TypeAnnotationSyntax typeAnnotation, ExpressionSyntax initializer)
        {
            IdentifierToken = identifierToken;
            TypeAnnotation = typeAnnotation;
            Initializer = initializer;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return TypeAnnotation;
            yield return IdentifierToken;
        }
    }
}