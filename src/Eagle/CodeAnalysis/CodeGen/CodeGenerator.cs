﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Eagle.CodeAnalysis.Binding;
using LLVMSharp.Interop;


namespace Eagle.CodeAnalysis.CodeGen
{
    public class CodeGenerator
    {
        private readonly BoundProgram _program;
        private readonly BoundGlobalScope _globalScope;

        public CodeGenerator(BoundProgram program, BoundGlobalScope globalScope)
        {
            _program = program;
            _globalScope = globalScope;
        }

        private LLVMBuilderRef _builder;
        private LLVMModuleRef _mod;

        private Dictionary<Symbol, LLVMValueRef> _symbols = new Dictionary<Symbol, LLVMValueRef>();
        private Dictionary<Symbol, LLVMTypeRef> _types = new Dictionary<Symbol, LLVMTypeRef>();

        public void Generate()
        {
            _mod = LLVMModuleRef.CreateWithName("MyMod");
            _builder = _mod.Context.CreateBuilder();



            var types = _globalScope.Symbols.OfType<TypeSymbol>();

            foreach (var type in types)
            {
                if (type == TypeSymbol.String) continue;

                var fields = _globalScope.Symbols.OfType<FieldSymbol>().Where(f => f.DeclaringType == type).ToList();
                var elements = fields.Select(f => GetXType(f.Type)).ToArray();
                var str = LLVMTypeRef.CreateStruct(elements, false);
                _types[type] = str;
            }

            var s = new List<IInvokableSymbol>();
            foreach (var type in types)
            {
                foreach (var member in type.Members)
                {
                    if (member is IInvokableSymbol ins)
                        s.Add(ins);

                    if (member is PropertySymbol ps)
                    {
                        if (ps.Getter != null)
                            s.Add(ps.Getter);

                        if (ps.Setter != null)
                            s.Add(ps.Setter);
                    }

                    if (member is IndexerSymbol inds)
                    {
                        if (inds.Getter != null)
                            s.Add(inds.Getter);

                        if (inds.Setter != null)
                            s.Add(inds.Setter);
                    }

                    //if (member is ConstructorSymbol cs)
                    //{
                    //    s.Add(cs);
                    //}
                }
            }

            var invokables = _globalScope.Symbols.OfType<IInvokableSymbol>()
                .Concat(s)
                .Concat(new[] { _program.MainFunction ?? _program.ScriptFunction })
                .ToArray();

            foreach (var invokable in invokables)
            {
                LLVMTypeRef type = null;
                switch (invokable)
                {
                    case FunctionSymbol fs:
                        type = CreateFunctionType(fs);
                        break;
                    case ConstructorSymbol cs:
                        var lt = GetXType(cs.Type);
                        type = CreateConstructorType(cs, lt);
                        break;
                    case MethodSymbol ms:
                        var lt1 = GetXType(ms.DeclaringType);
                        type = CreateMethodType(ms, lt1);
                        break;
                }

                var f = _mod.AddFunction(invokable.Name, type);
                _symbols[(Symbol)invokable] = f;
            }

            var a2 = _program.Functions.Select(x => x.Value.ToString()).ToArray();
            var array = _program.Functions.Select(x => x.Value.GetType()).ToArray();
            var strings = _program.Functions.Select(f =>
            {
                var writer = new StringWriter();
                ControlFlowGraph.Create(f.Value).WriteTo(writer);
                return writer.ToString();
            }).ToArray();

            foreach (var (symbol, body) in _program.Functions)
            {
                if (symbol.Extern) continue;

                var f = _symbols[(Symbol)symbol];
                var bb = f.AppendBasicBlock("entry");
                _builder.PositionAtEnd(bb);

                var rewriter = new PreCodeGenerationTreeRewriter();
                var newBody = (BoundBlockStatement)rewriter.RewriteStatement(body);

                var offset = 0;
                if (symbol is MethodSymbol || symbol is ConstructorSymbol)
                    offset = 1;

                foreach (var (index, parameter) in symbol.Parameters.Select((p, i) => (i, p)))
                {
                    _symbols[parameter] = f.Params[offset + index];
                }

                GenerateStatement(newBody);
            }
            _mod.Dump();

            if (!_mod.TryVerify(LLVMVerifierFailureAction.LLVMPrintMessageAction, out var message))
                Console.WriteLine("Issues:" + message);
        }

        //private LLVMTypeRef GetFieldType(BoundFieldDeclaration node)
        //{
        //    _context.Types.TryGetValue(node.Field.Type, out var type);
        //    return type;
        //}

        //private LLVMTypeRef GenerateMember(BoundMemberDeclaration node, LLVMTypeRef owner)
        //{
        //    switch (node)
        //    {
        //        case BoundMethodDeclaration m:
        //            GenerateMethodDeclaration(m, owner);
        //            break;
        //        case BoundConstructorDeclaration c:
        //            GenerateConstructorDeclaration(c, owner);
        //            break;
        //        default:
        //            throw new Exception($"Unexpected member {node.GetType()}");
        //    }
        //    return null;
        //}

        //private void GenerateConstructorDeclaration(BoundConstructorDeclaration node, LLVMTypeRef owner)
        //{
        //    //node.Indexer

        //    var constructor = node.Indexer;
        //    var ft = CreateConstructorType(constructor, owner);
        //    var f = _context.Module.AddFunction(constructor.Name, ft);
        //    _context.Symbols[constructor] = f;
        //    var entry = f.AppendBasicBlock("entry");

        //    using (var builder = _context.Module.Context.CreateBuilder())
        //    {
        //        builder.PositionAtEnd(entry);

        //        var c = new CodeGenerator(_context, builder);
        //        c.GenerateStatement(node.Body);

        //        builder.BuildRet(c._lastValue);
        //    }
        //}

        //private void GenerateMethodDeclaration(BoundMethodDeclaration node, LLVMTypeRef owner)
        //{
        //    GenerateMethodBase(node, owner);
        //}

        //private bool TryGenerateStructDeclaration(BoundClassDeclaration node)
        //{
        //    if (NativeTypeNames.Names.Contains(node.Type.Name)) return true;

        //    var fields = node.Members.OfType<BoundFieldDeclaration>().ToList();
        //    var fieldTypes = fields.Select(f => (f.Field, Type: GetFieldType(f))).ToList();
        //    if (fieldTypes.Any(ft => ft.Type == null)) return false;

        //    for (var i = 0; i < fieldTypes.Count; i++)
        //    {
        //        var fieldType = fieldTypes[i];
        //        _context.FieldIndicies[fieldType.Field] = i;
        //    }

        //    //var members = node.Members.Select(GenerateMember).ToList();
        //    var structType = LLVMTypeRef.CreateStruct(fieldTypes.Select(ft => ft.Type).ToArray(), false);

        //    //node.Members
        //    //    .Except(fields)
        //    //    .Select(x => GenerateMember(x, structType))
        //    //    .ToList();

        //    _context.Types[node.Type] = structType;
        //    return true;
        //}

        //private void GenerateStructDeclaration(BoundClassDeclaration node)
        //{
        //    var fields = node.Members.OfType<BoundFieldDeclaration>().ToList();
        //    //var fieldTypes = fields.Select(f => GenerateMember(f, null)).ToList();
        //    //var members = node.Members.Select(GenerateMember).ToList();
        //    //var structType = XType.Struct(fieldTypes);

        //    var structType = _context.Types[node.Type];

        //    node.Members
        //        .Except(fields)
        //        .Select(x => GenerateMember(x, structType))
        //        .ToList();

        //    _context.Types[node.Type] = structType;
        //}

        //private void GenerateConstDeclaration(BoundConstDeclaration node)
        //{
        //    var type = node.Const.Type;
        //    var nodeValue = node.Value;
        //    var value = GetAsValue(type, nodeValue);

        //    _context.Symbols[node.Const] = value;
        //}

        private LLVMValueRef GetAsValue(TypeSymbol type, object nodeValue)
        {
            LLVMValueRef value = null;
            if (type == TypeSymbol.Bool) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, (ulong)((bool)nodeValue ? 1 : 0));
            if (type == TypeSymbol.I16) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int16, (ulong)((short)Convert.ChangeType(nodeValue, typeof(short))));
            if (type == TypeSymbol.I32) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, (ulong)((int)Convert.ChangeType(nodeValue, typeof(int))));
            if (type == TypeSymbol.I64) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, (ulong)((long)Convert.ChangeType(nodeValue, typeof(long))));
            if (type == TypeSymbol.I8) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int8, (ulong)((sbyte)Convert.ChangeType(nodeValue, typeof(sbyte))));
            //if (type == _stringType) value = LLVMValueRef.String((string)nodeValue);
            if (type == TypeSymbol.U16) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int16, ((ushort)Convert.ChangeType(nodeValue, typeof(ushort))));
            if (type == TypeSymbol.U32) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, ((uint)Convert.ChangeType(nodeValue, typeof(uint))));
            if (type == TypeSymbol.U64) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, ((ulong)Convert.ChangeType(nodeValue, typeof(ulong))));
            if (type == TypeSymbol.U8) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int8, ((byte)Convert.ChangeType(nodeValue, typeof(byte))));

            if (type == TypeSymbol.UInt) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, (ulong)((long)Convert.ChangeType(nodeValue, typeof(long))));
            if (type == TypeSymbol.Int) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, (ulong)((long)Convert.ChangeType(nodeValue, typeof(long))));

            if (type == TypeSymbol.String)
                value = _builder.BuildGlobalString((string)nodeValue);

            if (type == TypeSymbol.Char) value = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int8, (char)nodeValue);

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
                    case BoundReturnStatement r:
                        GenerateReturnStatement(r);
                        break;
                    default:
                        throw new Exception($"Unexpected node {statement.GetType()}");
                }
            }
        }

        private void GenerateReturnStatement(BoundReturnStatement node)
        {
            var value = node.Expression != null ? GenerateExpression(node.Expression) : null;

            if (value != null)
                _builder.BuildRet(value);
            else
                _builder.BuildRetVoid();
        }

        private LLVMValueRef _lastValue;

        //private void GenerateExternDeclaration(BoundExternDeclaration node)
        //{
        //    var ft = CreateFunctionType(node.Function);
        //    var f = _context.Module.AddFunction(ft, node.Function.Name);
        //    _context.Symbols[node.Function] = f;
        //}

        //private void GenerateFunctionDeclaration(BoundFunctionDeclaration node)
        //{
        //    GenerateFunctionBase(node);
        //}

        //private void GenerateFunctionBase(BoundFunctionDeclaration node)
        //{
        //    var function = node.Function;
        //    var ft = CreateFunctionType(function);
        //    var f = _context.Module.AddFunction(function.Name, ft);
        //    _context.Symbols[function] = f;
        //    var entry = f.AppendBasicBlock("entry");

        //    using (var builder = _context.Module.Context.CreateBuilder())
        //    {
        //        builder.PositionAtEnd(entry);

        //        var c = new CodeGenerator(_context, builder);GenerateSt//atement(c.node.Body);

        //builder.//BuildRet._lastValue);
        //    }
        //}
        //
        //pri//vate void GenerateMethodBase(BoundMethodDeclaration node, LLVMTypeRef owner)
        //{
        //    var function = node.Method;
        //    var ft = CreateMethodType(function, owner);
        //    var f = _context.Module.AddFunction(function.Name, ft);
        //    _context.Symbols[function] = f;
        //    var entry = f.AppendBasicBlock("entry");

        //    using (var builder = _context.Module.Context.CreateBuilder())
        //    {
        //        builder.PositionAtEnd(entry);

        //        var c = new CodeGenerator(_context, builder);
        //        c.GenerateStatement(node.Body);

        //        builder.BuildRet(c._lastValue);
        //    }
        //}

        private LLVMTypeRef CreateFunctionType(FunctionSymbol function)
        {
            var returnType = GetXType(function.Type);
            var parameterTypes = function.Parameters.Select(p => GetXType(p.Type)).ToArray();
            return LLVMTypeRef.CreateFunction(returnType, parameterTypes);
        }

        private LLVMTypeRef CreateMethodType(MethodSymbol method, LLVMTypeRef owner)
        {
            var returnType = GetXType(method.Type);
            var parameterTypes = new[] { LLVMTypeRef.CreatePointer(owner, 0) }.Concat(method.Parameters.Select(p => GetXType(p.Type))).ToArray();
            return LLVMTypeRef.CreateFunction(returnType, parameterTypes);
        }

        private LLVMTypeRef CreateConstructorType(ConstructorSymbol constructor, LLVMTypeRef owner)
        {
            var returnType = GetXType(constructor.Type);
            var parameterTypes = new[] { LLVMTypeRef.CreatePointer(owner, 0) }.Concat(constructor.Parameters.Select(p => GetXType(p.Type))).ToArray();
            return LLVMTypeRef.CreateFunction(returnType, parameterTypes);
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
                targetPhi.AddIncoming(new[] { _lastValue }, new[] { currentBlock }, 1);
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

        private readonly Dictionary<BoundLabel, LLVMBasicBlockRef> _labels = new Dictionary<BoundLabel, LLVMBasicBlockRef>();

        private LLVMBasicBlockRef GetOrAppend(BoundLabel labelSymbol, bool addPhi = false)
        {
            if (_labels.TryGetValue(labelSymbol, out var label))
            {
                return label;
            }

            var target = Append(addPhi, labelSymbol.Name);

            _labels[labelSymbol] = target;
            return target;
        }

        private void GenerateGotoStatement(BoundGotoStatement node)
        {
            var currentBlock = _builder.InsertBlock;

            var target = GetOrAppend(node.Label, true);
            var targetPhi = target.FirstInstruction;
            targetPhi.AddIncoming(new[] { _lastValue }, new[] { currentBlock }, 1);
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
            if (variable.IsReadOnly)
            {
                if (!_symbols.TryGetValue(variable, out var _))
                {
                    _symbols[variable] = value;
                }
            }
            else
            {
                if (!_symbols.TryGetValue(variable, out var ptr))
                {
                    var xType = GetXType(variable.Type);
                    ptr = _builder.BuildAlloca(xType, variable.Name);
                    _symbols[variable] = ptr;
                }
                _builder.BuildStore(value, ptr);
            }
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
                case BoundArrayIndexExpression a:
                    return GenerateArrayIndexExpression(a);
                case BoundNewArrayExpression n:
                    return GenerateNewArrayExpression(n);
                default:
                    throw new Exception($"Unexpected node {expression.GetType()}");
            }
        }

        private LLVMValueRef GenerateNewArrayExpression(BoundNewArrayExpression node)
        {
            var type = GetXType(node.Type);
            var args = node.Arguments.Select(GenerateExpression).ToArray();
            return _builder.BuildArrayAlloca(type, args[0]);
        }

        private LLVMValueRef GenerateArrayIndexExpression(BoundArrayIndexExpression node)
        {
            var target = GenerateExpression(node.Target);
            var indexes = node.Arguments.Select(GenerateExpression).ToArray();
            return _builder.BuildGEP(target, indexes);
        }

        private LLVMValueRef GenerateConstructorCallExpression(BoundConstructorCallExpression node)
        {
            throw new NotImplementedException();
        }

        private LLVMValueRef GenerateFieldExpression(BoundFieldExpression node)
        {
            var index = 0; //_context.FieldIndicies[node.Field];
            LLVMValueRef target;
            if (node.Target != null)
            {
                target = GenerateExpression(node.Target);
            }
            else
            {
                target = _builder.InsertBlock.Parent.Params[0];
            }

            return _builder.BuildGEP(target, new[] { LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, (ulong)index) });
        }

        private LLVMValueRef GenerateMethodCallExpression(BoundMethodCallExpression node)
        {
            var target = GenerateExpression(node.Target);
            var arguments = new[] { target }
                .Concat(node.Arguments.Select(GenerateExpression))
                .ToArray();
            var function = _symbols[node.Method];
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
            var value = _symbols[node.Const];
            return value;
        }

        private LLVMValueRef GenerateCastExpression(BoundConversionExpression node)
        {
            var type = GetXType(node.Type);
            var value = GenerateExpression(node.Expression);
            if (node.Type.IsPointer || node.Type.IsArray)
                return _builder.BuildPointerCast(value, type);
            return _builder.BuildIntCast(value, type);
        }

        private LLVMValueRef GenerateParameterExpression(BoundParameterExpression node)
        {
            return _builder.InsertBlock.Parent.Params[node.Parameter.Index];
        }

        private LLVMValueRef GenerateFunctionCallExpression(BoundFunctionCallExpression node)
        {
            var function = _symbols[node.Function];
            var args = node.Arguments.Select(GenerateExpression).ToArray();
            return _builder.BuildCall(function, args);
        }

        private LLVMValueRef GenerateVariableExpression(BoundVariableExpression node)
        {
            var variable = node.Variable;
            if (_symbols.TryGetValue(variable, out var ptr))
                return ptr;
            //return _builder.BuildLoad(ptr, variable.Name);
            return null;
        }

        private LLVMValueRef GenerateAssignmentExpression(BoundAssignmentExpression node)
        {
            var target = GenerateExpression(node.Target);
            var value = GenerateExpression(node.Expression);

            _builder.BuildStore(value, target);
            return value;
            throw new NotImplementedException();
            //var variable = node.Variable;
            //if (!_symbols.TryGetValue(variable, out var ptr))
            //    throw new Exception("variable does not exist");

            //var value = GenerateExpression(node.Expression);
            //_builder.Store(value, ptr);

            //return value;
        }


        //var typeMap = new Dictionary<TypeSymbol, LLVMTypeRef>
        //{
        //    [TypeSymbol.Void] = LLVMTypeRef.Void,
        //    [TypeSymbol.String] = LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0),
        //    [TypeSymbol.Any] = LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0),
        //    [TypeSymbol.Bool] = LLVMTypeRef.Int1,
        //    [TypeSymbol.Char] = LLVMTypeRef.Int8,
        //    [TypeSymbol.I8] = LLVMTypeRef.Int8,
        //    [TypeSymbol.I16] = LLVMTypeRef.Int16,
        //    [TypeSymbol.I32] = LLVMTypeRef.Int32,
        //    [TypeSymbol.I64] = LLVMTypeRef.Int64,
        //    [TypeSymbol.U8] = LLVMTypeRef.Int8,
        //    [TypeSymbol.U16] = LLVMTypeRef.Int16,
        //    [TypeSymbol.U32] = LLVMTypeRef.Int32,
        //    [TypeSymbol.U64] = LLVMTypeRef.Int64,
        //    [TypeSymbol.Int] = LLVMTypeRef.Int64,
        //    [TypeSymbol.UInt] = LLVMTypeRef.Int64,
        //};

        private LLVMTypeRef GetXType(TypeSymbol type)
        {
            if (_types.TryGetValue(type, out var x))
                return x;
            if (type == TypeSymbol.Bool) return LLVMTypeRef.Int1;
            if (type == TypeSymbol.Char) return LLVMTypeRef.Int8;
            if (type == TypeSymbol.I8) return LLVMTypeRef.Int8;
            if (type == TypeSymbol.I16) return LLVMTypeRef.Int16;
            if (type == TypeSymbol.I32) return LLVMTypeRef.Int32;
            if (type == TypeSymbol.I64) return LLVMTypeRef.Int64;
            if (type == TypeSymbol.U8) return LLVMTypeRef.Int8;
            if (type == TypeSymbol.U16) return LLVMTypeRef.Int16;
            if (type == TypeSymbol.U32) return LLVMTypeRef.Int32;
            if (type == TypeSymbol.U64) return LLVMTypeRef.Int64;
            if (type == TypeSymbol.String) return LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0);
            if (type == TypeSymbol.Void) return LLVMTypeRef.Void;
            if (type == TypeSymbol.Int) return LLVMTypeRef.Int64;
            if (type == TypeSymbol.UInt) return LLVMTypeRef.Int64;
            if (type == TypeSymbol.Any) return LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0);
            if (type.IsPointer) return LLVMTypeRef.CreatePointer(GetXType(type.ElementType), 0);
            if (type.IsArray) return LLVMTypeRef.CreatePointer(GetXType(type.ElementType), 0);
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

            //if (type == _stringType)
            //    //return Value.String((string)value);
            //    return _builder.BuildGlobalStringPtr((string)value);
            return GetAsValue(type, value);
        }
    }
}