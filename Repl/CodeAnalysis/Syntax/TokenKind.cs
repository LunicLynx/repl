namespace Repl.CodeAnalysis.Syntax
{
    public enum TokenKind
    {
        // Other
        EndOfFile,
        Bad,
        WhiteSpace,

        // Operators
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

        // Structure
        Comma,
        OpenParenthesis,
        CloseParenthesis,
        OpenBrace,
        CloseBrace,

        // Keywords
        LetKeyword,
        VarKeyword,
        IfKeyword,
        ElseKeyword,
        WhileKeyword,
        LoopKeyword,
        ForKeyword,
        BreakKeyword,
        ContinueKeyword,
        ToKeyword,
        ExternKeyword,

        // Types
        VoidKeyword,
        IntKeyword,

        // Literals
        TrueKeyword,
        FalseKeyword,
        Number,

        Identifier,
    }
}