using Eagle.CodeAnalysis.Text;

namespace Eagle.CodeAnalysis.Syntax
{
    abstract class LexerBase
    {
        public SyntaxTree SyntaxTree { get; }
        public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

        protected int Position = 0;

        protected SourceText Text => SyntaxTree.Text;

        protected LexerBase(SyntaxTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
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