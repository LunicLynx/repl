using System;
using System.Collections.Generic;
using Repl.CodeAnalysis.Syntax;
using Xunit;

namespace Repl.Tests
{
    public class SyntaxFactTests
    {
        [Theory]
        [MemberData(nameof(GetSyntaxKindData))]
        public void SyntaxFact_GetText_RoundTrips(TokenKind kind)
        {
            var text = SyntaxFacts.GetText(kind);
            if (text == null)
                return;

            var tokens = SyntaxTree.ParseTokens(text);
            var token = Assert.Single(tokens);
            Assert.Equal(kind, token.Kind);
            Assert.Equal(text, token.Text);
        }

        public static IEnumerable<object[]> GetSyntaxKindData()
        {
            var kinds = (TokenKind[])Enum.GetValues(typeof(TokenKind));
            foreach (var kind in kinds)
                yield return new object[] { kind };
        }
    }
}
