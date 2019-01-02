using System.Collections.Generic;
using Repl.CodeAnalysis.Syntax;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundBinaryOperator
    {
        public TokenKind TokenKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public TypeSymbol LeftType { get; }
        public TypeSymbol RightType { get; }
        public TypeSymbol ResultType { get; }

        public BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, TypeSymbol type)
            : this(tokenKind, kind, type, type, type) { }

        public BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, TypeSymbol type, TypeSymbol resultType)
            : this(tokenKind, kind, type, type, resultType) { }

        public BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, TypeSymbol leftType, TypeSymbol rightType, TypeSymbol resultType)
        {
            TokenKind = tokenKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            ResultType = resultType;
        }

        private static IEnumerable<BoundBinaryOperator> NumericalOperators(TypeSymbol type, TypeSymbol boolType)
        {
            yield return new BoundBinaryOperator(TokenKind.Plus, BoundBinaryOperatorKind.Addition, type);
            yield return new BoundBinaryOperator(TokenKind.Minus, BoundBinaryOperatorKind.Subtraction, type);
            yield return new BoundBinaryOperator(TokenKind.Star, BoundBinaryOperatorKind.Multiplication, type);
            yield return new BoundBinaryOperator(TokenKind.Slash, BoundBinaryOperatorKind.Division, type);
            yield return new BoundBinaryOperator(TokenKind.Percent, BoundBinaryOperatorKind.Modulo, type);
            yield return new BoundBinaryOperator(TokenKind.Ampersand, BoundBinaryOperatorKind.BitwiseAnd, type);
            yield return new BoundBinaryOperator(TokenKind.Pipe, BoundBinaryOperatorKind.BitwiseOr, type);
            yield return new BoundBinaryOperator(TokenKind.Hat, BoundBinaryOperatorKind.BitwiseXor, type);
            yield return new BoundBinaryOperator(TokenKind.EqualsEquals, BoundBinaryOperatorKind.Equals, type,
                boolType);
            yield return new BoundBinaryOperator(TokenKind.BangEquals, BoundBinaryOperatorKind.NotEquals, type,
                boolType);
            yield return new BoundBinaryOperator(TokenKind.Less, BoundBinaryOperatorKind.LessThan, type, boolType);
            yield return new BoundBinaryOperator(TokenKind.LessEquals, BoundBinaryOperatorKind.LessOrEquals, type,
                boolType);
            yield return new BoundBinaryOperator(TokenKind.Greater, BoundBinaryOperatorKind.GreaterThan, type,
                boolType);
            yield return new BoundBinaryOperator(TokenKind.GreaterEquals, BoundBinaryOperatorKind.GreaterOrEquals, type, boolType);
        }

        private static IEnumerable<BoundBinaryOperator> BooleanOperators(TypeSymbol type)
        {
            yield return new BoundBinaryOperator(TokenKind.AmpersandAmpersand, BoundBinaryOperatorKind.LogicalAnd,
                type);
            yield return new BoundBinaryOperator(TokenKind.PipePipe, BoundBinaryOperatorKind.LogicalOr, type);
            yield return new BoundBinaryOperator(TokenKind.Ampersand, BoundBinaryOperatorKind.BitwiseAnd, type);
            yield return new BoundBinaryOperator(TokenKind.Pipe, BoundBinaryOperatorKind.BitwiseOr, type);
            yield return new BoundBinaryOperator(TokenKind.Hat, BoundBinaryOperatorKind.BitwiseXor, type);
            yield return new BoundBinaryOperator(TokenKind.EqualsEquals, BoundBinaryOperatorKind.Equals, type);
            yield return new BoundBinaryOperator(TokenKind.BangEquals, BoundBinaryOperatorKind.NotEquals, type);

        }

        private static readonly BoundBinaryOperator[] Operators = BoundOperators.GetOperators(NumericalOperators, BooleanOperators);


        public static BoundBinaryOperator Bind(TokenKind operatorTokenKind, TypeSymbol leftType, TypeSymbol rightType)
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