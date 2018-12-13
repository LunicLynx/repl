using System.Collections.Generic;
using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis.Syntax
{
    public class ParserBase
    {
        protected Token[] Tokens;

        private int _position = 0;
        public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

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

            return new Token(kind, new TextSpan(0, 0), "");
        }
    }
}