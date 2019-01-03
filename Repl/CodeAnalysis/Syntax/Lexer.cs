using System.Collections.Generic;
using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis.Syntax
{
    internal class Lexer : LexerBase
    {
        private static readonly Dictionary<string, TokenKind> KeywordKinds = new Dictionary<string, TokenKind>
        {
            {"true", TokenKind.TrueKeyword},
            {"false", TokenKind.FalseKeyword},
            {"let", TokenKind.LetKeyword},
            {"var", TokenKind.VarKeyword},
            {"if", TokenKind.IfKeyword},
            {"else", TokenKind.ElseKeyword},
            {"while", TokenKind.WhileKeyword},
            {"loop", TokenKind.LoopKeyword},
            {"for", TokenKind.ForKeyword},
            {"to", TokenKind.ToKeyword},
            {"break", TokenKind.BreakKeyword},
            {"continue", TokenKind.ContinueKeyword},
            {"extern", TokenKind.ExternKeyword},
            {"struct", TokenKind.StructKeyword },
            {"new", TokenKind.NewKeyword },
            {"alias", TokenKind.AliasKeyword },
            {"func", TokenKind.FuncKeyword },
            {"const", TokenKind.ConstKeyword },

            {"void", TokenKind.VoidKeyword},
            {"bool", TokenKind.BoolKeyword},
            {"i8", TokenKind.I8Keyword },
            {"i16", TokenKind.I16Keyword },
            {"i32", TokenKind.I32Keyword },
            {"i64", TokenKind.I64Keyword },
            {"i128", TokenKind.I128Keyword },
            {"u8", TokenKind.U8Keyword },
            {"u16", TokenKind.U16Keyword },
            {"u32", TokenKind.U32Keyword },
            {"u64", TokenKind.U64Keyword },
            {"u128", TokenKind.U128Keyword },
            {"int", TokenKind.IntKeyword},
            {"uint", TokenKind.UintKeyword},
            {"string", TokenKind.StringKeyword },
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
            else if (c == '/' && Current == '/')
            {
                Next();
                while (Current != '\r' && Current != '\n' && Current != '\0')
                    Next();

                // Windows uses \r\n  we want to consume the \n as well 
                if (Current == '\n')
                    Next();
                kind = TokenKind.SingleLineComment;
            }
            else if (c == '/' && Current == '*')
            {
                Next();
                while (Current != '\0')
                {
                    Next();
                    if (Current != '*') continue;
                    Next();
                    if (Current != '/') continue;
                    Next();
                    break;
                }
                kind = TokenKind.MultiLineComment;
            }
            else if (char.IsDigit(c))
            {
                while (char.IsDigit(Current))
                    Next();

                kind = TokenKind.NumberLiteral;
            }
            else if (c == '"')
            {
                while (Current != '"' && Current != '\0')
                    Next();

                if (Current == '"')
                    Next();

                kind = TokenKind.StringLiteral;
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
                    case '[':
                        kind = TokenKind.OpenBracket;
                        break;
                    case ']':
                        kind = TokenKind.CloseBracket;
                        break;
                    case '~':
                        kind = TokenKind.Tilde;
                        break;
                    case '^':
                        kind = TokenKind.Hat;
                        break;
                    case '%':
                        kind = TokenKind.Percent;
                        break;
                    case ',':
                        kind = TokenKind.Comma;
                        break;
                    case '.':
                        kind = TokenKind.Dot;
                        break;
                    case ':':
                        kind = TokenKind.Colon;
                        break;
                    case '=' when Current == '=':
                        Next();
                        kind = TokenKind.EqualsEquals;
                        break;
                    case '=' when Current == '>':
                        Next();
                        kind = TokenKind.EqualsGreater;
                        break;
                    case '=':
                        kind = TokenKind.Equals;
                        break;
                    case '<' when Current == '=':
                        Next();
                        kind = TokenKind.LessEquals;
                        break;
                    case '<':
                        kind = TokenKind.Less;
                        break;
                    case '>' when Current == '=':
                        Next();
                        kind = TokenKind.GreaterEquals;
                        break;
                    case '>':
                        kind = TokenKind.Greater;
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
                    case '&':
                        kind = TokenKind.Ampersand;
                        break;
                    case '|' when Current == '|':
                        Next();
                        kind = TokenKind.PipePipe;
                        break;
                    case '|':
                        kind = TokenKind.Pipe;
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