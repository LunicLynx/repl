using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis.Syntax
{
    public class ParserBase
    {
        public SyntaxTree SyntaxTree { get; }
        protected Token[] Tokens;

        private int _position = 0;
        public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

        public ParserBase(SyntaxTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        protected Token NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        protected Token Current => Peek(0);

        protected Token Peek(int offset)
        {
            var position = _position + offset;

            return position >= Tokens.Length
                ? Tokens[Tokens.Length - 1]
                : Tokens[position];
        }

        protected Token MatchToken(TokenKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            Diagnostics.ReportUnexpectedToken(Current.Location, Current.Kind, kind);
            return new Token(SyntaxTree, kind, new TextSpan(Current.Span.Start, 0), null);
        }

        protected int GetPosition()
        {
            return _position;
        }

        protected void ResetPosition(int position)
        {
            _position = position;
        }
    }
}