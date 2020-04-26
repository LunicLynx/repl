namespace Eagle.CodeAnalysis.Syntax
{
    public class ReferenceTypeSyntax : SyntaxNode
    {
        public SyntaxNode Type { get; }
        public Token AmpersandToken { get; }

        public ReferenceTypeSyntax(SyntaxTree syntaxTree, SyntaxNode type, Token ampersandToken)
            : base(syntaxTree)
        {
            Type = type;
            AmpersandToken = ampersandToken;
        }
    }
}