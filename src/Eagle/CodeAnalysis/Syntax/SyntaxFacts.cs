using System;
using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Syntax
{
    public class SyntaxFacts
    {
        public static bool IsTypeKeyword(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.VoidKeyword:
                case TokenKind.BoolKeyword:
                case TokenKind.I8Keyword:
                case TokenKind.I16Keyword:
                case TokenKind.I32Keyword:
                case TokenKind.I64Keyword:
                case TokenKind.I128Keyword:
                case TokenKind.U8Keyword:
                case TokenKind.U16Keyword:
                case TokenKind.U32Keyword:
                case TokenKind.U64Keyword:
                case TokenKind.U128Keyword:
                case TokenKind.IntKeyword:
                case TokenKind.UIntKeyword:
                case TokenKind.StringKeyword:
                case TokenKind.CharKeyword:
                    return true;
                default:
                    return false;
            }
        }

        public static int GetUnaryOperatorPrecedence(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Plus:
                case TokenKind.Minus:
                case TokenKind.Bang:
                case TokenKind.Tilde:
                case TokenKind.Ampersand:
                    return 10;

                default: return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Star:
                case TokenKind.Slash:
                case TokenKind.Percent:
                    return 9;
                case TokenKind.Plus:
                case TokenKind.Minus:
                    return 8;
                case TokenKind.Less:
                case TokenKind.LessEquals:
                case TokenKind.Greater:
                case TokenKind.GreaterEquals:
                    return 7;
                case TokenKind.EqualsEquals:
                case TokenKind.BangEquals:
                    return 6;
                case TokenKind.Ampersand:
                    return 5;
                case TokenKind.Hat:
                    return 4;
                case TokenKind.Pipe:
                    return 3;
                case TokenKind.AmpersandAmpersand:
                    return 2;
                case TokenKind.PipePipe:
                    return 1;
                default:
                    return 0;
            }
        }

        public static TokenKind GetKeywordKind(string text)
        {
            switch (text)
            {
                case "break":
                    return TokenKind.BreakKeyword;
                case "continue":
                    return TokenKind.ContinueKeyword;
                case "else":
                    return TokenKind.ElseKeyword;
                case "false":
                    return TokenKind.FalseKeyword;
                case "for":
                    return TokenKind.ForKeyword;
                case "if":
                    return TokenKind.IfKeyword;
                case "let":
                    return TokenKind.LetKeyword;
                case "return":
                    return TokenKind.ReturnKeyword;
                case "to":
                    return TokenKind.ToKeyword;
                case "true":
                    return TokenKind.TrueKeyword;
                case "var":
                    return TokenKind.VarKeyword;
                case "while":
                    return TokenKind.WhileKeyword;
                default:
                    return TokenKind.Identifier;
            }
        }

        public static IEnumerable<TokenKind> GetUnaryOperatorKinds()
        {
            var kinds = (TokenKind[])Enum.GetValues(typeof(TokenKind));
            foreach (var kind in kinds)
            {
                if (GetUnaryOperatorPrecedence(kind) > 0)
                    yield return kind;
            }
        }

        public static IEnumerable<TokenKind> GetBinaryOperatorKinds()
        {
            var kinds = (TokenKind[])Enum.GetValues(typeof(TokenKind));
            foreach (var kind in kinds)
            {
                if (GetBinaryOperatorPrecedence(kind) > 0)
                    yield return kind;
            }
        }

        public static string GetText(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Plus:
                    return "+";
                case TokenKind.Minus:
                    return "-";
                case TokenKind.Star:
                    return "*";
                case TokenKind.Slash:
                    return "/";
                case TokenKind.Bang:
                    return "!";
                case TokenKind.Equals:
                    return "=";
                case TokenKind.Tilde:
                    return "~";
                case TokenKind.Less:
                    return "<";
                case TokenKind.LessEquals:
                    return "<=";
                case TokenKind.Greater:
                    return ">";
                case TokenKind.GreaterEquals:
                    return ">=";
                case TokenKind.Ampersand:
                    return "&";
                case TokenKind.AmpersandAmpersand:
                    return "&&";
                case TokenKind.Pipe:
                    return "|";
                case TokenKind.PipePipe:
                    return "||";
                case TokenKind.Hat:
                    return "^";
                case TokenKind.EqualsEquals:
                    return "==";
                case TokenKind.BangEquals:
                    return "!=";
                case TokenKind.OpenParenthesis:
                    return "(";
                case TokenKind.CloseParenthesis:
                    return ")";
                case TokenKind.OpenBrace:
                    return "{";
                case TokenKind.CloseBrace:
                    return "}";
                case TokenKind.Colon:
                    return ":";
                case TokenKind.Comma:
                    return ",";
                case TokenKind.BreakKeyword:
                    return "break";
                case TokenKind.ContinueKeyword:
                    return "continue";
                case TokenKind.ElseKeyword:
                    return "else";
                case TokenKind.FalseKeyword:
                    return "false";
                case TokenKind.ForKeyword:
                    return "for";
                case TokenKind.IfKeyword:
                    return "if";
                case TokenKind.LetKeyword:
                    return "let";
                case TokenKind.ReturnKeyword:
                    return "return";
                case TokenKind.ToKeyword:
                    return "to";
                case TokenKind.TrueKeyword:
                    return "true";
                case TokenKind.VarKeyword:
                    return "var";
                case TokenKind.WhileKeyword:
                    return "while";
                case TokenKind.EqualsGreater:
                    return "=>";
                case TokenKind.SingleLineComment:
                    return "// comment";
                case TokenKind.MultiLineComment:
                    return "/* comment */";
                case TokenKind.Percent:
                    return "%";
                case TokenKind.Dot:
                    return ".";
                case TokenKind.OpenBracket:
                    return "[";
                case TokenKind.CloseBracket:
                    return "]";
                case TokenKind.LoopKeyword:
                    return "loop";
                case TokenKind.ExternKeyword:
                    return "extern";
                case TokenKind.ObjectKeyword:
                    return "object";
                case TokenKind.NewKeyword:
                    return "new";
                case TokenKind.AliasKeyword:
                    return "alias";
                case TokenKind.ConstKeyword:
                    return "const";
                case TokenKind.ThisKeyword:
                    return "this";
                case TokenKind.VoidKeyword:
                    return "void";
                case TokenKind.BoolKeyword:
                    return "bool";
                case TokenKind.I8Keyword:
                    return "i8";
                case TokenKind.I16Keyword:
                    return "i16";
                case TokenKind.I32Keyword:
                    return "i32";
                case TokenKind.I64Keyword:
                    return "i64";
                case TokenKind.I128Keyword:
                    return "i128";
                case TokenKind.U8Keyword:
                    return "u8";
                case TokenKind.U16Keyword:
                    return "u16";
                case TokenKind.U32Keyword:
                    return "u32";
                case TokenKind.U64Keyword:
                    return "u64";
                case TokenKind.U128Keyword:
                    return "u128";
                case TokenKind.IntKeyword:
                    return "int";
                case TokenKind.UIntKeyword:
                    return "uint";
                case TokenKind.StringKeyword:
                    return "string";
                case TokenKind.CharKeyword:
                    return "char";
                default:
                    return null;
            }
        }
    }
}