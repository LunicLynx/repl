namespace Eagle.CodeAnalysis.Syntax
{
    public abstract class MemberDeclarationSyntax : SyntaxNode
    {
        protected MemberDeclarationSyntax(SyntaxTree syntaxTree)
            : base(syntaxTree)
        {
        }
    }
}