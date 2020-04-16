namespace Eagle.CodeAnalysis.Syntax
{
    public interface IInvokableDeclarationSyntax
    {
        Token IdentifierToken { get; }
        SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        TypeClauseSyntax? Type { get; }
        BlockStatementSyntax Body { get; }
    }
}