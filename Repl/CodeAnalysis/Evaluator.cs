using System;
using System.Collections.Generic;
using Repl.CodeAnalysis.Binding;

namespace Repl.CodeAnalysis
{
    public class Evaluator
    {
        private readonly BoundStatement _root;
        private readonly Dictionary<VariableSymbol, object> _variables;

        public Evaluator(BoundStatement root, Dictionary<VariableSymbol, object> variables)
        {
            _root = root;
            _variables = variables;
        }

        public object Evaluate()
        {
            EvaluateStatement(_root);
            return _lastValue;
        }

        private object EvaluateExpression(BoundExpression expression)
        {
            switch (expression)
            {
                case BoundBinaryExpression b:
                    return EvaluateBinaryExpression(b);
                case BoundUnaryExpression u:
                    return EvaluateUnaryExpression(u);
                case BoundLiteralExpression l:
                    return EvaluateLiteralExpression(l);
                case BoundAssignmentExpression a:
                    return EvaluateAssignmentExpression(a);
                case BoundVariableExpression v:
                    return EvaluateVariableExpression(v);
                default:
                    throw new Exception($"Unexpected node {expression.GetType()}");
            }
        }

        private object _lastValue;

        private void EvaluateStatement(BoundStatement statement)
        {
            switch (statement)
            {
                case BoundBlockStatement b:
                    EvaluateBlockStatement(b); return;
                case BoundVariableDeclaration v:
                    EvaluateVariableDeclaration(v); return;
                case BoundExpressionStatement e:
                    EvaluateExpressionStatement(e); return;
                default:
                    throw new Exception($"Unexpected node {statement.GetType()}");
            }
        }
        private void EvaluateVariableDeclaration(BoundVariableDeclaration node)
        {
            var value = EvaluateExpression(node.Initializer);
            _variables[node.Variable] = value;
            _lastValue = value;
        }

        private void EvaluateExpressionStatement(BoundExpressionStatement boundExpressionStatement)
        {
            _lastValue = EvaluateExpression(boundExpressionStatement.Expression);
        }

        private void EvaluateBlockStatement(BoundBlockStatement boundBlockStatement)
        {
            foreach (var statement in boundBlockStatement.Statements)
            {
                EvaluateStatement(statement);
            }
        }

        private object EvaluateVariableExpression(BoundVariableExpression boundVariableExpression)
        {
            var value = _variables[boundVariableExpression.Variable];
            return value;
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression boundAssignmentExpression)
        {
            var value = EvaluateExpression(boundAssignmentExpression.Expression);
            _variables[boundAssignmentExpression.Variable] = value;
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
                default: throw new Exception("Operator not implemented");
            }
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
                case BoundBinaryOperatorKind.LessThan: return (int)left < (int)right;
                case BoundBinaryOperatorKind.LessOrEquals: return (int)left <= (int)right;
                case BoundBinaryOperatorKind.GreaterThan: return (int)left > (int)right;
                case BoundBinaryOperatorKind.GreaterOrEquals: return (int)left >= (int)right;
                default: throw new Exception("Operator not implemented");

            }
        }
    }
}