﻿namespace Repl.CodeAnalysis.Syntax
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
                    return 6;

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
                case TokenKind.EqualsEquals:
                case TokenKind.BangEquals:
                    return 3;
                case TokenKind.Plus:
                case TokenKind.Minus:
                    return 4;
                case TokenKind.Star:
                case TokenKind.Slash:
                    return 5;
                default: return 0;
            }
        }
    }
}