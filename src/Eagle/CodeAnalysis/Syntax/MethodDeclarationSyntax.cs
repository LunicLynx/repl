using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Syntax
{
    public class MethodDeclarationSyntax : MemberDeclarationSyntax, IInvokableDeclarationSyntax
    {
        public Token OpenParenthesisToken { get; }
        public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        public Token CloseParenthesisToken { get; }
        public BlockStatementSyntax Body { get; }

        Token IInvokableDeclarationSyntax.IdentifierToken => IdentifierToken;
        TypeAnnotationSyntax? IInvokableDeclarationSyntax.Type => TypeAnnotation;

        public MethodDeclarationSyntax(SyntaxTree syntaxTree, Token identifierToken, Token openParenthesisToken, SeparatedSyntaxList<ParameterSyntax> parameters, Token closeParenthesisToken, TypeAnnotationSyntax? type, BlockStatementSyntax body)
        : base(syntaxTree, identifierToken, type)
        {
            OpenParenthesisToken = openParenthesisToken;
            Parameters = parameters;
            CloseParenthesisToken = closeParenthesisToken;
            Body = body;
        }
    }
}