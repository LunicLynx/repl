using System;
using System.Collections.Generic;
using System.Linq;
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

        public Value Generate(BoundUnit unit)
        {
            _lastValue = Value.Int32(0);

            foreach (var node in unit.GetChildren())
            {
                GenerateNode(node);
            }

            return _lastValue;
        }

        private void GenerateNode(BoundNode node)
        {
            switch (node)
            {
                case BoundBlockStatement s:
                    GenerateStatement(s);
                    break;
                case BoundFunctionDeclaration f:
                    GenerateFunctionDeclaration(f);
                    break;
                case BoundExternDeclaration e:
                    GenerateExternDeclaration(e);
                    break;
            }
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

                    default:
                        throw new Exception($"Unexpected node {statement.GetType()}");
                }
            }
        }

        private void GenerateExternDeclaration(BoundExternDeclaration node)
        {
            var ft = CreateFunctionType(node.Function);
            var f = _module.AddFunction(ft, node.Function.Name);
            _symbols[node.Function] = f;
        }

        private void GenerateFunctionDeclaration(BoundFunctionDeclaration node)
        {
            var ft = CreateFunctionType(node.Function);
            var f = _module.AddFunction(ft, node.Function.Name);
            _symbols[node.Function] = f;
            var entry = f.AppendBasicBlock("entry");

            using (var builder = new Builder())
            {
                builder.PositionAtEnd(entry);

                var c = new CodeGenerator(_module, builder, _symbols);
                c.GenerateStatement(node.Body);

                builder.Ret(c._lastValue);
            }
        }

        private FunctionType CreateFunctionType(FunctionSymbol function)
        {
            var returnType = GetXType(function.ReturnType.ClrType);
            var parameterTypes = function.Parameters.Select(p => GetXType(p.Type.ClrType)).ToArray();
            return new FunctionType(returnType, parameterTypes);
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

        private void GenerateExpressionStatement(BoundExpressionStatement node)
        {
            _lastValue = GenerateExpression(node.Expression);
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
                case BoundCallExpression i:
                    return GenerateInvokeExpression(i);
                case BoundParameterExpression p:
                    return GenerateParameterExpression(p);
                case BoundCastExpression c:
                    return GenerateCastExpression(c);
                default:
                    throw new Exception($"Unexpected node {expression.GetType()}");
            }
        }

        private Value GenerateCastExpression(BoundCastExpression node)
        {
            var type = GetXType(node.Type);
            var value = GenerateExpression(node.Expression);
            return _builder.IntCast(value, type);
        }

        private Value GenerateParameterExpression(BoundParameterExpression node)
        {
            return _builder.GetInsertBlock().GetParent().AsFunction().GetParam(node.Parameter.Index);
        }

        private Value GenerateInvokeExpression(BoundCallExpression node)
        {
            var function = _symbols[node.Function];
            var args = node.Arguments.Select(GenerateExpression).ToArray();
            return _builder.Call(function, args);
        }

        private Value GenerateVariableExpression(BoundVariableExpression node)
        {
            var variable = node.Variable;
            if (_symbols.TryGetValue(variable, out var ptr))
                return _builder.Load(ptr, variable.Name);
            return null;
        }

        private Value GenerateAssignmentExpression(BoundAssignmentExpression node)
        {
            var variable = node.Variable;
            if (!_symbols.TryGetValue(variable, out var ptr))
                throw new Exception("variable does not exist");

            var value = GenerateExpression(node.Expression);
            _builder.Store(value, ptr);

            return value;
        }

        private XType GetXType(Type type)
        {
            if (type == typeof(bool)) return XType.Int1;
            if (type == typeof(sbyte)) return XType.Int8;
            if (type == typeof(short)) return XType.Int16;
            if (type == typeof(int)) return XType.Int32;
            if (type == typeof(long)) return XType.Int64;
            if (type == typeof(byte)) return XType.Int8;
            if (type == typeof(ushort)) return XType.Int16;
            if (type == typeof(uint)) return XType.Int32;
            if (type == typeof(ulong)) return XType.Int64;
            if (type == typeof(string)) return XType.Int64;
            throw new Exception("Unsupported type");
        }

        private Value GenerateBinaryExpression(BoundBinaryExpression node)
        {
            var left = GenerateExpression(node.Left);
            var right = GenerateExpression(node.Right);

            switch (node.Operator.Kind)
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

        private Value GenerateUnaryExpression(BoundUnaryExpression node)
        {
            var operand = GenerateExpression(node.Operand);
            switch (node.Operator.Kind)
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

        private Value GenerateLiteralExpression(BoundLiteralExpression node)
        {
            var type = node.Type;
            var value = node.Value;
            if (type == typeof(bool))
                return Value.Int1((bool)value);
            if (type == typeof(string))
                //return Value.String((string)value);
                return _builder.GlobalStringPtr((string)value);
            return Value.Int32((int)value);
        }
    }
}