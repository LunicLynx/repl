using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Syntax
{
    public class MethodDeclarationSyntax : MemberDeclarationSyntax, IInvokableDeclarationSyntax
    {
        public Token IdentifierToken { get; }
        public Token OpenParenthesisToken { get; }
        public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        public Token CloseParenthesisToken { get; }
        public TypeClauseSyntax TypeClause { get; }
        public BlockStatementSyntax Body { get; }
        public ImmutableArray<Token> Modifiers { get; }

        Token IInvokableDeclarationSyntax.IdentifierToken => IdentifierToken;
        TypeClauseSyntax? IInvokableDeclarationSyntax.Type => TypeClause;

        public MethodDeclarationSyntax(SyntaxTree syntaxTree, Token identifierToken, Token openParenthesisToken, SeparatedSyntaxList<ParameterSyntax> parameters, Token closeParenthesisToken, TypeClauseSyntax typeClause, BlockStatementSyntax body, ImmutableArray<Token> modifiers)
            : base(syntaxTree)
        {
            IdentifierToken = identifierToken;
            OpenParenthesisToken = openParenthesisToken;
            Parameters = parameters;
            CloseParenthesisToken = closeParenthesisToken;
            TypeClause = typeClause;
            Body = body;
            Modifiers = modifiers;
        }
    }
}