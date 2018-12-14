using System.Collections.Generic;
using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis.Syntax
{
    class Lexer : LexerBase
    {
        private static readonly Dictionary<string, TokenKind> KeywordKinds = new Dictionary<string, TokenKind>
        {
            {"true", TokenKind.TrueKeyword},
            {"false", TokenKind.FalseKeyword}
        };

        public Lexer(SourceText text) : base(text) { }

        public override Token Lex()
        {
            var start = Position;
            var kind = TokenKind.EndOfFile;

            var c = Current;
            Next();

            if (char.IsWhiteSpace(c))
            {
                while (char.IsWhiteSpace(Current))
                    Next();

                kind = TokenKind.WhiteSpace;
            }
            else if (char.IsDigit(c))
            {
                while (char.IsDigit(Current))
                    Next();

                kind = TokenKind.Number;
            }
            else if (IsIdentifierStart(c))
            {
                while (IsIdentifierFollow(Current))
                    Next();

                var text = Text.ToString(start, Position - start);

                kind = KeywordKinds.TryGetValue(text, out var k)
                    ? k
                    : TokenKind.Identifier;
            }
            else
            {
                switch (c)
                {
                    case '\0':
                        start = Text.Length;
                        Position = Text.Length;
                        break;
                    case '+':
                        kind = TokenKind.Plus;
                        break;
                    case '-':
                        kind = TokenKind.Minus;
                        break;
                    case '*':
                        kind = TokenKind.Star;
                        break;
                    case '/':
                        kind = TokenKind.Slash;
                        break;
                    case '(':
                        kind = TokenKind.OpenParenthesis;
                        break;
                    case ')':
                        kind = TokenKind.CloseParenthesis;
                        break;
                    case '{':
                        kind = TokenKind.OpenBrace;
                        break;
                    case '}':
                        kind = TokenKind.CloseBrace;
                        break;
                    case '=' when Current == '=':
                        Next();
                        kind = TokenKind.EqualsEquals;
                        break;
                    case '=':
                        kind = TokenKind.Equals;
                        break;
                    case '!' when Current == '=':
                        Next();
                        kind = TokenKind.BangEquals;
                        break;
                    case '!':
                        kind = TokenKind.Bang;
                        break;
                    case '&' when Current == '&':
                        Next();
                        kind = TokenKind.AmpersandAmpersand;
                        break;
                    case '|' when Current == '|':
                        Next();
                        kind = TokenKind.PipePipe;
                        break;
                    default:
                        kind = TokenKind.Bad;
                        Diagnostics.ReportUnexpectedCharacter(TextSpan.FromBounds(start, Position), c);
                        break;
                }
            }

            return new Token(kind, TextSpan.FromBounds(start, Position), Text.ToString(start, Position - start));
        }

        private bool IsIdentifierStart(char c)
        {
            return char.IsLetter(c) || c == '_';
        }

        private bool IsIdentifierFollow(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }
    }
}