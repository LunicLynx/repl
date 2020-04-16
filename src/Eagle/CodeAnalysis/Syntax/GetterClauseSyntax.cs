namespace Eagle.CodeAnalysis.Syntax
{
    public class GetterClauseSyntax : SyntaxNode
    {
        public Token GetKeyword { get; }
        public SyntaxNode Body { get; }

        public GetterClauseSyntax(SyntaxTree syntaxTree, Token getKeyword, SyntaxNode body) : base(syntaxTree)
        {
            GetKeyword = getKeyword;
            Body = body;
        }
    }
}