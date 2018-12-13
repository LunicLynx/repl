using System;
using Repl.CodeAnalysis.Syntax;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundUnaryOperator
    {
        public TokenKind TokenKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type ResultType { get; }

        public BoundUnaryOperator(TokenKind tokenKind, BoundUnaryOperatorKind kind, Type type)
            : this(tokenKind, kind, type, type)
        {

        }

        public BoundUnaryOperator(TokenKind tokenKind, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
        {
            TokenKind = tokenKind;
            Kind = kind;
            OperandType = operandType;
            ResultType = resultType;
        }

        private static readonly BoundUnaryOperator[] Operators = {
            new BoundUnaryOperator(TokenKind.Plus, BoundUnaryOperatorKind.Identity, typeof(int)),
            new BoundUnaryOperator(TokenKind.Minus, BoundUnaryOperatorKind.Negation, typeof(int)),
            new BoundUnaryOperator(TokenKind.Bang, BoundUnaryOperatorKind.LogicalNot, typeof(bool))
        };

        public static BoundUnaryOperator Bind(TokenKind operatorTokenKind, Type operandType)
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