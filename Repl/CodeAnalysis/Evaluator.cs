using System.Collections.Generic;
using Repl.CodeAnalysis.Binding;

namespace Repl.CodeAnalysis
{
    public class Evaluator
    {
        public BoundExpression Expression { get; }
        public Dictionary<VariableSymbol, object> Variables { get; }

        public Evaluator(BoundExpression expression, Dictionary<VariableSymbol, object> variables)
        {
            Expression = expression;
            Variables = variables;
        }

        public object Evaluate()
        {
            return EvaluateExpression(Expression);
        }

        private object EvaluateExpression(BoundExpression expression)
        {
            if (expression is BoundBinaryExpression b)
                return EvaluateBinaryExpression(b);
            if (expression is BoundUnaryExpression u)
                return EvaluateUnaryExpression(u);
            if (expression is BoundLiteralExpression l)
                return EvaluateLiteralExpression(l);
            if (expression is BoundAssignmentExpression a)
                return EvaluateAssignmentExpression(a);
            if (expression is BoundVariableExpression v)
                return EvaluateVariableExpression(v);
            return 0;
        }

        private object EvaluateVariableExpression(BoundVariableExpression boundVariableExpression)
        {
            var value = Variables[boundVariableExpression.Variable];
            return value;
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression boundAssignmentExpression)
        {
            var value = EvaluateExpression(boundAssignmentExpression.Expression);

            Variables[boundAssignmentExpression.Variable] = value;

            return value;
        }

        private object EvaluateLiteralExpression(BoundLiteralExpression boundLiteralExpression)
        {
            return boundLiteralExpression.Value;
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression boundUnaryExpression)
        {
            var operand = EvaluateExpression(boundUnaryExpression.Operand);

            switch (boundUnaryExpression.Operator.Kind)
            {
                case BoundUnaryOperatorKind.Identity: return (int)operand;
                case BoundUnaryOperatorKind.Negation: return -(int)operand;
                case BoundUnaryOperatorKind.LogicalNot: return !(bool)operand;
            }

            return 0;
        }

        private object EvaluateBinaryExpression(BoundBinaryExpression boundBinaryExpression)
        {
            var left = EvaluateExpression(boundBinaryExpression.Left);
            var right = EvaluateExpression(boundBinaryExpression.Right);

            switch (boundBinaryExpression.Operator.Kind)
            {
                case BoundBinaryOperatorKind.Addition: return (int)left + (int)right;
                case BoundBinaryOperatorKind.Subtraction: return (int)left - (int)right;
                case BoundBinaryOperatorKind.Multiplication: return (int)left * (int)right;
                case BoundBinaryOperatorKind.Division: return (int)left / (int)right;
                case BoundBinaryOperatorKind.LogicalAnd: return (bool)left && (bool)right;
                case BoundBinaryOperatorKind.LogicalOr: return (bool)left || (bool)right;
                case BoundBinaryOperatorKind.Equals: return left == right;
                case BoundBinaryOperatorKind.NotEquals: return left != right;
            }

            return 0;
        }
    }
}