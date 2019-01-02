using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ConstDeclarationSyntax : SyntaxNode
    {
        public Token ConstKeyword { get; }
        public Token IdentifierToken { get; }
        public TypeAnnotationSyntax TypeAnnotation { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax Initializer { get; }

        public ConstDeclarationSyntax(Token constKeyword, Token identifierToken, TypeAnnotationSyntax typeAnnotation, Token equalsToken, ExpressionSyntax initializer)
        {
            ConstKeyword = constKeyword;
            IdentifierToken = identifierToken;
            TypeAnnotation = typeAnnotation;
            EqualsToken = equalsToken;
            Initializer = initializer;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ConstKeyword;
            yield return IdentifierToken;
            if (TypeAnnotation != null)
                yield return TypeAnnotation;
            yield return EqualsToken;
            yield return Initializer;
        }
    }
}