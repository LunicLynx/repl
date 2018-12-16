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

        private object _lastValue = 0;
        private ActionKind _action;

        private void EvaluateStatement(BoundStatement statement)
        {
            switch (statement)
            {
                case BoundBlockStatement b:
                    EvaluateBlockStatement(b); return;
                case BoundVariableDeclaration v:
                    EvaluateVariableDeclaration(v); return;
                case BoundIfStatement i:
                    EvaluateIfStatement(i); return;
                case BoundLoopStatement l:
                    EvaluateLoopStatement(l); return;
                case BoundWhileStatement w:
                    EvaluateWhileStatement(w); return;
                case BoundContinueStatement c:
                    EvaluateContinueStatement(c); return;
                case BoundBreakStatement b:
                    EvaluateBreakStatement(b); return;
                case BoundExpressionStatement e:
                    EvaluateExpressionStatement(e); return;
                default:
                    throw new Exception($"Unexpected node {statement.GetType()}");
            }
        }

        private void EvaluateBreakStatement(BoundBreakStatement boundBreakStatement)
        {
            _action = ActionKind.Break;
        }

        private void EvaluateContinueStatement(BoundContinueStatement node)
        {
            _action = ActionKind.Continue;
        }

        private void EvaluateWhileStatement(BoundWhileStatement node)
        {
            do
            {
                _action = ActionKind.None;

                while (_action == ActionKind.None)
                {
                    var condition = (bool)EvaluateExpression(node.Condition);
                    if (!condition) break;
                    EvaluateBlockStatement(node.Block);
                }
            } while (_action == ActionKind.Continue);


            if (_action == ActionKind.Break)
                _action = ActionKind.None;
        }

        private enum ActionKind
        {
            None,
            Continue,
            Break,
            Return
        }

        private void EvaluateLoopStatement(BoundLoopStatement node)
        {
            do
            {
                _action = ActionKind.None;

                while (_action == ActionKind.None)
                {
                    EvaluateBlockStatement(node.Block);
                }
            } while (_action == ActionKind.Continue);


            if (_action == ActionKind.Break)
                _action = ActionKind.None;
        }

        private void EvaluateIfStatement(BoundIfStatement node)
        {
            var condition = (bool)EvaluateExpression(node.Condition);
            if (condition)
            {
                EvaluateBlockStatement(node.ThenBlock);
            }
            else if (node.ElseStatement != null)
            {
                EvaluateStatement(node.ElseStatement);
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
                if (_action != ActionKind.None)
                    break;
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
                case BoundUnaryOperatorKind.BitwiseComplement: return ~(int)operand;
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
                case BoundBinaryOperatorKind.BitwiseAnd when left is bool l && right is bool r: return l & r;
                case BoundBinaryOperatorKind.BitwiseAnd when left is int l && right is int r: return l & r;
                case BoundBinaryOperatorKind.BitwiseOr when left is bool l && right is bool r: return l | r;
                case BoundBinaryOperatorKind.BitwiseOr when left is int l && right is int r: return l | r;
                case BoundBinaryOperatorKind.BitwiseXor when left is bool l && right is bool r: return l ^ r;
                case BoundBinaryOperatorKind.BitwiseXor when left is int l && right is int r: return l ^ r;
                default: throw new Exception("Operator not implemented");

            }
        }
    }
}