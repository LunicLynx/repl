using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis.Syntax
{
    public class Token : SyntaxNode
    {
        public TokenKind Kind { get; }
        public TextSpan Span { get; }
        public string Text { get; }

        public Token(TokenKind kind, TextSpan span, string text)
        {
            Kind = kind;
            Span = span;
            Text = text;
        }

        public override string ToString()
        {
            return $"{Kind}: {Text}";
        }
    }
}