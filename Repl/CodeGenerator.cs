using System;
using XLang.Codegen;
using XLang.Codegen.Llvm;

namespace Repl
{
    public class CodeGenerator
    {
        //XModule _module = new XModule("test");
        private readonly Builder _builder = new Builder();

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
            return null;
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