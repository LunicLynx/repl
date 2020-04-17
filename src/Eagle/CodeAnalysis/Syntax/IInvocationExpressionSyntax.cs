namespace Eagle.CodeAnalysis.Syntax
{
    interface IInvocationExpressionSyntax
    {
        SyntaxTree SyntaxTree { get; }
        SeparatedSyntaxList<ExpressionSyntax> Arguments { get; }
        Token CloseToken { get; }
    }
}