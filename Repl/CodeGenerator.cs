using System;
using System.Collections.Generic;
using XLang.Codegen;
using XLang.Codegen.Llvm;

namespace Repl
{
    public class CodeGenerator
    {
        private readonly Dictionary<VariableSymbol, Value> _variables;

        public XModule Module { get; } = new XModule("test");
        public BasicBlock BasicBlock { get; }
        public Function Function { get; }

        private readonly Builder _builder = new Builder();

        public CodeGenerator(Dictionary<VariableSymbol, Value> variables)
        {
            _variables = variables;

            var functionType = new FunctionType(XType.Int32);
            Function = Module.AddFunction(functionType, "__anon_expr");

            BasicBlock = Function.AppendBasicBlock();
            _builder.PositionAtEnd(BasicBlock);
        }

        public void Generate(BoundExpression expression)
        {
            using (_builder)
            {
                var value = GenerateExpression(expression);
                _builder.Ret(value);
            }
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
                ptr = _builder.Alloca(XType.Int32/*, variable.Name*/);
                _variables[variable] = ptr;
            }

            var value = GenerateExpression(boundAssignmentExpression.Expression);

            _builder.Store(value, ptr);

            return value;
        }

        private Value GenerateBinaryExpression(BoundBinaryExpression boundBinaryExpression)
        {
            var left = GenerateExpression(boundBinaryExpression.Left);
            var right = GenerateExpression(boundBinaryExpression.Right);

            switch (boundBinaryExpression.Operator.Kind)
            {
                case BoundBinaryOperatorKind.Addition:
                    return _builder.Add(left, right);
                case BoundBinaryOperatorKind.Multiplication:
                    return _builder.Mul(left, right);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Value GenerateUnaryExpression(BoundUnaryExpression boundUnaryExpression)
        {
            var operand = GenerateExpression(boundUnaryExpression.Operand);
            return operand;
        }

        private Value GenerateLiteralExpression(BoundLiteralExpression boundLiteralExpression)
        {
            return Const.Int32((int)boundLiteralExpression.Value);
        }
    }
}