namespace Eagle.CodeAnalysis.Syntax
{
    public class SetterClauseSyntax : SyntaxNode
    {
        public Token SetKeyword { get; }
        public SyntaxNode Body { get; }

        public SetterClauseSyntax(SyntaxTree syntaxTree, Token setKeyword, SyntaxNode body) : base(syntaxTree)
        {
            SetKeyword = setKeyword;
            Body = body;
        }
    }
}