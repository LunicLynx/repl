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

        public Value Generate(BoundExpression expression)
        {
            return GenerateExpression(expression);
        }

        private Value GenerateExpression(BoundExpression expression)
        {
            if (expression is BoundBinaryExpression b)
                return GenerateBinaryExpression(b);
            if (expression is BoundUnaryExpression u)
                return GenerateUnaryExpression(u);
            if (expression is BoundLiteralExpression l)
                return GenerateLiteralExpression(l);
            if (expression is BoundAssignmentExpression a)
                return GenerateAssignmentExpression(a);
            if (expression is BoundVariableExpression v)
                return GenerateVariableExpression(v);
            return null;
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
            {
                var xType = GetXType(variable.Type);
                ptr = _builder.Alloca(xType, variable.Name);
                _variables[variable] = ptr;
            }

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
                return Const.Int1((bool)value);
            return Const.Int32((int)value);
        }
    }
}