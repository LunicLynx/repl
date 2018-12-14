namespace Repl.CodeAnalysis.Syntax
{
    public enum TokenKind
    {
        EndOfFile,
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
        VarKeyword
    }
}