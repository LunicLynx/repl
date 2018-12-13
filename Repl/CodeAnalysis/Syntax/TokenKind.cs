namespace Repl.CodeAnalysis.Syntax
{
    public enum TokenKind
    {
        Eof,
        Bad,

        Plus,
        Star,
        Equals,

        EqualsEquals,
        BangEquals,
        Bang,
        Minus,
        Slash,
        OpenParenthesis,
        CloseParenthesis,


        Number,
        WhiteSpace,
        Identifier,

        AmpersandAmpersand,
        PipePipe,
        TrueKeyword,
        FalseKeyword
    }
}