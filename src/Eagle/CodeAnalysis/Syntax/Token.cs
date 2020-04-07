using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis.Syntax
{
    public class Token : SyntaxNode
    {
        public TokenKind Kind { get; }
        public override TextSpan Span { get; }
        public string? Text { get; }

        public Token(SyntaxTree syntaxTree, TokenKind kind, TextSpan span, string? text) : base(syntaxTree)
        {
            Kind = kind;
            Span = span;
            Text = text;
        }

        public override string ToString()
        {
            return $"{Kind}: {Text}";
        }

        /// <summary>
        /// A token is missing if it was inserted by the parser and doesn't appear in source.
        /// </summary>
        public bool IsMissing => Text == null;
    }
}