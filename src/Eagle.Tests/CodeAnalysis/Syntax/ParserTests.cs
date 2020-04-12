using System.Collections.Generic;
using Eagle.CodeAnalysis.Syntax;
using Xunit;

namespace Eagle.Tests.CodeAnalysis.Syntax
{
    public class ParserTests
    {
        [Theory]
        [MemberData(nameof(GetBinaryOperatorPairsData))]
        public void Parser_BinaryExpression_HonorsPrecedences(TokenKind op1, TokenKind op2)
        {
            var op1Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op1);
            var op2Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op2);
            var op1Text = SyntaxFacts.GetText(op1);
            var op2Text = SyntaxFacts.GetText(op2);
            var text = $"a {op1Text} b {op2Text} c";
            var expression = ParseExpression(text);

            if (op1Precedence >= op2Precedence)
            {
                //     op2
                //    /   \
                //   op1   c
                //  /   \
                // a     b

                using var e = new AssertingEnumerator(expression);
                e.AssertNode<BinaryExpressionSyntax>();
                e.AssertNode<BinaryExpressionSyntax>();
                e.AssertNode<NameExpressionSyntax>();
                e.AssertToken(TokenKind.Identifier, "a");
                e.AssertToken(op1, op1Text);
                e.AssertNode<NameExpressionSyntax>();
                e.AssertToken(TokenKind.Identifier, "b");
                e.AssertToken(op2, op2Text);
                e.AssertNode<NameExpressionSyntax>();
                e.AssertToken(TokenKind.Identifier, "c");
            }
            else
            {
                //   op1
                //  /   \
                // a    op2
                //     /   \
                //    b     c

                using var e = new AssertingEnumerator(expression);
                e.AssertNode<BinaryExpressionSyntax>();
                e.AssertNode<NameExpressionSyntax>();
                e.AssertToken(TokenKind.Identifier, "a");
                e.AssertToken(op1, op1Text);
                e.AssertNode<BinaryExpressionSyntax>();
                e.AssertNode<NameExpressionSyntax>();
                e.AssertToken(TokenKind.Identifier, "b");
                e.AssertToken(op2, op2Text);
                e.AssertNode<NameExpressionSyntax>();
                e.AssertToken(TokenKind.Identifier, "c");
            }
        }

        [Theory]
        [MemberData(nameof(GetUnaryOperatorPairsData))]
        public void Parser_UnaryExpression_HonorsPrecedences(TokenKind unaryKind, TokenKind binaryKind)
        {
            var unaryPrecedence = SyntaxFacts.GetUnaryOperatorPrecedence(unaryKind);
            var binaryPrecedence = SyntaxFacts.GetBinaryOperatorPrecedence(binaryKind);
            var unaryText = SyntaxFacts.GetText(unaryKind);
            var binaryText = SyntaxFacts.GetText(binaryKind);
            var text = $"{unaryText} a {binaryText} b";
            var expression = ParseExpression(text);

            if (unaryPrecedence >= binaryPrecedence)
            {
                //   binary
                //   /    \
                // unary   b
                //   |
                //   a

                using var e = new AssertingEnumerator(expression);
                e.AssertNode<BinaryExpressionSyntax>();
                e.AssertNode<UnaryExpressionSyntax>();
                e.AssertToken(unaryKind, unaryText);
                e.AssertNode<NameExpressionSyntax>();
                e.AssertToken(TokenKind.Identifier, "a");
                e.AssertToken(binaryKind, binaryText);
                e.AssertNode<NameExpressionSyntax>();
                e.AssertToken(TokenKind.Identifier, "b");
            }
            else
            {
                //  unary
                //    |
                //  binary
                //  /   \
                // a     b

                using var e = new AssertingEnumerator(expression);
                e.AssertNode<UnaryExpressionSyntax>();
                e.AssertToken(unaryKind, unaryText);
                e.AssertNode<BinaryExpressionSyntax>();
                e.AssertNode<NameExpressionSyntax>();
                e.AssertToken(TokenKind.Identifier, "a");
                e.AssertToken(binaryKind, binaryText);
                e.AssertNode<NameExpressionSyntax>();
                e.AssertToken(TokenKind.Identifier, "b");
            }
        }

        private static ExpressionSyntax ParseExpression(string text)
        {
            var syntaxTree = SyntaxTree.Parse(text);
            var root = syntaxTree.Root;
            var member = Assert.Single(root.Members);
            var globalStatement = Assert.IsType<GlobalStatementSyntax>(member);
            return Assert.IsType<ExpressionStatementSyntax>(globalStatement.Statement).Expression;
        }

        public static IEnumerable<object[]> GetBinaryOperatorPairsData()
        {
            foreach (var op1 in SyntaxFacts.GetBinaryOperatorKinds())
            {
                foreach (var op2 in SyntaxFacts.GetBinaryOperatorKinds())
                {
                    yield return new object[] { op1, op2 };
                }
            }
        }

        public static IEnumerable<object[]> GetUnaryOperatorPairsData()
        {
            foreach (var unary in SyntaxFacts.GetUnaryOperatorKinds())
            {
                foreach (var binary in SyntaxFacts.GetBinaryOperatorKinds())
                {
                    yield return new object[] { unary, binary };
                }
            }
        }
    }
}