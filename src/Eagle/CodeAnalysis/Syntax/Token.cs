using Eagle.CodeAnalysis.Text;

namespace Eagle.CodeAnalysis.Syntax
{
    public class Token : SyntaxNode
    {
        public TokenKind Kind { get; }
        public int Position { get; }
        public string? Text { get; }
        public object? Value { get; }
        public override TextSpan Span => new TextSpan(Position, Text?.Length ?? 0);

        public Token(SyntaxTree syntaxTree, TokenKind kind, int position, string? text, object? value)
            : base(syntaxTree)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
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