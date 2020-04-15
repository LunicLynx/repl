using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.CodeAnalysis.Binding;
using LLVMSharp.Interop;


namespace Eagle.CodeAnalysis.CodeGen
{
    public class CodeGenerator
    {
        private readonly CodeGeneratorContext _context;
        private readonly LLVMBuilderRef _builder;

        public CodeGenerator(CodeGeneratorContext context, LLVMBuilderRef builder)
        {
            _context = context;
            _builder = builder;

            InitializeTypes();
        }

        private LLVMValueRef _lastValue = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0);
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

        public LLVMValueRef Generate(BoundProgram program)
        {

            BoundNode[] children = null;

            var decl = children.OfType<BoundClassDeclaration>().ToList();

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
            var list = _context.Types.Cast<KeyValuePair<TypeSymbol, LLVMTypeRef>?>().ToList();
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
                //case BoundExternDeclaration e:
                //    GenerateExternDeclaration(e);
                //    break;
                case BoundConstDeclaration c:
                    GenerateConstDeclaration(c);
                    break;
                case BoundClassDeclaration s:
                    GenerateStructDeclaration(s);
                    break;
                default:
                    throw new Exception($"Unexpected node {node.GetType()}");
            }
        }

        private LLVMTypeRef GetFieldType(BoundFieldDeclaration node)
        {
            _context.Types.TryGetValue(node.Field.Type, out var type);
            return type;
        }

        private LLVMTypeRef GenerateMember(BoundMemberDeclaration node, LLVMTypeRef owner)
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

        private void GenerateConstructorDeclaration(BoundConstructorDeclaration node, LLVMTypeRef owner)
        {
            //node.Constructor

            var constructor = node.Constructor;
            var ft = CreateConstructorType(constructor, owner);
            var f = _context.Module.AddFunction(constructor.Name, ft);
            _context.Symbols[constructor] = f;
            var entry = f.AppendBasicBlock("entry");

            using (var builder = _context.Module.Context.CreateBuilder())
            {
                builder.PositionAtEnd(entry);

                var c = new CodeGenerator(_context, builder);
                c.GenerateStatement(node.Body);

                builder.BuildRet(c._lastValue);
            }
        }

        private void GenerateMethodDeclaration(BoundMethodDeclaration node, LLVMTypeRef owner)
        {
            GenerateMethodBase(node, owner);
        }

        private bool TryGenerateStructDeclaration(BoundClassDeclaration node)
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
            var structType = LLVMTypeRef.CreateStruct(fieldTypes.Select(ft => ft.Type).ToArray(), false);

            //node.Members
            //    .Except(fields)
            //    .Select(x => GenerateMember(x, structType))
            //    .ToList();

            _context.Types[node.Type] = structType;
            return true;
        }

        private void GenerateStructDeclaration(BoundClassDeclaration node)
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

        private LLVMValueRef GetAsValue(TypeSymbol type, object nodeValue)
        {
            LLVMValueRef value = null;
            if (type == _boolType) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, (ulong)((bool)nodeValue ? 1 : 0));
            if (type == _i16Type) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int16, (ulong)((short)Convert.ChangeType(nodeValue, typeof(short))));
            if (type == _i32Type) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, (ulong)((int)Convert.ChangeType(nodeValue, typeof(int))));
            if (type == _i64Type) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, (ulong)((long)Convert.ChangeType(nodeValue, typeof(long))));
            if (type == _i8Type) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int8, (ulong)((sbyte)Convert.ChangeType(nodeValue, typeof(sbyte))));
            //if (type == _stringType) value = LLVMValueRef.String((string)nodeValue);
            if (type == _u16Type) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int16, ((ushort)Convert.ChangeType(nodeValue, typeof(ushort))));
            if (type == _u32Type) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, ((uint)Convert.ChangeType(nodeValue, typeof(uint))));
            if (type == _u64Type) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, ((ulong)Convert.ChangeType(nodeValue, typeof(ulong))));
            if (type == _u8Type) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int8, ((byte)Convert.ChangeType(nodeValue, typeof(byte))));

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

        //private void GenerateExternDeclaration(BoundExternDeclaration node)
        //{
        //    var ft = CreateFunctionType(node.Function);
        //    var f = _context.Module.AddFunction(ft, node.Function.Name);
        //    _context.Symbols[node.Function] = f;
        //}

        private void GenerateFunctionDeclaration(BoundFunctionDeclaration node)
        {
            GenerateFunctionBase(node);
        }

        private void GenerateFunctionBase(BoundFunctionDeclaration node)
        {
            var function = node.Function;
            var ft = CreateFunctionType(function);
            var f = _context.Module.AddFunction(function.Name, ft);
            _context.Symbols[function] = f;
            var entry = f.AppendBasicBlock("entry");

            using (var builder = _context.Module.Context.CreateBuilder())
            {
                builder.PositionAtEnd(entry);

                var c = new CodeGenerator(_context, builder);
                c.GenerateStatement(node.Body);

                builder.BuildRet(c._lastValue);
            }
        }

        private void GenerateMethodBase(BoundMethodDeclaration node, LLVMTypeRef owner)
        {
            var function = node.Method;
            var ft = CreateMethodType(function, owner);
            var f = _context.Module.AddFunction(function.Name, ft);
            _context.Symbols[function] = f;
            var entry = f.AppendBasicBlock("entry");

            using (var builder = _context.Module.Context.CreateBuilder())
            {
                builder.PositionAtEnd(entry);

                var c = new CodeGenerator(_context, builder);
                c.GenerateStatement(node.Body);

                builder.BuildRet(c._lastValue);
            }
        }

        private LLVMTypeRef CreateFunctionType(FunctionSymbol function)
        {
            var returnType = GetXType(function.Type);
            var parameterTypes = function.Parameters.Select(p => GetXType(p.Type)).ToArray();
            return LLVMTypeRef.CreateFunction(returnType, parameterTypes);
            //return new FunctionType(returnType, parameterTypes);
        }

        private LLVMTypeRef CreateMethodType(MethodSymbol method, LLVMTypeRef owner)
        {
            var returnType = GetXType(method.Type);
            var parameterTypes = new[] { owner }.Concat(method.Parameters.Select(p => GetXType(p.Type))).ToArray();
            return LLVMTypeRef.CreateFunction(returnType, parameterTypes);
            //return new FunctionType(returnType, parameterTypes);
        }

        private LLVMTypeRef CreateConstructorType(ConstructorSymbol constructor, LLVMTypeRef owner)
        {


            var returnType = GetXType(constructor.Type);
            var parameterTypes = new[] { owner }.Concat(constructor.Parameters.Select(p => GetXType(p.Type))).ToArray();
            return LLVMTypeRef.CreateFunction(returnType, parameterTypes);
            //return new FunctionType(returnType, parameterTypes);
        }

        private void GenerateLabelStatement(BoundLabelStatement node)
        {
            
            var currentBlock = _builder.InsertBlock;
            var target = GetOrAppend(node.Label, true);
            target.MoveAfter(currentBlock);

            var targetPhi = target.FirstInstruction;//.AsPhi();

            // if the current block is not completed
            // we need to branch to the target block
            // otherwise we can expect to never reach this label
            // and we can just switch to the target block for emitting
            // this happens for the else statement: a goto followed by a label
            //if (!currentBlock.IsTerminated())
            if (currentBlock.Terminator != null)
            {
                targetPhi.AddIncoming(new []{ _lastValue}, new []{ currentBlock }, 1);
                _builder.BuildBr(target);
            }

            _lastValue = targetPhi;

            _builder.PositionAtEnd(target);
        }

        private LLVMBasicBlockRef Append(bool addPhi = false, string name = "")
        {
            var insertBlock = _builder.InsertBlock;
            var function = insertBlock.Parent;
            var target = function.AppendBasicBlock(name);

            if (addPhi)
            {
                _builder.PositionAtEnd(target);
                _builder.BuildPhi(LLVMTypeRef.Int32);
            }

            _builder.PositionAtEnd(insertBlock);
            return target;
        }

        private LLVMBasicBlockRef GetOrAppend(BoundLabel labelSymbol, bool addPhi = false)
        {
            //if (_context.Symbols.TryGetValue(labelSymbol, out var label))
            //{
            //    return label.AsBasicBlock();
            //}

            var target = Append(addPhi, labelSymbol.Name);

            //_context.Symbols[labelSymbol] = target.AsValue();
            return target;
        }

        private void GenerateGotoStatement(BoundGotoStatement node)
        {
            var currentBlock = _builder.InsertBlock;

            var target = GetOrAppend(node.Label, true);
            var targetPhi = target.FirstInstruction;
            targetPhi.AddIncoming(new []{ _lastValue}, new []{ currentBlock},1);
            _builder.BuildBr(target);
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
                _builder.BuildCondBr(value, target, fallThrough);
            }
            else
            {
                _builder.BuildCondBr(value, fallThrough, target);
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
                ptr = _builder.BuildAlloca(xType, variable.Name);
                _context.Symbols[variable] = ptr;
            }
            _builder.BuildStore(value, ptr);
            _lastValue = value;
        }

        private void GenerateExpressionStatement(BoundExpressionStatement node)
        {
            _lastValue = GenerateExpression(node.Expression);
        }

        private LLVMValueRef GenerateExpression(BoundExpression expression)
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
                case BoundConversionExpression c:
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

        private LLVMValueRef GenerateConstructorCallExpression(BoundConstructorCallExpression node)
        {
            throw new NotImplementedException();
        }

        private LLVMValueRef GenerateFieldExpression(BoundFieldExpression node)
        {
            var index = _context.FieldIndicies[node.Field];
            LLVMValueRef target;
            if (node.Target != null)
            {
                target = GenerateExpression(node.Target);
            }
            else
            {
                target = _builder.InsertBlock.Parent.Params[0];
            }

            return _builder.BuildGEP(target, new[] { LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, (ulong) index) });
        }

        private LLVMValueRef GenerateMethodCallExpression(BoundMethodCallExpression node)
        {
            var target = GenerateExpression(node.Target);
            var arguments = new[] { target }
                .Concat(node.Arguments.Select(GenerateExpression))
                .ToArray();
            var function = _context.Symbols[node.Method];
            return _builder.BuildCall(function, arguments);
        }

        private LLVMValueRef GenerateThisExpression(BoundThisExpression node)
        {
            return _builder.InsertBlock.Parent.Params[0];
        }

        private LLVMValueRef GeneratePropertyExpression(BoundPropertyExpression node)
        {
            var value = GenerateExpression(node.Target);
            return null;
        }

        private LLVMValueRef GenerateConstExpression(BoundConstExpression node)
        {
            var value = _context.Symbols[node.Const];
            return value;
        }

        private LLVMValueRef GenerateCastExpression(BoundConversionExpression node)
        {
            var type = GetXType(node.Type);
            var value = GenerateExpression(node.Expression);
            return _builder.BuildIntCast(value, type);
        }

        private LLVMValueRef GenerateParameterExpression(BoundParameterExpression node)
        {
            return _builder.InsertBlock.Parent.Params[node.Parameter.Index];
        }

        private LLVMValueRef GenerateFunctionCallExpression(BoundFunctionCallExpression node)
        {
            var function = _context.Symbols[node.Function];
            var args = node.Arguments.Select(GenerateExpression).ToArray();
            return _builder.BuildCall(function, args);
        }

        private LLVMValueRef GenerateVariableExpression(BoundVariableExpression node)
        {
            var variable = node.Variable;
            if (_context.Symbols.TryGetValue(variable, out var ptr))
                return _builder.BuildLoad(ptr, variable.Name);
            return null;
        }

        private LLVMValueRef GenerateAssignmentExpression(BoundAssignmentExpression node)
        {
            throw new NotImplementedException();
            //var variable = node.Variable;
            //if (!_symbols.TryGetValue(variable, out var ptr))
            //    throw new Exception("variable does not exist");

            //var value = GenerateExpression(node.Expression);
            //_builder.Store(value, ptr);

            //return value;
        }

        private LLVMTypeRef GetXType(TypeSymbol type)
        {
            if (_context.Types.TryGetValue(type, out var x))
                return x;
            if (type == _boolType) return LLVMTypeRef.Int1;
            if (type == _i8Type) return LLVMTypeRef.Int8;
            if (type == _i16Type) return LLVMTypeRef.Int16;
            if (type == _i32Type) return LLVMTypeRef.Int32;
            if (type == _i64Type) return LLVMTypeRef.Int64;
            if (type == _u8Type) return LLVMTypeRef.Int8;
            if (type == _u16Type) return LLVMTypeRef.Int16;
            if (type == _u32Type) return LLVMTypeRef.Int32;
            if (type == _u64Type) return LLVMTypeRef.Int64;
            if (type == _stringType) return LLVMTypeRef.Int64;
            if (type == _voidType) return LLVMTypeRef.Void;
            throw new Exception("Unsupported type");
        }

        private LLVMValueRef GenerateBinaryExpression(BoundBinaryExpression node)
        {
            var left = GenerateExpression(node.Left);
            var right = GenerateExpression(node.Right);

            switch (node.Operator.Kind)
            {
                case BoundBinaryOperatorKind.Addition:
                    return _builder.BuildAdd(left, right);
                case BoundBinaryOperatorKind.Subtraction:
                    return _builder.BuildSub(left, right);
                case BoundBinaryOperatorKind.Multiplication:
                    return _builder.BuildMul(left, right);
                case BoundBinaryOperatorKind.Division:
                    return _builder.BuildSDiv(left, right);
                case BoundBinaryOperatorKind.Modulo:
                    return _builder.BuildSRem(left, right);
                case BoundBinaryOperatorKind.Equal:
                    return _builder.BuildICmp(LLVMIntPredicate.LLVMIntEQ, left, right);
                case BoundBinaryOperatorKind.NotEqual:
                    return _builder.BuildICmp(LLVMIntPredicate.LLVMIntNE, left, right);
                case BoundBinaryOperatorKind.LogicalAnd:
                case BoundBinaryOperatorKind.BitwiseAnd:
                    return _builder.BuildAnd(left, right);
                case BoundBinaryOperatorKind.LogicalOr:
                case BoundBinaryOperatorKind.BitwiseOr:
                    return _builder.BuildOr(left, right);
                case BoundBinaryOperatorKind.LessThan:
                    return _builder.BuildICmp(LLVMIntPredicate.LLVMIntSLT, left, right);
                case BoundBinaryOperatorKind.LessOrEqual:
                    return _builder.BuildICmp(LLVMIntPredicate.LLVMIntSLE, left, right);
                case BoundBinaryOperatorKind.GreaterThan:
                    return _builder.BuildICmp(LLVMIntPredicate.LLVMIntSGT, left, right);
                case BoundBinaryOperatorKind.GreaterOrEqual:
                    return _builder.BuildICmp(LLVMIntPredicate.LLVMIntSGE, left, right);
                case BoundBinaryOperatorKind.BitwiseXor:
                    return _builder.BuildXor(left, right);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private LLVMValueRef GenerateUnaryExpression(BoundUnaryExpression node)
        {
            var operand = GenerateExpression(node.Operand);
            switch (node.Operator.Kind)
            {
                case BoundUnaryOperatorKind.Identity:
                    return operand;
                case BoundUnaryOperatorKind.Negation:
                    return _builder.BuildNeg(operand);
                case BoundUnaryOperatorKind.LogicalNot:
                    return _builder.BuildNot(operand);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private LLVMValueRef GenerateLiteralExpression(BoundLiteralExpression node)
        {
            var type = node.Type;
            var value = node.Value;

            if (type == _stringType)
                //return Value.String((string)value);
                return _builder.BuildGlobalStringPtr((string)value);
            return GetAsValue(type, value);
        }
    }
}