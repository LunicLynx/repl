using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class PropertyDeclarationSyntax : MemberDeclarationSyntax
    {
        public ExpressionBodySyntax ExpressionBody { get; }

        public PropertyDeclarationSyntax(SyntaxTree syntaxTree, Token identifierToken, TypeAnnotationSyntax typeAnnotation, ExpressionBodySyntax expressionBody) : base(syntaxTree, identifierToken, typeAnnotation)
        {
            ExpressionBody = expressionBody;
        }
    }
}