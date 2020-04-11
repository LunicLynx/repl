using System;
using System.Collections.Generic;
using System.Linq;
using Repl.CodeAnalysis.Syntax;
using Repl.CodeAnalysis.Text;
using Xunit;

namespace Repl.Tests
{
    public class LexerTests
    {
        [Fact]
        public void Lexer_Lexes_UnterminatedString()
        {
            var text = "\"text";
            var tokens = SyntaxTree.ParseTokens(text, out var diagnostics);

            var token = Assert.Single(tokens);
            Assert.Equal(TokenKind.StringLiteral, token.Kind);
            Assert.Equal(text, token.Text);

            var diagnostic = Assert.Single(diagnostics);
            Assert.Equal(new TextSpan(0, 1), diagnostic.Location.Span);
            Assert.Equal("Unterminated string literal.", diagnostic.Message);
        }

        [Fact]
        public void Lexer_Covers_AllTokens()
        {
            var tokenKinds = Enum.GetValues(typeof(TokenKind))
                .Cast<TokenKind>()
                .Where(k => k.ToString().EndsWith("Keyword") ||
                            k.ToString().EndsWith("Token"));

            var testedTokenKinds = GetTokens().Concat(GetSeparators()).Select(t => t.kind);

            var untestedTokenKinds = new SortedSet<TokenKind>(tokenKinds);
            untestedTokenKinds.Remove(TokenKind.Bad);
            untestedTokenKinds.Remove(TokenKind.EndOfFile);
            untestedTokenKinds.ExceptWith(testedTokenKinds);

            Assert.Empty(untestedTokenKinds);
        }

        [Theory]
        [MemberData(nameof(GetTokensData))]
        public void Lexer_Lexes_Token(TokenKind kind, string text)
        {
            var tokens = SyntaxTree.ParseTokens(text);

            var token = Assert.Single(tokens);
            Assert.Equal(kind, token.Kind);
            Assert.Equal(text, token.Text);
        }

        [Theory]
        [MemberData(nameof(GetTokenPairsData))]
        public void Lexer_Lexes_TokenPairs(TokenKind t1Kind, string t1Text,
            TokenKind t2Kind, string t2Text)
        {
            var text = t1Text + t2Text;
            var tokens = SyntaxTree.ParseTokens(text).ToArray();

            Assert.Equal(2, tokens.Length);
            Assert.Equal(t1Kind, tokens[0].Kind);
            Assert.Equal(t1Text, tokens[0].Text);
            Assert.Equal(t2Kind, tokens[1].Kind);
            Assert.Equal(t2Text, tokens[1].Text);
        }

        [Theory]
        [MemberData(nameof(GetTokenPairsWithSeparatorData))]
        public void Lexer_Lexes_TokenPairs_WithSeparators(TokenKind t1Kind, string t1Text,
            TokenKind separatorKind, string separatorText,
            TokenKind t2Kind, string t2Text)
        {
            var text = t1Text + separatorText + t2Text;
            var tokens = SyntaxTree.ParseTokens(text).ToArray();

            Assert.Equal(3, tokens.Length);
            Assert.Equal(t1Kind, tokens[0].Kind);
            Assert.Equal(t1Text, tokens[0].Text);
            Assert.Equal(separatorKind, tokens[1].Kind);
            Assert.Equal(separatorText, tokens[1].Text);
            Assert.Equal(t2Kind, tokens[2].Kind);
            Assert.Equal(t2Text, tokens[2].Text);
        }

        public static IEnumerable<object[]> GetTokensData()
        {
            foreach (var t in GetTokens().Concat(GetSeparators()))
                yield return new object[] { t.kind, t.text };
        }

        public static IEnumerable<object[]> GetTokenPairsData()
        {
            foreach (var t in GetTokenPairs())
                yield return new object[] { t.t1Kind, t.t1Text, t.t2Kind, t.t2Text };
        }

        public static IEnumerable<object[]> GetTokenPairsWithSeparatorData()
        {
            foreach (var t in GetTokenPairsWithSeparator())
                yield return new object[] { t.t1Kind, t.t1Text, t.separatorKind, t.separatorText, t.t2Kind, t.t2Text };
        }

        private static IEnumerable<(TokenKind kind, string text)> GetTokens()
        {
            var fixedTokens = Enum.GetValues(typeof(TokenKind))
                .Cast<TokenKind>()
                .Select(k => (kind: k, text: SyntaxFacts.GetText(k)))
                .Where(t => t.text != null);


            var dynamicTokens = new[]
            {
                (TokenKind.NumberLiteral, "1"),
                (TokenKind.NumberLiteral, "123"),
                (TokenKind.Identifier, "a"),
                (TokenKind.Identifier, "abc"),
                (TokenKind.StringLiteral, "\"Test\""),
                (TokenKind.StringLiteral, "\"Te\"\"st\""),
            };

            return fixedTokens.Concat(dynamicTokens);
        }

        private static IEnumerable<(TokenKind kind, string text)> GetSeparators()
        {
            return new[]
            {
                (TokenKind.Whitespace, " "),
                (TokenKind.Whitespace, "  "),
                (TokenKind.Whitespace, "\r"),
                (TokenKind.Whitespace, "\n"),
                (TokenKind.Whitespace, "\r\n")
            };
        }

        private static bool RequiresSeparator(TokenKind t1Kind, TokenKind t2Kind)
        {
            var t1IsKeyword = t1Kind.ToString().EndsWith("Keyword");
            var t2IsKeyword = t2Kind.ToString().EndsWith("Keyword");

            if (t1Kind == TokenKind.Identifier && t2Kind == TokenKind.Identifier)
                return true;

            if (t1IsKeyword && t2IsKeyword)
                return true;

            if (t1IsKeyword && t2Kind == TokenKind.Identifier)
                return true;

            if (t1Kind == TokenKind.Identifier && t2IsKeyword)
                return true;

            if (t1Kind == TokenKind.NumberLiteral && t2Kind == TokenKind.NumberLiteral)
                return true;

            if (t1Kind == TokenKind.StringLiteral && t2Kind == TokenKind.StringLiteral)
                return true;

            if (t1Kind == TokenKind.Bang && t2Kind == TokenKind.Equals)
                return true;

            if (t1Kind == TokenKind.Bang && t2Kind == TokenKind.EqualsEquals)
                return true;

            if (t1Kind == TokenKind.Equals && t2Kind == TokenKind.Equals)
                return true;

            if (t1Kind == TokenKind.Equals && t2Kind == TokenKind.EqualsEquals)
                return true;

            if (t1Kind == TokenKind.Less && t2Kind == TokenKind.Equals)
                return true;

            if (t1Kind == TokenKind.Less && t2Kind == TokenKind.EqualsEquals)
                return true;

            if (t1Kind == TokenKind.Greater && t2Kind == TokenKind.Equals)
                return true;

            if (t1Kind == TokenKind.Greater && t2Kind == TokenKind.EqualsEquals)
                return true;

            if (t1Kind == TokenKind.Ampersand && t2Kind == TokenKind.Ampersand)
                return true;

            if (t1Kind == TokenKind.Ampersand && t2Kind == TokenKind.AmpersandAmpersand)
                return true;

            if (t1Kind == TokenKind.Pipe && t2Kind == TokenKind.Pipe)
                return true;

            if (t1Kind == TokenKind.Pipe && t2Kind == TokenKind.PipePipe)
                return true;

            return false;
        }

        private static IEnumerable<(TokenKind t1Kind, string t1Text, TokenKind t2Kind, string t2Text)> GetTokenPairs()
        {
            foreach (var t1 in GetTokens())
            {
                foreach (var t2 in GetTokens())
                {
                    if (!RequiresSeparator(t1.kind, t2.kind))
                        yield return (t1.kind, t1.text, t2.kind, t2.text);
                }
            }
        }

        private static IEnumerable<(TokenKind t1Kind, string t1Text,
            TokenKind separatorKind, string separatorText,
            TokenKind t2Kind, string t2Text)> GetTokenPairsWithSeparator()
        {
            foreach (var t1 in GetTokens())
            {
                foreach (var t2 in GetTokens())
                {
                    if (RequiresSeparator(t1.kind, t2.kind))
                    {
                        foreach (var s in GetSeparators())
                            yield return (t1.kind, t1.text, s.kind, s.text, t2.kind, t2.text);
                    }
                }
            }
        }
    }
}