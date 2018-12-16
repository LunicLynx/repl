namespace Repl.CodeAnalysis.Syntax
{
    public enum TokenKind
    {
        EndOfFile,
        Bad,
        WhiteSpace,

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
        AmpersandAmpersand,
        PipePipe,
        Ampersand,
        Pipe,
        Tilde,
        Hat,
        Percent,

        OpenParenthesis,
        CloseParenthesis,
        OpenBrace,
        CloseBrace,

        Number,
        Identifier,

        TrueKeyword,
        FalseKeyword,
        LetKeyword,
        VarKeyword,
        IfKeyword,
        ElseKeyword,
        WhileKeyword,
        LoopKeyword,
        ForKeyword,
        BreakKeyword,
        ContinueKeyword,
        ToKeyword
    }
}