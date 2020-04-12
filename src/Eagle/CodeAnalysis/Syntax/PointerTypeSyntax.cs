namespace Eagle.CodeAnalysis.Syntax
{
    public class PointerTypeSyntax : SyntaxNode
    {
        public SyntaxNode Type { get; }
        public Token AsteriskToken { get; }

        public PointerTypeSyntax(SyntaxTree syntaxTree, SyntaxNode type, Token asteriskToken)
            : base(syntaxTree)
        {
            Type = type;
            AsteriskToken = asteriskToken;
        }
    }
}