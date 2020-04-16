namespace Eagle.CodeAnalysis.Syntax
{
    public class TypeClauseSyntax : SyntaxNode
    {
        public Token ColonToken { get; }
        public SyntaxNode Type { get; }

        public TypeClauseSyntax(SyntaxTree syntaxTree, Token colonToken, SyntaxNode type)
            : base(syntaxTree)
        {
            ColonToken = colonToken;
            Type = type;
        }
    }
}