namespace Repl.CodeAnalysis.Syntax
{
    public enum TokenKind
    {
        EndOfFile,
        Bad,

        Plus,
        Star,
        Equals,

        LessEquals,
        Less,
        GreaterEquals,
        Greater,
        EqualsEquals,
        BangEquals,
        Bang,
        Minus,
        Slash,
        OpenParenthesis,
        CloseParenthesis,
        OpenBrace,
        CloseBrace,

        Number,
        WhiteSpace,
        Identifier,

        AmpersandAmpersand,
        PipePipe,
        TrueKeyword,
        FalseKeyword,
        LetKeyword,
        VarKeyword,
    }
}