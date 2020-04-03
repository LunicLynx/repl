using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class PropertyDeclarationSyntax : MemberDeclarationSyntax
    {
        public ExpressionBodySyntax ExpressionBody { get; }
        //public PropertyDeclarationSyntax(Token identifierToken, TypeAnnotationSyntax typeAnnotation) : base(identifierToken, typeAnnotation)
        //{
        //}

        public PropertyDeclarationSyntax(SyntaxTree syntaxTree, Token identifierToken, TypeAnnotationSyntax typeAnnotation, ExpressionBodySyntax expressionBody) : base(syntaxTree, identifierToken, typeAnnotation)
        {
            ExpressionBody = expressionBody;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            if (TypeAnnotation != null)
                yield return TypeAnnotation;
        }
    }
}