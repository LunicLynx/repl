namespace Repl.CodeAnalysis.Syntax
{
    class SyntaxFacts
    {
        public static int GetUnaryOperatorPrecedence(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Plus:
                case TokenKind.Minus:
                case TokenKind.Bang:
                case TokenKind.Tilde:
                    return 10;

                default: return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.PipePipe:
                    return 1;
                case TokenKind.AmpersandAmpersand:
                    return 2;
                case TokenKind.Pipe:
                    return 3;
                case TokenKind.Hat:
                    return 4;
                case TokenKind.Ampersand:
                    return 5;
                case TokenKind.EqualsEquals:
                case TokenKind.BangEquals:
                    return 6;
                case TokenKind.Less:
                case TokenKind.LessEquals:
                case TokenKind.Greater:
                case TokenKind.GreaterEquals:
                    return 7;
                case TokenKind.Plus:
                case TokenKind.Minus:
                    return 8;
                case TokenKind.Star:
                case TokenKind.Slash:
                case TokenKind.Percent:
                    return 9;
                default: return 0;
            }
        }
    }
}