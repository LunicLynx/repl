namespace Repl.CodeAnalysis.Syntax
{
    public enum TokenKind
    {
        // Other
        EndOfFile,
        Bad,
        WhiteSpace,
        SingleLineComment,

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
        BoolKeyword,
        I8Keyword,
        I16Keyword,
        I32Keyword,
        I64Keyword,
        I128Keyword,
        U8Keyword,
        U16Keyword,
        U32Keyword,
        U64Keyword,
        U128Keyword,
        IntKeyword,
        UintKeyword,
        StringKeyword,

        // Literals
        TrueKeyword,
        FalseKeyword,
        Number,
        String,

        Identifier,
    }
}