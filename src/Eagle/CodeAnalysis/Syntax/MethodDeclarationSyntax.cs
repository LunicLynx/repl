using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Syntax
{
    public class MethodDeclarationSyntax : MemberDeclarationSyntax, IInvokableDeclarationSyntax
    {
        public ImmutableArray<Token> Modifiers { get; }
        public Token IdentifierToken { get; }
        public Token OpenParenthesisToken { get; }
        public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        public Token CloseParenthesisToken { get; }
        public TypeClauseSyntax TypeClause { get; }
        public BlockStatementSyntax Body { get; }

        Token IInvokableDeclarationSyntax.IdentifierToken => IdentifierToken;
        TypeClauseSyntax? IInvokableDeclarationSyntax.Type => TypeClause;

        public MethodDeclarationSyntax(SyntaxTree syntaxTree, ImmutableArray<Token> modifiers, Token identifierToken, Token openParenthesisToken, SeparatedSyntaxList<ParameterSyntax> parameters, Token closeParenthesisToken, TypeClauseSyntax typeClause, BlockStatementSyntax body)
            : base(syntaxTree)
        {
            Modifiers = modifiers;
            IdentifierToken = identifierToken;
            OpenParenthesisToken = openParenthesisToken;
            Parameters = parameters;
            CloseParenthesisToken = closeParenthesisToken;
            TypeClause = typeClause;
            Body = body;
        }
    }
}