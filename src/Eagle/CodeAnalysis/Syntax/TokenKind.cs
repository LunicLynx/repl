namespace Repl.CodeAnalysis.Syntax
{
    public enum TokenKind
    {
        // Other
        EndOfFile,
        Bad,
        Whitespace,
        SingleLineComment,
        MultiLineComment,

        // Operators
        Plus,
        Star,
        Equals,
        LessEquals,
        Less,
        EqualsGreater,
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
        Colon,
        Dot,
        Comma,
        OpenParenthesis,
        CloseParenthesis,
        OpenBrace,
        CloseBrace,
        OpenBracket,
        CloseBracket,

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
        ObjectKeyword,
        NewKeyword,
        AliasKeyword,
        ConstKeyword,
        ThisKeyword,
        ReturnKeyword,

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
        UIntKeyword,
        StringKeyword,

        // Literals
        TrueKeyword,
        FalseKeyword,
        NumberLiteral,
        StringLiteral,

        Identifier
    }
}