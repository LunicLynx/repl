using System.Collections.Generic;
using System.Collections.Immutable;
using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis.Syntax
{
    public class SyntaxTree
    {
        public SourceText Text { get; }
        public CompilationUnitSyntax Root { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }

        private SyntaxTree(SourceText text)
        {
            var parser = new Parser(text);
            var root = parser.ParseCompilationUnit();
            var diagnostics = parser.Diagnostics.ToImmutableArray();

            Diagnostics = diagnostics;
            Text = text;
            Root = root;
        }

        public static SyntaxTree Parse(string text)
        {
            var sourceText = SourceText.From(text);
            return Parse(sourceText);
        }

        public static SyntaxTree Parse(SourceText text)
        {
            return new SyntaxTree(text);
        }

        public static IEnumerable<Token> ParseTokens(string text)
        {
            var sourceText = SourceText.From(text);
            return ParseTokens(sourceText);
        }

        public static IEnumerable<Token> ParseTokens(SourceText text)
        {
            var lexer = new Lexer(text);
            while (true)
            {
                var token = lexer.Lex();
                if (token.Kind == TokenKind.EndOfFile)
                    break;

                yield return token;
            }
        }
    }
}