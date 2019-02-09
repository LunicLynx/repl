using System.Collections.Generic;
using Repl.CodeAnalysis.Syntax;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundUnaryOperator
    {
        public TokenKind TokenKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public TypeSymbol OperandType { get; }
        public TypeSymbol ResultType { get; }

        public BoundUnaryOperator(TokenKind tokenKind, BoundUnaryOperatorKind kind, TypeSymbol type)
            : this(tokenKind, kind, type, type) { }

        public BoundUnaryOperator(TokenKind tokenKind, BoundUnaryOperatorKind kind, TypeSymbol operandType, TypeSymbol resultType)
        {
            TokenKind = tokenKind;
            Kind = kind;
            OperandType = operandType;
            ResultType = resultType;
        }

        private static IEnumerable<BoundUnaryOperator> NumericalOperatorsSigned(TypeSymbol type, TypeSymbol _)
        {
            yield return new BoundUnaryOperator(TokenKind.Plus, BoundUnaryOperatorKind.Identity, type);
            yield return new BoundUnaryOperator(TokenKind.Minus, BoundUnaryOperatorKind.Negation, type);
            yield return new BoundUnaryOperator(TokenKind.Tilde, BoundUnaryOperatorKind.BitwiseComplement, type);
        }
        private static IEnumerable<BoundUnaryOperator> NumericalOperatorsUnsigned(TypeSymbol type, TypeSymbol _)
        {
            yield return new BoundUnaryOperator(TokenKind.Plus, BoundUnaryOperatorKind.Identity, type);
            yield return new BoundUnaryOperator(TokenKind.Tilde, BoundUnaryOperatorKind.BitwiseComplement, type);
        }

        private static IEnumerable<BoundUnaryOperator> BooleanOperators(TypeSymbol type)
        {
            yield return new BoundUnaryOperator(TokenKind.Bang, BoundUnaryOperatorKind.LogicalNot, type);
        }

        public static BoundUnaryOperator[] Operators;

        public static void Initialize(IScope scope)
        {
            Operators = BoundOperators.GetOperators(scope,
                NumericalOperatorsSigned,
                NumericalOperatorsUnsigned,
                BooleanOperators);
        }

        public static BoundUnaryOperator Bind(TokenKind operatorTokenKind, TypeSymbol operandType)
        {
            foreach (var @operator in Operators)
            {
                if (@operator.TokenKind == operatorTokenKind && @operator.OperandType == operandType)
                    return @operator;
            }

            return null;
        }
    }
}