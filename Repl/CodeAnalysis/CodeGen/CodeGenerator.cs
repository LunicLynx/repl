using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        private TypeSymbol _i32Type;
        private TypeSymbol _i64Type;
        private TypeSymbol _i16Type;
        private TypeSymbol _i8Type;
        private TypeSymbol _u64Type;
        private TypeSymbol _u32Type;
        private TypeSymbol _u16Type;
        private TypeSymbol _u8Type;
        private TypeSymbol _stringType;
        private TypeSymbol _boolType;
        private TypeSymbol _voidType;
        private TypeSymbol _intType;
        private TypeSymbol _uintType;

        public Value Generate(BoundUnit unit, ImmutableArray<Symbol> symbols)
        {
            _i64Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "Int64");
            _i32Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "Int32");
            _i16Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "Int16");
            _i8Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "Int8");
            _u64Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "UInt64");
            _u32Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "UInt32");
            _u16Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "UInt16");
            _u8Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "UInt8");
            _stringType = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "String");
            _boolType = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "Boolean");
            _voidType = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "Void");
            _intType = _i64Type;
            _uintType = _u64Type;

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
                case BoundAliasDeclaration s:
                    break;
                case BoundBlockStatement s:
                    GenerateStatement(s);
                    break;
                case BoundFunctionDeclaration f:
                    GenerateFunctionDeclaration(f);
                    break;
                case BoundExternDeclaration e:
                    GenerateExternDeclaration(e);
                    break;
                case BoundConstDeclaration c:
                    GenerateConstDeclaration(c);
                    break;
                default:
                    throw new Exception($"Unexpected node {node.GetType()}");
            }
        }

        private void GenerateConstDeclaration(BoundConstDeclaration node)
        {
            var type = node.Const.Type;
            var nodeValue = node.Value;
            var value = GetAsValue(type, nodeValue);

            _symbols[node.Const] = value;
        }

        private Value GetAsValue(TypeSymbol type, object nodeValue)
        {
            Value value = null;
            if (type == _boolType) value = Value.Int1((bool)nodeValue);
            if (type == _i16Type) value = Value.Int16((short)nodeValue);
            if (type == _i32Type) value = Value.Int32((int)nodeValue);
            if (type == _i64Type) value = Value.Int64((long)nodeValue);
            if (type == _i8Type) value = Value.Int8((sbyte)nodeValue);
            if (type == _stringType) value = Value.String((string)nodeValue);
            if (type == _u16Type) value = Value.UInt16((ushort)nodeValue);
            if (type == _u32Type) value = Value.UInt32((uint)nodeValue);
            if (type == _u64Type) value = Value.UInt64((ulong)nodeValue);
            if (type == _u8Type) value = Value.UInt8((byte)nodeValue);

            if (value == null) throw new Exception("");
            return value;
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
            var returnType = GetXType(function.ReturnType);
            var parameterTypes = function.Parameters.Select(p => GetXType(p.Type)).ToArray();
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
                case BoundFunctionCallExpression i:
                    return GenerateFunctionCallExpression(i);
                case BoundParameterExpression p:
                    return GenerateParameterExpression(p);
                case BoundCastExpression c:
                    return GenerateCastExpression(c);
                case BoundConstExpression c:
                    return GenerateConstExpression(c);
                case BoundPropertyExpression m:
                    return GeneratePropertyExpression(m);
                default:
                    throw new Exception($"Unexpected node {expression.GetType()}");
            }
        }

        private Value GeneratePropertyExpression(BoundPropertyExpression node)
        {
            var value = GenerateExpression(node.Target);
            string x;
            return null;
        }

        private Value GenerateConstExpression(BoundConstExpression node)
        {
            var value = _symbols[node.Const];
            return value;
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

        private Value GenerateFunctionCallExpression(BoundFunctionCallExpression node)
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
            throw new NotImplementedException();
            //var variable = node.Variable;
            //if (!_symbols.TryGetValue(variable, out var ptr))
            //    throw new Exception("variable does not exist");

            //var value = GenerateExpression(node.Expression);
            //_builder.Store(value, ptr);

            //return value;
        }

        private XType GetXType(TypeSymbol type)
        {
            if (type == _boolType) return XType.Int1;
            if (type == _i8Type) return XType.Int8;
            if (type == _i16Type) return XType.Int16;
            if (type == _i32Type) return XType.Int32;
            if (type == _i64Type) return XType.Int64;
            if (type == _u8Type) return XType.Int8;
            if (type == _u16Type) return XType.Int16;
            if (type == _u32Type) return XType.Int32;
            if (type == _u64Type) return XType.Int64;
            if (type == _stringType) return XType.Int64;
            if (type == _voidType) return XType.Void;
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
                case BoundBinaryOperatorKind.Equal:
                    return _builder.ICmpEq(left, right);
                case BoundBinaryOperatorKind.NotEqual:
                    return _builder.ICmpNe(left, right);
                case BoundBinaryOperatorKind.LogicalAnd:
                case BoundBinaryOperatorKind.BitwiseAnd:
                    return _builder.And(left, right);
                case BoundBinaryOperatorKind.LogicalOr:
                case BoundBinaryOperatorKind.BitwiseOr:
                    return _builder.Or(left, right);
                case BoundBinaryOperatorKind.LessThan:
                    return _builder.ICmpSlt(left, right);
                case BoundBinaryOperatorKind.LessOrEqual:
                    return _builder.ICmpSle(left, right);
                case BoundBinaryOperatorKind.GreaterThan:
                    return _builder.ICmpSgt(left, right);
                case BoundBinaryOperatorKind.GreaterOrEqual:
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

            if (type == _stringType)
                //return Value.String((string)value);
                return _builder.GlobalStringPtr((string)value);
            return GetAsValue(type, value);
        }
    }
}