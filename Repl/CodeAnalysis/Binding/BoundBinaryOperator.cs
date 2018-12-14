using System;
using Repl.CodeAnalysis.Syntax;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundBinaryOperator
    {
        public TokenKind TokenKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ResultType { get; }

        public BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, Type type)
            : this(tokenKind, kind, type, type, type) { }

        public BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, Type type, Type resultType)
            : this(tokenKind, kind, type, type, resultType) { }

        public BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType)
        {
            TokenKind = tokenKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            ResultType = resultType;
        }

        private static readonly BoundBinaryOperator[] Operators = {
            new BoundBinaryOperator(TokenKind.Plus, BoundBinaryOperatorKind.Addition, typeof(int)),
            new BoundBinaryOperator(TokenKind.Minus, BoundBinaryOperatorKind.Subtraction, typeof(int)),
            new BoundBinaryOperator(TokenKind.Star, BoundBinaryOperatorKind.Multiplication, typeof(int)),
            new BoundBinaryOperator(TokenKind.Slash, BoundBinaryOperatorKind.Division, typeof(int)),

            new BoundBinaryOperator(TokenKind.AmpersandAmpersand, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(TokenKind.PipePipe, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),

            new BoundBinaryOperator(TokenKind.EqualsEquals, BoundBinaryOperatorKind.Equals, typeof(int),typeof(bool)),
            new BoundBinaryOperator(TokenKind.EqualsEquals, BoundBinaryOperatorKind.Equals, typeof(bool)),
            new BoundBinaryOperator(TokenKind.BangEquals, BoundBinaryOperatorKind.NotEquals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(TokenKind.BangEquals, BoundBinaryOperatorKind.NotEquals, typeof(bool)),

            new BoundBinaryOperator(TokenKind.Less, BoundBinaryOperatorKind.LessThan, typeof(int), typeof(bool)),
            new BoundBinaryOperator(TokenKind.LessEquals, BoundBinaryOperatorKind.LessOrEquals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(TokenKind.Greater, BoundBinaryOperatorKind.GreaterThan, typeof(int), typeof(bool)),
            new BoundBinaryOperator(TokenKind.GreaterEquals, BoundBinaryOperatorKind.GreaterOrEquals, typeof(int), typeof(bool)),

        };

        public static BoundBinaryOperator Bind(TokenKind operatorTokenKind, Type leftType, Type rightType)
        {
            foreach (var @operator in Operators)
            {
                if (@operator.TokenKind == operatorTokenKind && @operator.LeftType == leftType &&
                    @operator.RightType == rightType)
                    return @operator;
            }

            return null;
        }
    }
}