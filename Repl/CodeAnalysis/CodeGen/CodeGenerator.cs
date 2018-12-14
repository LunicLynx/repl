using System;
using System.Collections.Generic;
using Repl.CodeAnalysis.Binding;
using XLang.Codegen.Llvm;

namespace Repl.CodeAnalysis.CodeGen
{
    public class CodeGenerator
    {
        private readonly Dictionary<VariableSymbol, Value> _variables;
        private readonly Builder _builder;

        public CodeGenerator(Builder builder, Dictionary<VariableSymbol, Value> variables)
        {
            _builder = builder;
            _variables = variables;
        }

        private Value _lastValue;

        public Value Generate(BoundStatement statement)
        {
            _lastValue = Value.Int32(0);
            GenerateStatement(statement);
            return _lastValue;
        }

        private void GenerateStatement(BoundStatement statement)
        {
            switch (statement)
            {
                case BoundBlockStatement b:
                    GenerateBlockStatement(b);
                    return;
                case BoundVariableDeclaration v:
                    GenerateVariableDeclaration(v);
                    return;
                case BoundExpressionStatement e:
                    GenerateExpressionStatement(e);
                    return;
                case BoundIfStatement i:
                    GenerateIfStatement(i);
                    return;
                default:
                    throw new Exception($"Unexpected node {statement.GetType()}");
            }
        }

        private void GenerateIfStatement(BoundIfStatement node)
        {

        }

        private void GenerateVariableDeclaration(BoundVariableDeclaration node)
        {
            var value = GenerateExpression(node.Initializer);
            var variable = node.Variable;
            if (!_variables.TryGetValue(variable, out var ptr))
            {
                var xType = GetXType(variable.Type);
                ptr = _builder.Alloca(xType, variable.Name);
                _variables[variable] = ptr;
            }
            _builder.Store(value, ptr);
            _lastValue = value;
        }

        private void GenerateExpressionStatement(BoundExpressionStatement boundExpressionStatement)
        {
            _lastValue = GenerateExpression(boundExpressionStatement.Expression);
        }

        private void GenerateBlockStatement(BoundBlockStatement boundBlockStatement)
        {
            foreach (var statement in boundBlockStatement.Statements)
            {
                GenerateStatement(statement);
            }
        }

        private Value GenerateExpression(BoundExpression expression)
        {
            switch (expression)
            {
                case BoundBinaryExpression b:
                    return GenerateBinaryExpression(b);
                case BoundUnaryExpression u:
                    return GenerateUnaryExpression(u);
                case BoundLiteralExpression l:
                    return GenerateLiteralExpression(l);
                case BoundAssignmentExpression a:
                    return GenerateAssignmentExpression(a);
                case BoundVariableExpression v:
                    return GenerateVariableExpression(v);
                default:
                    throw new Exception($"Unexpected node {expression.GetType()}");
            }
        }

        private Value GenerateVariableExpression(BoundVariableExpression boundVariableExpression)
        {
            var variable = boundVariableExpression.Variable;
            if (_variables.TryGetValue(variable, out var ptr))
                return _builder.Load(ptr, variable.Name);
            return null;
        }

        private Value GenerateAssignmentExpression(BoundAssignmentExpression boundAssignmentExpression)
        {
            var variable = boundAssignmentExpression.Variable;
            if (!_variables.TryGetValue(variable, out var ptr))
                throw new Exception("variable does not exist");

            var value = GenerateExpression(boundAssignmentExpression.Expression);
            _builder.Store(value, ptr);

            return value;
        }

        private XType GetXType(Type type)
        {
            if (type == typeof(bool))
                return XType.Int1;
            return XType.Int32;
        }

        private Value GenerateBinaryExpression(BoundBinaryExpression boundBinaryExpression)
        {
            var left = GenerateExpression(boundBinaryExpression.Left);
            var right = GenerateExpression(boundBinaryExpression.Right);

            switch (boundBinaryExpression.Operator.Kind)
            {
                case BoundBinaryOperatorKind.Addition:
                    return _builder.Add(left, right);
                case BoundBinaryOperatorKind.Subtraction:
                    return _builder.Sub(left, right);
                case BoundBinaryOperatorKind.Multiplication:
                    return _builder.Mul(left, right);
                case BoundBinaryOperatorKind.Division:
                    return _builder.SDiv(left, right);
                case BoundBinaryOperatorKind.Equals:
                    return _builder.ICmpEq(left, right);
                case BoundBinaryOperatorKind.NotEquals:
                    return _builder.ICmpNe(left, right);
                case BoundBinaryOperatorKind.LogicalAnd:
                    return _builder.And(left, right);
                case BoundBinaryOperatorKind.LogicalOr:
                    return _builder.Or(left, right);
                case BoundBinaryOperatorKind.LessThan:
                    return _builder.ICmpSlt(left, right);
                case BoundBinaryOperatorKind.LessOrEquals:
                    return _builder.ICmpSle(left, right);
                case BoundBinaryOperatorKind.GreaterThan:
                    return _builder.ICmpSgt(left, right);
                case BoundBinaryOperatorKind.GreaterOrEquals:
                    return _builder.ICmpSge(left, right);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Value GenerateUnaryExpression(BoundUnaryExpression boundUnaryExpression)
        {
            var operand = GenerateExpression(boundUnaryExpression.Operand);
            switch (boundUnaryExpression.Operator.Kind)
            {
                case BoundUnaryOperatorKind.Identity:
                    return operand;
                case BoundUnaryOperatorKind.Negation:
                    return _builder.Neg(operand);
                case BoundUnaryOperatorKind.LogicalNot:
                    return _builder.Not(operand);
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private Value GenerateLiteralExpression(BoundLiteralExpression literal)
        {
            var type = literal.Type;
            var value = literal.Value;
            if (type == typeof(bool))
                return Value.Int1((bool)value);
            return Value.Int32((int)value);
        }
    }
}