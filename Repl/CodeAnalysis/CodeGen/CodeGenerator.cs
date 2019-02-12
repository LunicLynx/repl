using System;
using System.Collections.Generic;
using System.Linq;
using Repl.CodeAnalysis.Binding;
using XLang.Codegen.Llvm;

namespace Repl.CodeAnalysis.CodeGen
{
    public class CodeGenerator
    {
        private readonly CodeGeneratorContext _context;
        private readonly Builder _builder;

        public CodeGenerator(CodeGeneratorContext context, Builder builder)
        {
            _context = context;
            _builder = builder;

            InitializeTypes();
        }

        private Value _lastValue = Value.Int32(0);
        private TypeSymbol _i32Type;
        private TypeSymbol _i64Type;
        private readonly TypeSymbol _i16Type;
        private readonly TypeSymbol _i8Type;
        private TypeSymbol _u64Type;
        private readonly TypeSymbol _u32Type;
        private readonly TypeSymbol _u16Type;
        private readonly TypeSymbol _u8Type;
        private readonly TypeSymbol _stringType;
        private readonly TypeSymbol _boolType;
        private readonly TypeSymbol _voidType;
        private TypeSymbol _intType;
        private TypeSymbol _uintType;

        public Value Generate(BoundUnit unit)
        {
            var children = unit.GetChildren().ToList();

            var decl = children.OfType<BoundStructDeclaration>().ToList();

            var lastCount = decl.Count;
            while (decl.Any())
            {
                var x = decl.ToList();
                foreach (var item in x)
                {
                    if (TryGenerateStructDeclaration(item))
                        decl.Remove(item);
                }
                if (lastCount == decl.Count)
                    throw new Exception("circular dependency");
            }

            //foreach (var d in decl)
            //{
            //    TryGenerateStructDeclaration(d);
            //}
            var other = children.Except(decl).ToList();

            //InitializeTypes();

            _lastValue = GetAsValue(_intType, 0);

            foreach (var node in other)
            {
                GenerateNode(node);
            }

            return _lastValue;
        }

        private void InitializeTypes(/*ImmutableArray<Symbol> symbols*/)
        {
            var list = _context.Types.Cast<KeyValuePair<TypeSymbol, XType>?>().ToList();
            _i64Type = list.FirstOrDefault(kv => kv.Value.Key.Name == "Int64")?.Key;
            _i32Type = list.FirstOrDefault(kv => kv.Value.Key.Name == "Int32")?.Key;
            _u64Type = list.FirstOrDefault(kv => kv.Value.Key.Name == "UInt64")?.Key;
            //_i64Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "Int64");
            ////if(_i64Type == null && _types.) 
            //_i32Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "Int32");
            //_i16Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "Int16");
            //_i8Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "Int8");
            //_u64Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "UInt64");
            //_u32Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "UInt32");
            //_u16Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "UInt16");
            //_u8Type = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "UInt8");
            //_stringType = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "String");
            //_boolType = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "Boolean");
            //_voidType = symbols.OfType<TypeSymbol>().FirstOrDefault(t => t.Name == "Void");
            _intType = _i64Type;
            _uintType = _u64Type;
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
                case BoundStructDeclaration s:
                    GenerateStructDeclaration(s);
                    break;
                default:
                    throw new Exception($"Unexpected node {node.GetType()}");
            }
        }

        private XType GetFieldType(BoundFieldDeclaration node)
        {
            _context.Types.TryGetValue(node.Field.Type, out var type);
            return type;
        }

        private XType GenerateMember(BoundMemberDeclaration node, XType owner)
        {
            switch (node)
            {
                case BoundMethodDeclaration m:
                    GenerateMethodDeclaration(m, owner);
                    break;
                case BoundConstructorDeclaration c:
                    GenerateConstructorDeclaration(c, owner);
                    break;
                default:
                    throw new Exception($"Unexpected member {node.GetType()}");
            }
            return null;
        }

        private void GenerateConstructorDeclaration(BoundConstructorDeclaration node, XType owner)
        {
            //node.Constructor

            var constructor = node.Constructor;
            var ft = CreateConstructorType(constructor, owner);
            var f = _context.Module.AddFunction(ft, constructor.Name);
            _context.Symbols[constructor] = f;
            var entry = f.AppendBasicBlock("entry");

            using (var builder = new Builder())
            {
                builder.PositionAtEnd(entry);

                var c = new CodeGenerator(_context, builder);
                c.GenerateStatement(node.Body);

                builder.Ret(c._lastValue);
            }
        }

        private void GenerateMethodDeclaration(BoundMethodDeclaration node, XType owner)
        {
            GenerateMethodBase(node, owner);
        }

        private bool TryGenerateStructDeclaration(BoundStructDeclaration node)
        {
            if (NativeTypeNames.Names.Contains(node.Type.Name)) return true;

            var fields = node.Members.OfType<BoundFieldDeclaration>().ToList();
            var fieldTypes = fields.Select(f => (f.Field, Type: GetFieldType(f))).ToList();
            if (fieldTypes.Any(ft => ft.Type == null)) return false;

            for (var i = 0; i < fieldTypes.Count; i++)
            {
                var fieldType = fieldTypes[i];
                _context.FieldIndicies[fieldType.Field] = i;
            }

            //var members = node.Members.Select(GenerateMember).ToList();
            var structType = XType.Struct(fieldTypes.Select(ft => ft.Type));

            //node.Members
            //    .Except(fields)
            //    .Select(x => GenerateMember(x, structType))
            //    .ToList();

            _context.Types[node.Type] = structType;
            return true;
        }

        private void GenerateStructDeclaration(BoundStructDeclaration node)
        {
            var fields = node.Members.OfType<BoundFieldDeclaration>().ToList();
            //var fieldTypes = fields.Select(f => GenerateMember(f, null)).ToList();
            //var members = node.Members.Select(GenerateMember).ToList();
            //var structType = XType.Struct(fieldTypes);

            var structType = _context.Types[node.Type];

            node.Members
                .Except(fields)
                .Select(x => GenerateMember(x, structType))
                .ToList();

            _context.Types[node.Type] = structType;
        }

        private void GenerateConstDeclaration(BoundConstDeclaration node)
        {
            var type = node.Const.Type;
            var nodeValue = node.Value;
            var value = GetAsValue(type, nodeValue);

            _context.Symbols[node.Const] = value;
        }

        private Value GetAsValue(TypeSymbol type, object nodeValue)
        {
            Value value = null;
            if (type == _boolType) value = Value.Int1((bool)nodeValue);
            if (type == _i16Type) value = Value.Int16((short)Convert.ChangeType(nodeValue, typeof(short)));
            if (type == _i32Type) value = Value.Int32((int)Convert.ChangeType(nodeValue, typeof(int)));
            if (type == _i64Type) value = Value.Int64((long)Convert.ChangeType(nodeValue, typeof(long)));
            if (type == _i8Type) value = Value.Int8((sbyte)Convert.ChangeType(nodeValue, typeof(sbyte)));
            if (type == _stringType) value = Value.String((string)nodeValue);
            if (type == _u16Type) value = Value.UInt16((ushort)Convert.ChangeType(nodeValue, typeof(ushort)));
            if (type == _u32Type) value = Value.UInt32((uint)Convert.ChangeType(nodeValue, typeof(uint)));
            if (type == _u64Type) value = Value.UInt64((ulong)Convert.ChangeType(nodeValue, typeof(ulong)));
            if (type == _u8Type) value = Value.UInt8((byte)Convert.ChangeType(nodeValue, typeof(byte)));

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
            var f = _context.Module.AddFunction(ft, node.Function.Name);
            _context.Symbols[node.Function] = f;
        }

        private void GenerateFunctionDeclaration(BoundFunctionDeclaration node)
        {
            GenerateFunctionBase(node);
        }

        private void GenerateFunctionBase(BoundFunctionDeclaration node)
        {
            var function = node.Function;
            var ft = CreateFunctionType(function);
            var f = _context.Module.AddFunction(ft, function.Name);
            _context.Symbols[function] = f;
            var entry = f.AppendBasicBlock("entry");

            using (var builder = new Builder())
            {
                builder.PositionAtEnd(entry);

                var c = new CodeGenerator(_context, builder);
                c.GenerateStatement(node.Body);

                builder.Ret(c._lastValue);
            }
        }

        private void GenerateMethodBase(BoundMethodDeclaration node, XType owner)
        {
            var function = node.Method;
            var ft = CreateMethodType(function, owner);
            var f = _context.Module.AddFunction(ft, function.Name);
            _context.Symbols[function] = f;
            var entry = f.AppendBasicBlock("entry");

            using (var builder = new Builder())
            {
                builder.PositionAtEnd(entry);

                var c = new CodeGenerator(_context, builder);
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

        private FunctionType CreateMethodType(MethodSymbol method, XType owner)
        {
            var returnType = GetXType(method.Type);
            var parameterTypes = new[] { owner }.Concat(method.Parameters.Select(p => GetXType(p.Type))).ToArray();
            return new FunctionType(returnType, parameterTypes);
        }

        private FunctionType CreateConstructorType(ConstructorSymbol constructor, XType owner)
        {
            var returnType = GetXType(constructor.Type);
            var parameterTypes = new[] { owner }.Concat(constructor.Parameters.Select(p => GetXType(p.Type))).ToArray();
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
            if (_context.Symbols.TryGetValue(labelSymbol, out var label))
            {
                return label.AsBasicBlock();
            }

            var target = Append(addPhi, labelSymbol.Name);

            _context.Symbols[labelSymbol] = target.AsValue();
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
            if (!_context.Symbols.TryGetValue(variable, out var ptr))
            {
                var xType = GetXType(variable.Type);
                ptr = _builder.Alloca(xType, variable.Name);
                _context.Symbols[variable] = ptr;
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
                case BoundThisExpression t:
                    return GenerateThisExpression(t);
                case BoundMethodCallExpression m:
                    return GenerateMethodCallExpression(m);
                case BoundFieldExpression f:
                    return GenerateFieldExpression(f);
                case BoundConstructorCallExpression c:
                    return GenerateConstructorCallExpression(c);
                default:
                    throw new Exception($"Unexpected node {expression.GetType()}");
            }
        }

        private Value GenerateConstructorCallExpression(BoundConstructorCallExpression node)
        {
            throw new NotImplementedException();
        }

        private Value GenerateFieldExpression(BoundFieldExpression node)
        {
            var index = _context.FieldIndicies[node.Field];
            Value target;
            if (node.Target != null)
            {
                target = GenerateExpression(node.Target);
            }
            else
            {
                target = _builder.GetInsertBlock().GetParent().AsFunction().GetParam(0);
            }

            return _builder.GEP(target, new[] { Value.Int32(index) });
        }

        private Value GenerateMethodCallExpression(BoundMethodCallExpression node)
        {
            var target = GenerateExpression(node.Target);
            var arguments = new[] { target }
                .Concat(node.Arguments.Select(GenerateExpression))
                .ToArray();
            var function = _context.Symbols[node.Method];
            return _builder.Call(function, arguments);
        }

        private Value GenerateThisExpression(BoundThisExpression node)
        {
            return _builder.GetInsertBlock().GetParent().AsFunction().GetParam(0);
        }

        private Value GeneratePropertyExpression(BoundPropertyExpression node)
        {
            var value = GenerateExpression(node.Target);
            string x;
            return null;
        }

        private Value GenerateConstExpression(BoundConstExpression node)
        {
            var value = _context.Symbols[node.Const];
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
            var function = _context.Symbols[node.Function];
            var args = node.Arguments.Select(GenerateExpression).ToArray();
            return _builder.Call(function, args);
        }

        private Value GenerateVariableExpression(BoundVariableExpression node)
        {
            var variable = node.Variable;
            if (_context.Symbols.TryGetValue(variable, out var ptr))
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
            if (_context.Types.TryGetValue(type, out var x))
                return x;
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