using System.Collections.Generic;
using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis.Syntax
{
    abstract class LexerBase
    {
        public List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();

        protected int Position = 0;

        protected readonly SourceText Text;

        protected LexerBase(SourceText text)
        {
            Text = text;
        }

        protected char Current
            => Position >= Text.Length
                ? '\0'
                : Text[Position];

        protected void Next()
        {
            Position++;
        }

        public abstract Token Lex();
    }
}