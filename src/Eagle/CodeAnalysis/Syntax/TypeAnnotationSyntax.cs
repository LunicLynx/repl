namespace Eagle.CodeAnalysis.Syntax
{
    public class TypeAnnotationSyntax : SyntaxNode
    {
        public Token ColonToken { get; }
        public SyntaxNode Type { get; }

        public TypeAnnotationSyntax(SyntaxTree syntaxTree, Token colonToken, SyntaxNode type)
            : base(syntaxTree)
        {
            ColonToken = colonToken;
            Type = type;
        }
    }
}