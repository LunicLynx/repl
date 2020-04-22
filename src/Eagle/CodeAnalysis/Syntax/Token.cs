using Eagle.CodeAnalysis.Text;

namespace Eagle.CodeAnalysis.Syntax
{
    public class Token : SyntaxNode
    {
        public TokenKind Kind { get; }
        public int Position { get; }

        private readonly string? _text;
        public string Text => _text ?? "";

        public object? Value { get; }
        public override TextSpan Span => new TextSpan(Position, Text.Length);

        public Token(SyntaxTree syntaxTree, TokenKind kind, int position, string? text, object? value)
            : base(syntaxTree)
        {
            Kind = kind;
            Position = position;
            _text = text;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Kind}: {Text}";
        }

        /// <summary>
        /// A token is missing if it was inserted by the parser and doesn't appear in source.
        /// </summary>
        public bool IsMissing => _text == null;
    }
}