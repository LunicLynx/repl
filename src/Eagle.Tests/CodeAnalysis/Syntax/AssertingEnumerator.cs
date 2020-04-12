using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.CodeAnalysis.Syntax;
using Xunit;

namespace Eagle.Tests.CodeAnalysis.Syntax
{
    internal sealed class AssertingEnumerator : IDisposable
    {
        private readonly IEnumerator<SyntaxNode> _enumerator;
        private bool _hasErrors;

        public AssertingEnumerator(SyntaxNode node)
        {
            _enumerator = Flatten(node).GetEnumerator();
        }

        private bool MarkFailed()
        {
            _hasErrors = true;
            return false;
        }

        public void Dispose()
        {
            if (!_hasErrors)
                Assert.False(_enumerator.MoveNext());

            _enumerator.Dispose();
        }

        private static IEnumerable<SyntaxNode> Flatten(SyntaxNode node)
        {
            var stack = new Stack<SyntaxNode>();
            stack.Push(node);

            while (stack.Count > 0)
            {
                var n = stack.Pop();
                yield return n;

                foreach (var child in n.GetChildren().Reverse())
                    stack.Push(child);
            }
        }

        public void AssertNode<T>()
        {
            try
            {
                Assert.True(_enumerator.MoveNext());
                Assert.IsType<T>(_enumerator.Current);
                Assert.IsNotType<Token>(_enumerator.Current);
            }
            catch when (MarkFailed())
            {
                throw;
            }
        }

        public void AssertToken(TokenKind kind, string text)
        {
            try
            {
                Assert.True(_enumerator.MoveNext());
                var token = Assert.IsType<Token>(_enumerator.Current);
                Assert.Equal(kind, token.Kind);
                Assert.Equal(text, token.Text);
            }
            catch when (MarkFailed())
            {
                throw;
            }
        }
    }
}