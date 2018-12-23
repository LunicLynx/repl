using System;
using System.Collections.Generic;
using Repl.CodeAnalysis.Binding;
using XLang.Codegen.Llvm;

namespace Repl.CodeAnalysis.CodeGen
{
    public class CodeGenerator
    {
        private readonly Dictionary<Symbol, Value> _symbols;
        private readonly XModule _module;
        private readonly Builder _builder;

        public CodeGenerator(XModule module, Builder builder, Dictionary<Symbol, Value> symbols)
        {
            _module = module;
            _builder = builder;
            _symbols = symbols;
        }

        private Value _lastValue = Value.Int32(0);

        public Value Generate(BoundBlockStatement statement)
        {
            _lastValue = Value.Int32(0);
            GenerateStatement(statement);
            return _lastValue;
        }

        private void GenerateStatement(BoundBlockStatement node)
        {
            foreach (var statement in node.Statements)
            {
                switch (statement)
                {
                    case BoundVariableDeclaration v:
                        GenerateVariableDeclaration(v);
                        break;
                    case BoundExpressionStatement e:
                        GenerateExpressionStatement(e);
                        break;
                    case BoundConditionalGotoStatement c:
                        GenerateConditionalGotoStatement(c);
                        break;
                    case BoundGotoStatement g:
                        GenerateGotoStatement(g);
                        break;
                    case BoundLabelStatement l:
                        GenerateLabelStatement(l);
                        break;
                    case BoundFunctionDeclaration f:
                        GenerateFunctionDeclaration(f);
                        break;

                    default:
                        throw new Exception($"Unexpected node {statement.GetType()}");
                }
            }
        }

        private void GenerateFunctionDeclaration(BoundFunctionDeclaration node)
        {
            
        }

        private void GenerateLabelStatement(BoundLabelStatement node)
        {
            var currentBlock = _builder.GetInsertBlock();
            var target = GetOrAppend(node.Label, true);
            target.MoveAfter(currentBlock);

            var targetPhi = target.GetFirstInstruction().AsPhi();

            // if the current block is not completed
            // we need to branch to the target block
            // otherwise we can expect to never reach this label
            // and we can just switch to the target block for emitting
            // this happens for the else statement: a goto followed by a label
            if (!currentBlock.IsTerminated())
            {
                targetPhi.AddIncoming(_lastValue, currentBlock);
                _builder.Br(target);
            }

            _lastValue = targetPhi;

            _builder.PositionAtEnd(target);
        }

        private BasicBlock Append(bool addPhi = false, string name = "")
        {
            var insertBlock = _builder.GetInsertBlock();
            var function = insertBlock.GetParent().AsFunction();
            var target = function.AppendBasicBlock(name);

            if (addPhi)
            {
                _builder.PositionAtEnd(target);
                _builder.Phi(XType.Int32);
            }

            _builder.PositionAtEnd(insertBlock);
            return target;
        }

        private BasicBlock GetOrAppend(LabelSymbol labelSymbol, bool addPhi = false)
        {
            if (_symbols.TryGetValue(labelSymbol, out var label))
            {
                return label.AsBasicBlock();
            }

            var target = Append(addPhi, labelSymbol.Name);

            _symbols[labelSymbol] = target.AsValue();
            return target;
        }

        private void GenerateGotoStatement(BoundGotoStatement node)
        {
            var currentBlock = _builder.GetInsertBlock();

            var target = GetOrAppend(node.Label, true);
            var targetPhi = target.GetFirstInstruction().AsPhi();
            targetPhi.AddIncoming(_lastValue, currentBlock);
            _builder.Br(target);
            _lastValue = targetPhi;
            // TODO fall through creates issues a block should only have one terminator and it should be the last instruction
            // Do we need to reposition the builder?
            // does dead code crash llvm ?
        }

        private void GenerateConditionalGotoStatement(BoundConditionalGotoStatement node)
        {
            var fallThrough = Append();
            var target = GetOrAppend(node.Label);

            var lastValue = _lastValue;
            var value = GenerateExpression(node.Condition);
            _lastValue = lastValue;

            if (node.JumpIfTrue)
            {
                _builder.CondBr(value, target, fallThrough);
            }
            else
            {
                _builder.CondBr(value, fallThrough, target);
            }

            _builder.PositionAtEnd(fallThrough);
        }

        private void GenerateVariableDeclaration(BoundVariableDeclaration node)
        {
            var value = GenerateExpression(node.Initializer);
            var variable = node.Variable;
            if (!_symbols.TryGetValue(variable, out var ptr))
            {
                var xType = GetXType(variable.Type);
                ptr = _builder.Alloca(xType, variable.Name);
                _symbols[variable] = ptr;
            }
            _builder.Store(value, ptr);
            _lastValue = value;
        }

        private void GenerateExpressionStatement(BoundExpressionStatement boundExpressionStatement)
        {
            _lastValue = GenerateExpression(boundExpressionStatement.Expression);
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
            if (_symbols.TryGetValue(variable, out var ptr))
                return _builder.Load(ptr, variable.Name);
            return null;
        }

        private Value GenerateAssignmentExpression(BoundAssignmentExpression boundAssignmentExpression)
        {
            var variable = boundAssignmentExpression.Variable;
            if (!_symbols.TryGetValue(variable, out var ptr))
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
                case BoundBinaryOperatorKind.Modulo:
                    return _builder.SRem(left, right);
                case BoundBinaryOperatorKind.Equals:
                    return _builder.ICmpEq(left, right);
                case BoundBinaryOperatorKind.NotEquals:
                    return _builder.ICmpNe(left, right);
                case BoundBinaryOperatorKind.LogicalAnd:
                case BoundBinaryOperatorKind.BitwiseAnd:
                    return _builder.And(left, right);
                case BoundBinaryOperatorKind.LogicalOr:
                case BoundBinaryOperatorKind.BitwiseOr:
                    return _builder.Or(left, right);
                case BoundBinaryOperatorKind.LessThan:
                    return _builder.ICmpSlt(left, right);
                case BoundBinaryOperatorKind.LessOrEquals:
                    return _builder.ICmpSle(left, right);
                case BoundBinaryOperatorKind.GreaterThan:
                    return _builder.ICmpSgt(left, right);
                case BoundBinaryOperatorKind.GreaterOrEquals:
                    return _builder.ICmpSge(left, right);
                case BoundBinaryOperatorKind.BitwiseXor:
                    return _builder.Xor(left, right);
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