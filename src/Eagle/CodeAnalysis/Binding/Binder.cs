using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Eagle.CodeAnalysis.Lowering;
using Eagle.CodeAnalysis.Syntax;
using Eagle.CodeAnalysis.Text;

namespace Eagle.CodeAnalysis.Binding
{
    public class Binder
    {
        private readonly bool _isScript;
        private readonly IInvokableSymbol? _invokable;
        private readonly Stack<(BoundLabel BreakLabel, BoundLabel ContinueLabel)> _loopStack = new Stack<(BoundLabel BreakLabel, BoundLabel ContinueLabel)>();
        private int _labelCounter;
        private IScope _scope;
        private readonly Dictionary<IInvokableSymbol, (TextLocation, IScope, SyntaxNode)> _stuff = new Dictionary<IInvokableSymbol, (TextLocation, IScope, SyntaxNode)>();

        public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

        public Binder(bool isScript, IScope parent, IInvokableSymbol? invokable)
        {
            _isScript = isScript;
            _invokable = invokable;
            _scope = new BoundScope(parent);

            if (_invokable != null)
            {
                foreach (var p in _invokable.Parameters)
                {
                    _scope.TryDeclare(p);
                }
            }
        }

        public static BoundGlobalScope BindGlobalScope(bool isScript, BoundGlobalScope previous, ImmutableArray<SyntaxTree> syntaxTrees)
        {
            var parent = CreateParentScope(previous);
            var binder = new Binder(isScript, parent, invokable: null);

            var nodes = syntaxTrees.SelectMany(st => st.Root.Members).ToImmutableArray();

            // 1. bind types
            // create types
            binder.BindTypeDeclarations(nodes);

            // 2. bind members
            // create members
            binder.BindMemberDeclarations(nodes);

            // 2.5 create script or main function from global statements
            binder.GetEntryPoint(isScript, syntaxTrees, out FunctionSymbol? mainFunction,
                out FunctionSymbol? scriptFunction, out var gbs);

            var functionBodies = ImmutableDictionary.CreateBuilder<IInvokableSymbol, BoundBlockStatement>();
            var diagnostics2 = ImmutableArray.CreateBuilder<Diagnostic>();

            // 3. bind statements
            foreach (var kv in binder._stuff)
            {
                var (location, scope, body) = kv.Value;
                var function = kv.Key;

                var binder2 = new Binder(isScript, scope, function);

                BoundBlockStatement s;
                if (body is ExpressionBodySyntax e)
                {
                    var expr = binder.BindExpression(e.Expression, function.Type);
                    var r = new BoundReturnStatement(expr);
                    s = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(r));
                }
                else if (body is BlockStatementSyntax b)
                {
                    s = (BoundBlockStatement)binder2.BindStatement(b);
                }
                else
                {
                    throw new InvalidOperationException();
                }

                var loweredBody = Lowerer.Lower(s);

                if (function.Type != TypeSymbol.Void && !ControlFlowGraph.AllPathsReturn(loweredBody))
                    binder.Diagnostics.ReportAllPathsMustReturn(location);

                functionBodies.Add(function, loweredBody);

                diagnostics2.AddRange(binder.Diagnostics);
            }

            if (mainFunction != null && gbs.Any())
            {
                var body = Lowerer.Lower(new BoundBlockStatement(gbs));
                functionBodies.Add(mainFunction, body);
            }
            else if (scriptFunction != null)
            {
                var statements = gbs;
                if (statements.Length == 1 &&
                    statements[0] is BoundExpressionStatement es &&
                    es.Expression.Type != TypeSymbol.Void)
                {
                    statements = statements.SetItem(0, new BoundReturnStatement(es.Expression));
                }
                else if (statements.Any() && !(statements.Last() is BoundReturnStatement))
                {
                    var nullValue = new BoundLiteralExpression(TypeSymbol.String, "");
                    statements = statements.Add(new BoundReturnStatement(nullValue));
                }

                var body = Lowerer.Lower(new BoundBlockStatement(statements));
                functionBodies.Add(scriptFunction, body);
            }

            var diagnostics = binder.Diagnostics.ToImmutableArray();

            if (previous != null)
                diagnostics = diagnostics.InsertRange(0, previous.Diagnostics);

            // TODO concat diagnostics2

            return new BoundGlobalScope(previous, diagnostics, mainFunction, scriptFunction, binder._scope.GetDeclaredSymbols(), functionBodies.ToImmutable());
        }

        private void GetEntryPoint(
            bool isScript,
            ImmutableArray<SyntaxTree> syntaxTrees,
            out FunctionSymbol? mainFunction,
            out FunctionSymbol? scriptFunction,
            out ImmutableArray<BoundStatement> statements)
        {
            var globalStatements = syntaxTrees
                .SelectMany(st => st.Root.Members)
                .OfType<GlobalStatementSyntax>()
                .ToList();

            var statementsb = ImmutableArray.CreateBuilder<BoundStatement>();

            foreach (var globalStatement in globalStatements)
            {
                var statement = BindGlobalStatement(globalStatement);
                statementsb.Add(statement);
            }

            statements = statementsb.ToImmutable();

            // Check global statements

            var firstGlobalStatementPerSyntaxTree = syntaxTrees
                .Select(st => st.Root.Members.OfType<GlobalStatementSyntax>().FirstOrDefault())
                .Where(g => g != null)
                .ToArray();

            if (firstGlobalStatementPerSyntaxTree.Length > 1)
            {
                foreach (var globalStatement in firstGlobalStatementPerSyntaxTree)
                    Diagnostics.ReportOnlyOneFileCanHaveGlobalStatements(globalStatement.Location);
            }

            // Check for main/script with global statements

            var functions = _scope
                .GetDeclaredSymbols()
                .OfType<FunctionSymbol>();

            if (isScript)
            {
                mainFunction = null;
                scriptFunction = globalStatements.Any() ? new FunctionSymbol("$Eval", ImmutableArray<ParameterSymbol>.Empty, TypeSymbol.Any) : null;
            }
            else
            {
                mainFunction = functions.FirstOrDefault(f => f.Name == "Main");
                scriptFunction = null;

                if (mainFunction != null)
                {
                    if (mainFunction.Type != TypeSymbol.Void || mainFunction.Parameters.Any())
                    {
                        var (location, _, _) = _stuff[mainFunction];
                        Diagnostics.ReportMainMustHaveCorrectSignature(location);
                    }
                }

                if (globalStatements.Any())
                {
                    if (mainFunction != null)
                    {
                        var (location, _, _) = _stuff[mainFunction];
                        Diagnostics.ReportCannotMixMainAndGlobalStatements(location);

                        foreach (var globalStatement in firstGlobalStatementPerSyntaxTree)
                            Diagnostics.ReportCannotMixMainAndGlobalStatements(globalStatement.Location);
                    }
                    else
                    {
                        mainFunction = new FunctionSymbol("Main", ImmutableArray<ParameterSymbol>.Empty, TypeSymbol.Void);
                    }
                }
            }
        }

        private BoundStatement BindGlobalStatement(GlobalStatementSyntax syntax)
        {
            return BindStatement(syntax.Statement, isGlobal: true);
        }

        public static BoundProgram BindProgram(bool isScript, BoundProgram previous, BoundGlobalScope globalScope)
        {
            //    var parentScope = CreateParentScope(globalScope);

            //    var functionBodies = ImmutableDictionary.CreateBuilder<IInvokableSymbol, BoundBlockStatement>();
            var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();

            //    foreach (var function in globalScope.Symbols.OfType<IInvokableSymbol>())
            //    {
            //        if (function.Extern) continue;

            //        var binder = new Binder(isScript, parentScope, function);
            //        var body = binder.BindStatement(function.Declaration.Body);
            //        var loweredBody = Lowerer.Lower(body);

            //        if (function.Type != TypeSymbol.Void && !ControlFlowGraph.AllPathsReturn(loweredBody))
            //            binder.Diagnostics.ReportAllPathsMustReturn(function.Declaration.IdentifierToken.Location);

            //        functionBodies.Add(function, loweredBody);

            //        diagnostics.AddRange(binder.Diagnostics);
            //    }

            //    if (globalScope.MainFunction != null && globalScope.Statements.Any())
            //    {
            //        var body = Lowerer.Lower(new BoundBlockStatement(globalScope.Statements));
            //        functionBodies.Add(globalScope.MainFunction, body);
            //    }
            //    else if (globalScope.ScriptFunction != null)
            //    {
            //        var statements = globalScope.Statements;
            //        if (statements.Length == 1 &&
            //            statements[0] is BoundExpressionStatement es &&
            //            es.Expression.Type != TypeSymbol.Void)
            //        {
            //            statements = statements.SetItem(0, new BoundReturnStatement(es.Expression));
            //        }
            //        else if (statements.Any() && !(statements.Last() is BoundReturnStatement))
            //        {
            //            var nullValue = new BoundLiteralExpression(TypeSymbol.String, "");
            //            statements = statements.Add(new BoundReturnStatement(nullValue));
            //        }

            //        var body = Lowerer.Lower(new BoundBlockStatement(statements));
            //        functionBodies.Add(globalScope.ScriptFunction, body);
            //    }

            return new BoundProgram(previous,
                diagnostics.ToImmutable(),
                globalScope.MainFunction,
                globalScope.ScriptFunction,
                globalScope.FunctionBodies);
            //functionBodies.ToImmutable());
        }

        private void BindMemberDeclarations(ImmutableArray<MemberSyntax> nodes)
        {
            var functions = nodes.OfType<FunctionDeclarationSyntax>();
            foreach (var function in functions)
            {
                BindFunctionDeclaration(function);
            }

            var externals = nodes.OfType<ExternDeclarationSyntax>();
            foreach (var external in externals)
            {
                BindExternDeclaration(external);
            }

            var objects = nodes.OfType<ObjectDeclarationSyntax>();
            foreach (var @object in objects)
            {
                BindTypeMemberDeclarations(@object);
            }
        }

        private void BindTypeMemberDeclarations(ObjectDeclarationSyntax syntax)
        {
            // TODO move this to the type binding phase
            // 
            var symbol = (TypeSymbol)GetSymbol(new[] { SymbolKind.Type }, syntax.IdentifierToken);

            if (syntax.BaseType != null)
            {
                var baseType = LookupBaseType(syntax.BaseType);

                // check for cyclic dependency
                if (IsCyclicDependency(baseType, symbol))
                {
                    Diagnostics.ReportCyclicDependency(syntax.BaseType.Location);
                }

                symbol.BaseType = baseType;
            }


            // start binding members
            _scope = new TypeScope(_scope, symbol);

            foreach (var member in syntax.Members)
            {
                BindTypeMemberDeclaration(member);
            }

            var hasConstructor = symbol.Members.OfType<ConstructorSymbol>().Any();
            if (!hasConstructor)
            {
                var constructorSymbol = new ConstructorSymbol(symbol, ImmutableArray<ParameterSymbol>.Empty);
                DeclareSymbol(constructorSymbol, syntax.IdentifierToken);
            }

            _scope = _scope.Parent;
        }



        private void BindTypeMemberDeclaration(MemberDeclarationSyntax syntax)
        {
            switch (syntax)
            {
                case FieldDeclarationSyntax f:
                    BindFieldDeclaration(f);
                    break;
                case MethodDeclarationSyntax m:
                    BindMethodDeclaration(m);
                    break;
                case ConstructorDeclarationSyntax c:
                    BindConstructorDeclaration(c);
                    break;
                case PropertyDeclarationSyntax p:
                    BindPropertyDeclaration(p);
                    break;
                case IndexerDeclarationSyntax i:
                    BindIndexerDeclaration(i);
                    break;
                default:
                    throw new Exception($"Unexpected syntax {syntax.GetType()}");
            }
        }

        private void BindIndexerDeclaration(IndexerDeclarationSyntax syntax)
        {
            var declaringType = ((TypeScope)_scope).Type;

            var parameters = LookupParameterList(syntax.Parameters);

            var type = BindTypeClause(syntax.TypeClause);
            MethodSymbol? getter = null;
            MethodSymbol? setter = null;

            var name = "Item";
            if (syntax.Body is ExpressionBodySyntax e)
            {
                getter = new MethodSymbol(declaringType, type, "<>Get_" + name, parameters);
                _stuff[getter] = (syntax.Location, _scope, e);
            }
            else
            {
                var body = (PropertyBodySyntax)syntax.Body;

                if (body.GetterClause != null)
                {
                    getter = new MethodSymbol(declaringType, type, "<>Get_" + name, parameters);
                    _stuff[getter] = (body.GetterClause.GetKeyword.Location, _scope, body.GetterClause.Body);
                }

                if (body.SetterClause != null)
                {
                    setter = new MethodSymbol(declaringType, TypeSymbol.Void, "<>Set_" + name, parameters.Add(new ParameterSymbol("value", type, 0)));
                    _stuff[setter] = (body.SetterClause.SetKeyword.Location, _scope, body.SetterClause.Body);
                }
            }

            var property = new IndexerSymbol(declaringType, type, parameters, getter, setter);
            DeclareSymbol(property, syntax.Location, name);
        }

        private void BindPropertyDeclaration(PropertyDeclarationSyntax syntax)
        {
            var declaringType = ((TypeScope)_scope).Type;
            TypeSymbol type = null;
            if (syntax.TypeClause != null)
                type = BindTypeClause(syntax.TypeClause);
            MethodSymbol getter = null;
            MethodSymbol setter = null;

            if (syntax.Body is ExpressionBodySyntax e)
            {
                getter = new MethodSymbol(declaringType, type, "<>Get_" + syntax.IdentifierToken.Text, ImmutableArray<ParameterSymbol>.Empty);
                _stuff[getter] = (syntax.IdentifierToken.Location, _scope, e);
            }
            else
            {
                var body = (PropertyBodySyntax)syntax.Body;

                if (body.GetterClause != null)
                {
                    getter = new MethodSymbol(declaringType, type, "<>Get_" + syntax.IdentifierToken.Text, ImmutableArray<ParameterSymbol>.Empty);
                    _stuff[getter] = (body.GetterClause.GetKeyword.Location, _scope, body.GetterClause.Body);
                }

                if (body.SetterClause != null)
                {
                    setter = new MethodSymbol(declaringType, TypeSymbol.Void, "<>Set_" + syntax.IdentifierToken.Text, ImmutableArray.Create<ParameterSymbol>(new ParameterSymbol("value", type, 0)));
                    _stuff[setter] = (body.SetterClause.SetKeyword.Location, _scope, body.SetterClause.Body);
                }
            }

            var property = new PropertySymbol(declaringType, syntax.IdentifierToken.Text, type, getter, setter);
            DeclareSymbol(property, syntax.IdentifierToken);
        }

        private void BindConstructorDeclaration(ConstructorDeclarationSyntax syntax)
        {
            var declaringType = ((TypeScope)_scope).Type;
            var parameters = LookupParameterList(syntax.Parameters);
            var constructor = new ConstructorSymbol(declaringType, parameters);
            DeclareSymbol(constructor, syntax.IdentifierToken);
        }

        private void BindMethodDeclaration(MethodDeclarationSyntax syntax)
        {
            var declaringType = ((TypeScope)_scope).Type;

            var parameters = LookupParameterList(syntax.Parameters);

            var type = BindTypeClause(syntax.TypeClause) ?? TypeSymbol.Void;

            var name = syntax.IdentifierToken.Text;

            var method = new MethodSymbol(declaringType, type, name, parameters);

            _stuff[method] = (syntax.IdentifierToken.Location, _scope, syntax.Body);

            DeclareSymbol(method, syntax.IdentifierToken);
        }

        private void BindFunctionDeclaration(FunctionDeclarationSyntax syntax)
        {
            var parameters = LookupParameterList(syntax.Parameters);

            var type = BindTypeClause(syntax.Type) ?? TypeSymbol.Void;

            var name = syntax.IdentifierToken.Text;

            var function = new FunctionSymbol(name, parameters, type);
            var location = syntax.IdentifierToken.Location;
            var body = syntax.Body;

            _stuff[function] = (location, _scope, body);

            DeclareSymbol(function, syntax.IdentifierToken);
        }

        private void BindFieldDeclaration(FieldDeclarationSyntax syntax)
        {
            var declaringType = ((TypeScope)_scope).Type;
            var type = BindTypeClause(syntax.TypeClause);
            var index = declaringType.Members.OfType<FieldSymbol>().Count();
            var field = new FieldSymbol(declaringType, syntax.IdentifierToken.Text, type, index);
            DeclareSymbol(field, syntax.IdentifierToken);
        }

        private void BindTypeDeclarations(ImmutableArray<MemberSyntax> nodes)
        {
            // TODO find all types that are to be declared
            // ... depth search

            // then loop until all are defined
            // or the list does not get shorter -> cyclic dependency

            var classes = nodes.OfType<ObjectDeclarationSyntax>();
            var types = classes;

            foreach (var type in types)
            {
                BindType(type);
            }

            var aliases = nodes.OfType<AliasDeclarationSyntax>();
            foreach (var alias in aliases)
            {
                BindAlias(alias);
            }
        }

        private void BindAlias(AliasDeclarationSyntax alias)
        {
            var identifierToken = alias.IdentifierToken;
            var symbol = new AliasSymbol(alias.IdentifierToken.Text, null);
            DeclareSymbol(symbol, identifierToken);
        }

        // TODO make this try declare for class and struct
        // if cant be declared remember for retry
        // repeat until all types are declared
        // or set of retries does not reduce in size between two iterations
        // -> must be cyclic dependency or undefined type
        private void BindType(SyntaxNode type)
        {
            TypeSymbol symbol;
            Token identifierToken;
            switch (type)
            {
                case ObjectDeclarationSyntax c:
                    identifierToken = c.IdentifierToken;
                    var typeName = identifierToken.Text;

                    // if is built in type we don't declare a new type symbol
                    if (typeName == "String")
                    {
                        symbol = TypeSymbol.String;
                        break;
                    }

                    symbol = new TypeSymbol(typeName);
                    break;
                default:
                    throw new Exception($"Unexpected syntax {type.GetType()}");
            }
            DeclareSymbol(symbol, identifierToken);

            //switch (symbol.Name)
            //{
            //    // TODO should be in namespace std
            //    case "Void": TypeSymbol.Void = symbol; break;
            //    case "Boolean": TypeSymbol.Bool = symbol; break;
            //    case "Int8": TypeSymbol.I8 = symbol; break;
            //    case "Int16": TypeSymbol.I16 = symbol; break;
            //    case "Int32": TypeSymbol.I32 = symbol; break;
            //    case "Int64":
            //        TypeSymbol.I64 = symbol;
            //        TypeSymbol.Int = symbol;
            //        break;
            //    case "UInt8": TypeSymbol.U8 = symbol; break;
            //    case "UInt16": TypeSymbol.U16 = symbol; break;
            //    case "UInt32": TypeSymbol.U32 = symbol; break;
            //    case "UInt64":
            //        TypeSymbol.U64 = symbol;
            //        TypeSymbol.UInt = symbol;
            //        break;
            //    case "String": TypeSymbol.String = symbol; break;
            //    default:
            //        break;
            //}
        }

        private static BoundScope CreateParentScope(BoundGlobalScope previous)
        {
            var stack = new Stack<BoundGlobalScope>();
            while (previous != null)
            {
                stack.Push(previous);
                previous = previous.Previous;
            }

            var parent = CreateRootScope();

            while (stack.Count > 0)
            {
                previous = stack.Pop();
                var scope = new BoundScope(parent);
                foreach (var symbol in previous.Symbols)
                {
                    scope.TryDeclare(symbol);
                }

                parent = scope;
            }

            return parent;
        }

        private static BoundScope CreateRootScope()
        {
            var result = new BoundScope(null);

            result.TryDeclare(TypeSymbol.Bool);
            result.TryDeclare(TypeSymbol.String);
            result.TryDeclare(TypeSymbol.Char);
            result.TryDeclare(TypeSymbol.Void);
            result.TryDeclare(TypeSymbol.I8);
            result.TryDeclare(TypeSymbol.I16);
            result.TryDeclare(TypeSymbol.I32);
            result.TryDeclare(TypeSymbol.I64);
            result.TryDeclare(TypeSymbol.Int);
            result.TryDeclare(TypeSymbol.U8);
            result.TryDeclare(TypeSymbol.U16);
            result.TryDeclare(TypeSymbol.U32);
            result.TryDeclare(TypeSymbol.U64);
            result.TryDeclare(TypeSymbol.UInt);
            result.TryDeclare(TypeSymbol.Any);

            //foreach (var f in BuiltinFunctions.GetAll())
            //    result.TryDeclareFunction(f);

            return result;
        }

        //public BoundNode BindNode(SyntaxNode node)
        //{
        //    switch (node)
        //    {
        //        case StatementSyntax s:
        //            return BindStatement(s);
        //        case AliasDeclarationSyntax a:
        //            return BindAliasDeclaration(a);
        //        case ExternDeclarationSyntax e:
        //            return BindExternDeclaration(e);
        //        case FunctionDeclarationSyntax f:
        //            return BindFunctionDeclaration(f);
        //        case StructDeclarationSyntax s:
        //            return BindStructDeclaration(s);
        //        case ClassDeclarationSyntax c:
        //            return BindClassDeclaration(c);
        //        case ConstDeclarationSyntax c:
        //            return BindConstDeclaration(c);
        //        default:
        //            throw new Exception($"Unsupported node {node}");
        //    }
        //}

        private BoundNode BindConstDeclaration(ConstDeclarationSyntax syntax)
        {
            var initializer = BindExpression(syntax.Initializer);
            if (!TryEvalConstExpression(initializer, out var type, out var value))
            {
                Diagnostics.ReportExpressionIsNotCompileTimeConstant(syntax.Initializer.Location);
                type = TypeSymbol.Int;
                value = 0;
            }

            if (syntax.TypeClause != null)
            {
                type = BindTypeClause(syntax.TypeClause);
            }

            var identifierToken = syntax.IdentifierToken;
            var constSymbol = new ConstSymbol(identifierToken.Text, type);
            DeclareSymbol(constSymbol, identifierToken);
            return new BoundConstDeclaration(constSymbol, value);
        }

        private bool TryEvalConstExpression(BoundExpression expression, out TypeSymbol type, out object value)
        {
            type = null;
            value = null;
            switch (expression)
            {
                case BoundConversionExpression c:
                    if (!TryEvalConstExpression(c.Expression, out var t1, out var v1)) return false;
                    type = c.Type;
                    value = Convert.ChangeType(v1, type.GetClrType());
                    return true;
                case BoundUnaryExpression u:
                    if (!TryEvalConstExpression(u.Operand, out var t2, out var v2)) return false;
                    type = t2;
                    var @delegate = EvalOperators.UnaryOperators[u.Operator];
                    //switch (u.Operator.Kind)
                    //{
                    //    case BoundUnaryOperatorKind.Identity:
                    //        value = +(int)v2;
                    //        break;
                    //    case BoundUnaryOperatorKind.Negation:
                    //        value = -(int)v2;
                    //        break;
                    //    case BoundUnaryOperatorKind.LogicalNot:
                    //        value = !(bool)v2;
                    //        break;
                    //    case BoundUnaryOperatorKind.BitwiseComplement:
                    //        value = ~(int)v2;
                    //        break;
                    //}
                    value = @delegate.DynamicInvoke(v2);
                    return true;
                case BoundBinaryExpression b:
                    if (!TryEvalConstExpression(b.Left, out var lt, out var lv)) return false;
                    if (!TryEvalConstExpression(b.Right, out var rt, out var rv)) return false;
                    var binaryOperator = EvalOperators.BinaryOperators[b.Operator];
                    type = b.Operator.ResultType;
                    value = binaryOperator.DynamicInvoke(lv, rv);
                    return true;
                case BoundLiteralExpression l:
                    type = l.Type;
                    value = l.Value;
                    return true;
                default:
                    return false;
            }
        }

        //private BoundNode BindAliasDeclaration(AliasDeclarationSyntax syntax)
        //{
        //    var typeSymbol = LookupType(syntax.Type);
        //    //var identifierToken = syntax.IdentifierToken;
        //    var aliasSymbol = GetSymbol<AliasSymbol>(syntax.IdentifierToken);
        //    aliasSymbol.Type = typeSymbol;
        //    aliasSymbol.Lock();
        //    //var aliasSymbol = new AliasSymbol(identifierToken.Text, typeSymbol);
        //    //DeclareSymbol(aliasSymbol, identifierToken);
        //    return new BoundAliasDeclaration(aliasSymbol);
        //}

        private BoundStatement BindStatement(StatementSyntax syntax, bool isGlobal = false)
        {
            var result = BindStatementInternal(syntax);

            if (!_isScript || !isGlobal)
            {
                if (result is BoundExpressionStatement es)
                {
                    var isAllowedExpression = es.Expression is BoundErrorExpression ||
                                              es.Expression is BoundAssignmentExpression ||
                                              es.Expression is BoundFunctionCallExpression ||
                                              es.Expression is BoundMethodCallExpression ||
                                              es.Expression is BoundConstructorCallExpression ||
                                              es.Expression is BoundNewInstanceExpression;
                    if (!isAllowedExpression)
                        Diagnostics.ReportInvalidExpressionStatement(syntax.Location);
                }
            }

            return result;
        }

        public BoundStatement BindStatementInternal(StatementSyntax stmt)
        {
            switch (stmt)
            {
                case BlockStatementSyntax b:
                    return BindBlockStatement(b);
                case VariableDeclarationSyntax v:
                    return BindVariableDeclaration(v);
                case IfStatementSyntax i:
                    return BindIfStatement(i);
                case LoopStatementSyntax l:
                    return BindLoopStatement(l);
                case WhileStatementSyntax w:
                    return BindWhileStatement(w);
                case BreakStatementSyntax b:
                    return BindBreakStatement(b);
                case ContinueStatementSyntax c:
                    return BindContinueStatement(c);
                case ForStatementSyntax f:
                    return BindForStatement(f);
                case ExpressionStatementSyntax e:
                    return BindExpressionStatement(e);
                case ReturnStatementSyntax r:
                    return BindReturnStatement(r);
                default:
                    throw new Exception($"Unexpected syntax {stmt.GetType()}");
            }
        }

        private BoundStatement BindReturnStatement(ReturnStatementSyntax syntax)
        {
            var expression = syntax.Expression == null ? null : BindExpression(syntax.Expression);

            if (_invokable == null)
            {
                if (_isScript)
                {
                    // Ignore because we allow both return with and without values.
                    if (expression == null)
                        expression = new BoundLiteralExpression(TypeSymbol.String, "");
                }
                else if (expression != null)
                {
                    // Main does not support return values.
                    Diagnostics.ReportInvalidReturnExpression(syntax.Expression.Location, _invokable.Name);
                }
            }
            else
            {
                if (_invokable.Type == TypeSymbol.Void)
                {
                    if (expression != null)
                        Diagnostics.ReportInvalidReturnExpression(syntax.Expression.Location, _invokable.Name);
                }
                else
                {
                    if (expression == null)
                        Diagnostics.ReportMissingReturnExpression(syntax.ReturnKeyword.Location, _invokable.Type);
                    else
                        expression = BindConversion(syntax.Expression.Location, expression, _invokable.Type);
                }
            }

            return new BoundReturnStatement(expression);
        }

        //private BoundClassDeclaration BindClassDeclaration(ObjectDeclarationSyntax syntax)
        //{
        //    var symbol = GetSymbol<TypeSymbol>(syntax.IdentifierToken);

        //    _scope = new TypeScope(_scope, symbol);

        //    var members = syntax.Members.Select(BindMemberDeclaration).ToList();
        //    _scope = _scope.Parent;

        //    return new BoundClassDeclaration(symbol, members.ToImmutableArray());
        //}

        private bool IsCyclicDependency(ImmutableArray<TypeSymbol> baseType, TypeSymbol symbol)
        {
            return false;
        }

        private ImmutableArray<TypeSymbol> LookupBaseType(BaseTypeSyntax syntax)
        {
            return syntax.Types.OfType<TypeSyntax>().Select(LookupType).ToImmutableArray();
        }

        //private BoundMemberDeclaration BindMemberDeclaration(MemberDeclarationSyntax syntax)
        //{
        //    switch (syntax)
        //    {
        //        case FieldDeclarationSyntax f:
        //            return BindFieldDeclaration(f);
        //        case MethodDeclarationSyntax m:
        //            return BindMethodDeclaration(m);
        //        case ConstructorDeclarationSyntax c:
        //            return BindConstructorDeclaration(c);
        //        case PropertyDeclarationSyntax p:
        //            return BindPropertyDeclaration(p);
        //        default:
        //            throw new Exception($"Unexpected node {syntax.GetType()}");
        //    }
        //}

        //private BoundMemberDeclaration BindPropertyDeclaration(PropertyDeclarationSyntax syntax)
        //{
        //    var property = GetSymbol<PropertySymbol>(syntax.IdentifierToken);


        //    if (syntax.ExpressionBody != null)
        //    {
        //        //_scope = new BoundScope(_scope);
        //        var expression = BindExpression(syntax.ExpressionBody.Expression);
        //        //_scope = _scope.Parent;
        //        var expressionStatement = new BoundExpressionStatement(expression);
        //        var boundStatements = ImmutableArray.Create<BoundStatement>(expressionStatement);
        //        var boundBlockStatement = new BoundBlockStatement(boundStatements);
        //        return new BoundPropertyDeclaration(property, boundBlockStatement);
        //    }


        //    //_scope = new BoundScope(_scope);

        //    return null;
        //}

        //private BoundMemberDeclaration BindConstructorDeclaration(ConstructorDeclarationSyntax syntax)
        //{
        //    var constructor = GetSymbol<ConstructorSymbol>(syntax.IdentifierToken);

        //    _scope = new BoundScope(_scope);

        //    var parameterList = syntax.ParameterList;
        //    var parameterSymbols = constructor.Parameters;

        //    DeclareParameters(parameterList, parameterSymbols);
        //    var body = BindBlockStatement(syntax.Body);

        //    _scope = _scope.Parent;

        //    return new BoundConstructorDeclaration(constructor, body);
        //}

        //private BoundMemberDeclaration BindMethodDeclaration(MethodDeclarationSyntax syntax)
        //{
        //    var method = GetSymbol<MethodSymbol>(syntax.IdentifierToken);
        //    _scope = new BoundScope(_scope);

        //    var parameterList = syntax.Prototype.ParameterList;
        //    var parameterSymbols = method.Parameters;

        //    DeclareParameters(parameterList, parameterSymbols);

        //    var body = BindBlockStatement(syntax.Body);
        //    _scope = _scope.Parent;

        //    return new BoundMethodDeclaration(method, body);
        //}

        //private BoundMemberDeclaration BindFieldDeclaration(FieldDeclarationSyntax syntax)
        //{
        //    var field = GetSymbol<FieldSymbol>(syntax.IdentifierToken);

        //    BoundExpression initializer = null;
        //    if (syntax.Initializer != null)
        //    {
        //        initializer = BindExpression(syntax.Initializer.Expression);
        //    }

        //    TypeSymbol type = null;
        //    if (syntax.TypeClause != null)
        //    {
        //        type = BindTypeClause(syntax.TypeClause);
        //    }
        //    else if (initializer == null)
        //    {
        //        Diagnostics.ReportMemberMustBeTyped(syntax.IdentifierToken.Location);
        //        type = TypeSymbol.Int;
        //    }
        //    else
        //    {
        //        type = initializer.Type;
        //    }


        //    return new BoundFieldDeclaration(field, initializer);
        //}

        //private BoundFunctionDeclaration BindFunctionDeclaration(FunctionDeclarationSyntax syntax)
        //{
        //    var function = GetSymbol<FunctionSymbol>(syntax.Prototype.IdentifierToken);
        //    _scope = new BoundScope(_scope);

        //    var parameterList = syntax.Prototype.ParameterList;
        //    var parameterSymbols = function.Parameters;

        //    DeclareParameters(parameterList, parameterSymbols);

        //    var body = BindStatement(syntax.Body);
        //    _scope = _scope.Parent;

        //    return new BoundFunctionDeclaration(function, body);
        //}

        //private void DeclareParameters(ParameterListSyntax parameterList, ImmutableArray<ParameterSymbol> parameterSymbols)
        //{
        //    var parameters = parameterList.Parameters.OfType<ParameterSyntax>().ToArray();
        //    for (var i = 0; i < parameters.Length; i++)
        //    {
        //        var parameter = parameters[i];
        //        var symbol = parameterSymbols[i];
        //        DeclareSymbol(symbol, parameter.IdentifierToken);
        //    }
        //}

        private ImmutableArray<ParameterSymbol> LookupParameterList(SeparatedSyntaxList<ParameterSyntax> syntax)
        {
            var parameters = ImmutableArray.CreateBuilder<ParameterSymbol>();
            var seenParameterNames = new HashSet<string>();

            var index = 0;
            foreach (var parameterSyntax in syntax)
            {
                var parameterName = parameterSyntax.IdentifierToken.Text;
                var parameterType = BindTypeClause(parameterSyntax.Type);
                if (!seenParameterNames.Add(parameterName))
                {
                    Diagnostics.ReportParameterAlreadyDeclared(parameterSyntax.Location, parameterName);
                }
                else
                {
                    var parameter = new ParameterSymbol(parameterName, parameterType, index++);
                    parameters.Add(parameter);
                }
            }

            return parameters.ToImmutable();
        }

        [return: NotNullIfNotNull("syntax")]
        private TypeSymbol? BindTypeClause(TypeClauseSyntax? syntax)
        {
            if (syntax == null)
                return null;

            var type = LookupType(syntax.Type);
            if (type == null)
            {
                Diagnostics.ReportUndefinedType(syntax.Type.Location, syntax.Type.ToString());
                return TypeSymbol.Error;
            }

            return type;
        }

        private TypeSymbol? LookupType(SyntaxNode syntax)
        {
            if (syntax is ReferenceTypeSyntax r)
            {
                return LookupType(r.Type)?.MakeReference();
            }

            if (syntax is PointerTypeSyntax p)
            {
                return LookupType(p.Type)?.MakePointer();
            }

            if (syntax is TypeSyntax t)
            {
                var typeIdentifierToken = t.TypeOrIdentifierToken;
                var type = GetSymbol(new[] { SymbolKind.Alias, SymbolKind.Type }, typeIdentifierToken) ?? TypeSymbol.Int;

                while (type is AliasSymbol a)
                    type = a.Type;

                return (TypeSymbol?)type;
            }
            throw new InvalidOperationException($"Unexpected syntax node '{syntax.GetType()}'.");
        }

        private void BindExternDeclaration(ExternDeclarationSyntax syntax)
        {
            var parameters = LookupParameterList(syntax.Parameters);
            var type = BindTypeClause(syntax.Type) ?? TypeSymbol.Void;
            var name = syntax.IdentifierToken.Text;

            var function = new FunctionSymbol(name, parameters, type, @extern: true);
            DeclareSymbol(function, syntax.IdentifierToken);
        }

        private BoundStatement BindWhileStatement(WhileStatementSyntax syntax)
        {
            var condition = BindExpression(syntax.Condition, TypeSymbol.Bool);
            var body = BindLoopBody(syntax.Body, out var breakLabel, out var continueLabel);
            return new BoundWhileStatement(condition, body, breakLabel, continueLabel);
        }

        private BoundStatement BindLoopStatement(LoopStatementSyntax syntax)
        {
            var body = BindLoopBody(syntax.Body, out var breakLabel, out var continueLabel);
            return new BoundLoopStatement(body, breakLabel, continueLabel);
        }

        private BoundStatement BindForStatement(ForStatementSyntax syntax)
        {
            var lowerBound = BindExpression(syntax.LowerBound, TypeSymbol.Int);
            var upperBound = BindExpression(syntax.UpperBound, TypeSymbol.Int);

            _scope = new BoundScope(_scope);

            var variable = BindVariableDeclaration(syntax.IdentifierToken, /*code in body should not allow change*/false, lowerBound.Type);
            //var variable = new VariableSymbol(syntax.IdentifierToken.Text, false, lowerBound.Type);
            var body = BindLoopBody(syntax.Body, out var breakLabel, out var continueLabel);

            _scope = _scope.Parent;

            return new BoundForStatement(variable, lowerBound, upperBound, body, breakLabel, continueLabel);
        }

        private BoundStatement BindLoopBody(StatementSyntax body, out BoundLabel breakLabel, out BoundLabel continueLabel)
        {
            _labelCounter++;
            breakLabel = new BoundLabel($"break{_labelCounter}");
            continueLabel = new BoundLabel($"continue{_labelCounter}");

            _loopStack.Push((breakLabel, continueLabel));
            var boundBody = BindStatement(body);
            _loopStack.Pop();

            return boundBody;
        }

        private BoundStatement BindContinueStatement(ContinueStatementSyntax syntax)
        {
            if (_loopStack.Count == 0)
            {
                Diagnostics.ReportInvalidBreakOrContinue(syntax.ContinueKeyword.Location, syntax.ContinueKeyword.Text);
                return BindErrorStatement();
            }

            var continueLabel = _loopStack.Peek().ContinueLabel;
            return new BoundGotoStatement(continueLabel);
        }

        private BoundStatement BindErrorStatement()
        {
            return new BoundExpressionStatement(new BoundErrorExpression());
        }

        private BoundStatement BindBreakStatement(BreakStatementSyntax syntax)
        {
            if (_loopStack.Count == 0)
            {
                Diagnostics.ReportInvalidBreakOrContinue(syntax.BreakKeyword.Location, syntax.BreakKeyword.Text);
                return BindErrorStatement();
            }

            var breakLabel = _loopStack.Peek().BreakLabel;
            return new BoundGotoStatement(breakLabel);
        }

        private BoundStatement BindIfStatement(IfStatementSyntax syntax)
        {
            var condition = BindExpression(syntax.Condition, TypeSymbol.Bool);

            var thenStatement = BindStatement(syntax.ThenStatement);

            // Example for ?: operator -> if null its null of the right expression.type
            // var elseStatement = syntax.ElseClause ?: BindStatement(syntax.ElseClause.ElseStatement);
            var elseStatement = syntax.ElseClause == null ? null : BindStatement(syntax.ElseClause.ElseStatement);
            return new BoundIfStatement(condition, thenStatement, elseStatement);
        }

        private BoundStatement BindVariableDeclaration(VariableDeclarationSyntax syntax)
        {
            var isReadOnly = syntax.Keyword.Kind == TokenKind.LetKeyword;
            var type = BindTypeClause(syntax.TypeClause);
            var initializer = BindExpression(syntax.Initializer);
            var variableType = type ?? initializer.Type;
            var variable = BindVariableDeclaration(syntax.IdentifierToken, isReadOnly, variableType);
            var convertedInitializer = BindConversion(syntax.Initializer.Location, initializer, variableType);

            return new BoundVariableDeclaration(variable, convertedInitializer);
        }

        private VariableSymbol BindVariableDeclaration(Token identifier, bool isReadOnly, TypeSymbol type)
        {
            var name = identifier.Text ?? "?";
            var declare = !identifier.IsMissing;
            var variable = _invokable == null
                ? (VariableSymbol)new GlobalVariableSymbol(name, isReadOnly, type)
                : new LocalVariableSymbol(name, isReadOnly, type);

            if (declare && !_scope.TryDeclare(variable))
                Diagnostics.ReportSymbolAlreadyDeclared(identifier.Location, name);

            return variable;
        }

        private void DeclareSymbol(Symbol symbol, Token identifierToken)
        {
            DeclareSymbol(symbol, identifierToken.Location, identifierToken.Text);
        }

        private void DeclareSymbol(Symbol symbol, TextLocation location, string name)
        {
            if (!_scope.TryDeclare(symbol))
            {
                Diagnostics.ReportSymbolAlreadyDeclared(location, name);
            }
        }

        private BoundBlockStatement BindBlockStatement(BlockStatementSyntax syntax)
        {
            _scope = new BoundScope(_scope);

            var statements = ImmutableArray.CreateBuilder<BoundStatement>();
            foreach (var statementSyntax in syntax.Statements)
            {
                var statement = BindStatement(statementSyntax);
                statements.Add(statement);
            }

            _scope = _scope.Parent;

            return new BoundBlockStatement(statements.ToImmutable());
        }

        //private ImmutableArray<BoundNode> BindNodes(ImmutableArray<SyntaxNode> syntaxNodes)
        //{
        //    var nodes = ImmutableArray.CreateBuilder<BoundNode>();
        //    foreach (var syntaxNode in syntaxNodes)
        //    {
        //        var node = BindNode(syntaxNode);
        //        nodes.Add(node);
        //    }
        //    return nodes.ToImmutable();
        //}

        private BoundStatement BindExpressionStatement(ExpressionStatementSyntax syntax)
        {
            var expression = BindExpression(syntax.Expression, canBeVoid: true);
            return new BoundExpressionStatement(expression);
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax, TypeSymbol targetType)
        {
            return BindConversion(syntax, targetType);
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax, bool canBeVoid = false)
        {
            var result = BindExpressionInternal(syntax);

            if (result is BoundPropertyExpression p)
            {
                if (p.Property.Getter == null)
                {
                    Diagnostics.CannotReadSetOnlyProperty(syntax.Location);
                    return new BoundErrorExpression();
                }

                return new BoundMethodCallExpression(p.Target, p.Property.Getter, ImmutableArray<BoundExpression>.Empty);
            }

            if (result is BoundIndexerExpression i)
            {
                if (i.Indexer.Getter == null)
                {
                    Diagnostics.CannotReadSetOnlyIndexer(syntax.Location);
                    return new BoundErrorExpression();
                }

                return new BoundMethodCallExpression(i.Target, i.Indexer.Getter, i.Arguments);
            }

            if (!canBeVoid && result.Type == TypeSymbol.Void)
            {
                Diagnostics.ReportExpressionMustHaveValue(syntax.Location);
                return new BoundErrorExpression();
            }

            return result;
        }

        private BoundExpression BindExpressionInternal(ExpressionSyntax syntax, bool allowTypes = true)
        {
            switch (syntax)
            {
                case BinaryExpressionSyntax b:
                    return BindBinaryExpression(b);
                case UnaryExpressionSyntax u:
                    return BindUnaryExpression(u);
                case LiteralExpressionSyntax l:
                    return BindLiteralExpression(l);
                case AssignmentExpressionSyntax a:
                    return BindAssignmentExpression(a);
                case NameExpressionSyntax n:
                    return BindNameExpression(n, allowTypes);
                case ParenthesizedExpressionSyntax p:
                    return BindParenthesizedExpression(p, allowTypes);
                case InvokeExpressionSyntax i:
                    return BindInvokeExpression(i);
                case NewInstanceExpressionSyntax n:
                    return BindNewInstanceExpression(n);
                case NewArrayExpressionSyntax n:
                    return BindNewArrayExpression(n);
                case MemberAccessExpressionSyntax m:
                    return BindMemberAccessExpression(m);
                case CastExpressionSyntax c:
                    return BindCastExpression(c);
                case ThisExpressionSyntax t:
                    return BindThisExpression(t);
                case IndexExpressionSyntax i:
                    return BindIndexExpression(i);
                case SuffixCastExpressionSyntax s:
                    return BindSuffixCastExpression(s);
                default:
                    throw new Exception($"Unexpected syntax {syntax.GetType()}");
            }
        }

        private BoundExpression BindSuffixCastExpression(SuffixCastExpressionSyntax syntax)
        {
            var type = LookupType(syntax.Type);
            return BindConversion(syntax.Target, type, true);
        }

        private BoundExpression BindIndexExpression(IndexExpressionSyntax syntax)
        {
            var target = BindExpression(syntax.Target);

            var arguments = ImmutableArray.CreateBuilder<BoundExpression>();
            foreach (var argument in syntax.Arguments)
            {
                arguments.Add(BindExpression(argument));
            }

            if (target.Type.IsArray || target.Type.IsPointer)
            {
                // real indexer expression
                return new BoundArrayIndexExpression(target, arguments.ToImmutable());
            }
            else
            {
                var indexer = target.Type.Members.OfType<IndexerSymbol>().FirstOrDefault();
                if (indexer == null) return new BoundErrorExpression();

                return new BoundIndexerExpression(target, indexer, arguments.ToImmutable());
            }

            //TryBindArguments(syntax, "Item", syntax. )
            return new BoundErrorExpression();
        }

        private BoundExpression BindThisExpression(ThisExpressionSyntax syntax)
        {
            var s = _scope;
            while (s != null && !(s is TypeScope))
                s = s.Parent;

            if (s == null)
            {
                Diagnostics.ReportThisNotAllowed(syntax.Location);
                return new BoundErrorExpression();
            }

            var scope = (TypeScope)s;

            return new BoundThisExpression(scope.Type.MakePointer());
        }

        private BoundExpression BindCastExpression(CastExpressionSyntax syntax)
        {
            var type = LookupType(syntax.Type);
            return BindConversion(syntax.Expression, type, allowExplicit: true);
        }

        private BoundExpression BindConversion(ExpressionSyntax syntax, TypeSymbol type, bool allowExplicit = false)
        {
            var expression = BindExpression(syntax);
            return BindConversion(syntax.Location, expression, type, allowExplicit);
        }

        private BoundExpression BindConversion(TextLocation diagnosticLocation, BoundExpression expression, TypeSymbol type, bool allowExplicit = false)
        {
            var conversion = Conversion.Classify(expression.Type, type);

            if (!conversion.Exists)
            {
                if (expression.Type != TypeSymbol.Error && type != TypeSymbol.Error)
                    Diagnostics.ReportCannotConvert(diagnosticLocation, expression.Type, type);

                return new BoundErrorExpression();
            }

            if (!allowExplicit && conversion.IsExplicit)
            {
                Diagnostics.ReportCannotConvertImplicitly(diagnosticLocation, expression.Type, type);
            }

            if (conversion.IsIdentity)
                return expression;

            return new BoundConversionExpression(type, expression);
        }

        private BoundExpression BindMemberAccessExpression(MemberAccessExpressionSyntax syntax)
        {
            var identifierToken = syntax.IdentifierToken;
            var name = identifierToken.Text;
            var target = BindExpression(syntax.Target);
            var memberSymbol = target.Type.Members.SingleOrDefault(m => m.Name == name);
            if (memberSymbol == null)
            {
                Diagnostics.ReportTypeDoesNotHaveMember(identifierToken.Location, target.Type, name);
                return new BoundErrorExpression();
            }

            if (memberSymbol is FieldSymbol f)
                return new BoundFieldExpression(target, f);
            if (memberSymbol is PropertySymbol p)
                return new BoundPropertyExpression(target, p);

            throw new Exception();
        }

        private BoundExpression BindNewInstanceExpression(NewInstanceExpressionSyntax syntax)
        {
            var type = LookupType(syntax.Type);
            var constructor = type.Members.OfType<ConstructorSymbol>().First();

            var arguments = syntax.Arguments.ToArray();
            var parameters = constructor.Parameters;

            if (!TryBindArguments(syntax, constructor.Name, parameters, arguments, out var boundArguments))
                return new BoundErrorExpression();

            return new BoundNewInstanceExpression(type.MakePointer(), constructor, boundArguments);
        }

        private BoundExpression BindNewArrayExpression(NewArrayExpressionSyntax syntax)
        {
            if (syntax.Arguments.Count == 0)
            {
                Diagnostics.ArrayMustHaveAtLeastOneDimension(syntax.CloseBracketToken.Location);
                return new BoundErrorExpression();
            }

            var type = LookupType(syntax.Type);
            type = type.MakeArray(syntax.Arguments.Count);

            var arguments = ImmutableArray.CreateBuilder<BoundExpression>();

            foreach (var argument in syntax.Arguments)
            {
                arguments.Add(BindExpression(argument));
            }

            return new BoundNewArrayExpression(type, arguments.ToImmutable());
        }

        private BoundExpression BindInvokeExpression(InvokeExpressionSyntax syntax)
        {
            var target = syntax.Target;
            if (target is MemberAccessExpressionSyntax m)
            {
                var t = BindExpression(m.Target);
                if (!(t.Type.Members.FirstOrDefault(me => me.Name == m.IdentifierToken.Text) is MethodSymbol method))
                {
                    Diagnostics.ReportTypeDoesNotHaveMember(m.IdentifierToken.Location, t.Type, m.IdentifierToken.Text);
                    return new BoundErrorExpression();
                }

                var parameters = method.Parameters;
                var arguments = syntax.Arguments.OfType<ExpressionSyntax>().ToArray();

                if (!TryBindArguments(syntax, method.Name, parameters, arguments, out var boundArguments))
                    return new BoundErrorExpression();

                return new BoundMethodCallExpression(t, method, boundArguments);
            }

            // TODO this should be handled differently
            // this actaully means that we need to allocate
            // space on the heap
            // which also means that whatever follows the new
            // must be determinable in size
            //if (target is NewExpressionSyntax ne)
            //{
            //    target = ne.TypeName;
            //}

            if (!(target is NameExpressionSyntax n))
            {
                Diagnostics.ReportFunctionNameExpected(target.Location);
                return new BoundErrorExpression();
            }

            var symbol = GetSymbol(new []{SymbolKind.Method, SymbolKind.Type, SymbolKind.Function}, n.IdentifierToken);

            if (symbol is FunctionSymbol function)
            {
                var arguments = syntax.Arguments.ToArray();
                var parameters = function.Parameters;

                if (!TryBindArguments(syntax, function.Name, parameters, arguments, out var boundArguments))
                    return new BoundErrorExpression();

                return new BoundFunctionCallExpression(function, boundArguments);
            }

            if (symbol is TypeSymbol type)
            {
                var constructor = type.Members.OfType<ConstructorSymbol>().First();

                var arguments = syntax.Arguments.ToArray();
                var parameters = constructor.Parameters;

                if (!TryBindArguments(syntax, constructor.Name, parameters, arguments, out var boundArguments))
                    return new BoundErrorExpression();

                return new BoundConstructorCallExpression(constructor, boundArguments);
            }

            if (symbol is MethodSymbol method1)
            {
                //syntax.Target
                var arguments = syntax.Arguments.ToArray();
                var parameters = method1.Parameters;

                if (!TryBindArguments(syntax, method1.Name, parameters, arguments, out var boundArguments))
                    return new BoundErrorExpression();

                return new BoundMethodCallExpression(null, method1, boundArguments);
            }

            if (symbol != null)
                Diagnostics.ReportNotAFunction(n.Location, symbol);

            return new BoundErrorExpression();
        }

        private bool TryBindArguments(IInvocationExpressionSyntax syntax, string name, ImmutableArray<ParameterSymbol> parameters, ExpressionSyntax[] arguments,
            out ImmutableArray<BoundExpression> boundExpressions)
        {
            if (arguments.Length != parameters.Length)
            {
                TextSpan span;
                if (arguments.Length > parameters.Length)
                {
                    SyntaxNode firstExceedingNode;
                    if (parameters.Length > 0)
                        firstExceedingNode = syntax.Arguments.GetSeparator(parameters.Length - 1);
                    else
                        firstExceedingNode = syntax.Arguments[0];
                    var lastExceedingArgument = syntax.Arguments[syntax.Arguments.Count - 1];
                    span = TextSpan.FromBounds(firstExceedingNode.Span.Start, lastExceedingArgument.Span.End);
                }
                else
                {
                    span = syntax.CloseToken.Span;
                }

                var location = new TextLocation(syntax.SyntaxTree.Text, span);
                Diagnostics.ReportWrongArgumentCount(location, name, parameters.Length, arguments.Length);
                return false;
            }

            var builder1 = ImmutableArray.CreateBuilder<BoundExpression>();
            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var argument = BindExpression(arguments[i], parameter.Type);
                builder1.Add(argument);
            }

            boundExpressions = builder1.ToImmutable();
            return true;
        }

        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax, bool allowTypes)
        {
            return BindExpression(syntax.Expression, allowTypes);
        }

        //private T? GetSymbol<T>(Token identifierToken) where T : Symbol
        //{
        //    return (T?)GetSymbol(new[] { typeof(T) }, identifierToken);
        //}

        //private Symbol? GetSymbol<T1, T2>(Token identifierToken)
        //    where T1 : Symbol
        //    where T2 : Symbol
        //{
        //    return GetSymbol(new[] { typeof(T1), typeof(T2) }, identifierToken);
        //}

        //private bool TryGetSymbol<T>(Token identifierToken, out T? symbol)
        //    where T : Symbol
        //{
        //    symbol = default;
        //    if (!TryGetSymbol(new[] { typeof(T) }, identifierToken, out var s)) return false;
        //    symbol = (T)s;
        //    return true;
        //}

        //private bool TryGetTypeSymbol(Token identifierToken, out TypeSymbol type)
        //{
        //    type = null;
        //    var b = TryGetSymbol<AliasSymbol, TypeSymbol>(identifierToken, out var symbol);
        //    if (!b) return false;

        //    while (symbol is AliasSymbol a)
        //        symbol = a.Type;

        //    type = (TypeSymbol)symbol;
        //    return true;
        //}

        //private bool TryGetSymbol<T1, T2>(Token identifierToken, out Symbol symbol)
        //    where T1 : Symbol
        //    where T2 : Symbol
        //{
        //    return TryGetSymbol(new[] { typeof(T1), typeof(T2) }, identifierToken, out symbol);
        //}

        //private bool TryGetSymbol(SymbolKind[] allowedTypes, Token identifierToken, [NotNullWhen(true)] out Symbol? symbol)
        //{
        //    symbol = null;
        //    var name = identifierToken.Text;
        //    if (!_scope.TryLookup(allowedTypes, name, out var symbols)) return false;
        //    symbol = symbols.First();
        //    return allowedTypes.Contains(symbol.Kind);
        //}

        private Symbol? GetSymbol(SymbolKind[] allowedTypes, Token identifierToken)
        {
            var name = GetName(identifierToken);
            if (!_scope.TryLookup(allowedTypes, name, out var symbols))
            {
                return null;
            }

            var symbol = symbols.First();
                //GetSymbol(identifierToken);
            if (symbol == null)
            {
                return null;
            }

            if (!allowedTypes.Contains(symbol.Kind))
            {
                Diagnostics.ReportUnexpectedSymbol(identifierToken.Location, symbol.GetType().Name, allowedTypes.Select(t => t.ToString()).ToArray());
                return null;
            }

            return symbol;
        }

        private Symbol? GetSymbol(Token identifierToken)
        {
            var name = GetName(identifierToken);

            if (string.IsNullOrEmpty(name))
            {
                // Token was inserted by parser
                return null;
            }

            if (!_scope.TryLookup(Array.Empty<SymbolKind>(), name, out var symbols))
            {
                Diagnostics.ReportUndefinedName(identifierToken.Location, name);
                return null;
            }

            return symbols.First();
        }

        private static string GetName(Token token)
        {
            switch (token.Kind)
            {
                case TokenKind.Identifier:
                    return token.Text;
                case TokenKind.AnyKeyword:
                    return "Any";
                case TokenKind.StringKeyword:
                    return "String";
                case TokenKind.CharKeyword:
                    return "Char";
                case TokenKind.IntKeyword:
                    return "Int";
                case TokenKind.I8Keyword:
                    return "Int8";
                case TokenKind.I16Keyword:
                    return "Int16";
                case TokenKind.I32Keyword:
                    return "Int32";
                case TokenKind.I64Keyword:
                    return "Int64";
                case TokenKind.UIntKeyword:
                    return "UInt";
                case TokenKind.U8Keyword:
                    return "UInt8";
                case TokenKind.U16Keyword:
                    return "UInt16";
                case TokenKind.U32Keyword:
                    return "UInt32";
                case TokenKind.U64Keyword:
                    return "UInt64";
                case TokenKind.BoolKeyword:
                    return "Boolean";
                default:
                    return "?";
            }

            throw new InvalidOperationException($"Unsupported Token kind {token.Kind}");
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax syntax, bool allowTypes)
        {
            // was token inserted by parser? If so we already reported on it. Just return.
            if (syntax.IdentifierToken.IsMissing)
                return new BoundErrorExpression();

            var symbol = GetSymbol(syntax.IdentifierToken);

            while (symbol is AliasSymbol a)
                symbol = a.Type;

            switch (symbol)
            {
                case VariableSymbol v: return new BoundVariableExpression(v);
                case TypeSymbol t when allowTypes: return new BoundTypeExpression(t);
                case ConstSymbol c: return new BoundConstExpression(c);
                case FieldSymbol f: return new BoundFieldExpression(new BoundThisExpression(f.DeclaringType), f);
                //case FunctionGroup fg: return new 
                case FunctionSymbol f: return new BoundFunctionExpression(f);
                case null: return new BoundErrorExpression();
                default:
                    Diagnostics.ReportNotSupported(syntax.IdentifierToken.Location);
                    return new BoundErrorExpression();
            }
        }

        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
        {
            var target = BindExpressionInternal(syntax.Target);
            var expression = BindExpression(syntax.Expression);

            var type = TypeSymbol.Error;
            switch (target)
            {
                case BoundVariableExpression v:
                    if (v.Variable.IsReadOnly)
                        Diagnostics.ReportCannotAssign(syntax.EqualsToken.Location, v.Variable.Name);
                    type = v.Type;
                    break;
                case BoundArrayIndexExpression i:
                    type = i.Type;
                    break;
                case BoundFieldExpression f:
                    // todo check for readonly field
                    type = f.Type;
                    break;
                case BoundErrorExpression _: break;
                case BoundLiteralExpression _: break;
                default:
                    var symbol = target switch
                    {
                        BoundFunctionExpression f => f.FunctionSymbol,
                    };
                    Diagnostics.ReportNotLValue(syntax.Target.Location, symbol);
                    break;
            }

            var convertedExpression = BindConversion(syntax.Expression.Location, expression, type);

            return new BoundAssignmentExpression(target, convertedExpression);
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var token = syntax.LiteralToken;
            object value = null;

            TypeSymbol type = TypeSymbol.Int;
            switch (token.Kind)
            {
                case TokenKind.TrueKeyword:
                case TokenKind.FalseKeyword:
                    value = token.Kind == TokenKind.TrueKeyword;
                    type = TypeSymbol.Bool;
                    break;
                case TokenKind.NumberLiteral:
                    if (!long.TryParse(token.Text, out var number))
                        Diagnostics.ReportInvalidNumber(token.Location, token.Text);
                    value = number;
                    type = TypeSymbol.Int;
                    break;
                case TokenKind.CharacterLiteral:
                    type = TypeSymbol.Char;
                    value = syntax.Value;
                    break;
                case TokenKind.StringLiteral:
                    value = token.Text.Substring(1, token.Text.Length - 2)
                        .Replace(@"\""", "\"")
                        .Replace(@"\t", "\t")
                        .Replace(@"\r", "\r")
                        .Replace(@"\n", "\n");
                    type = TypeSymbol.String;
                    break;
            }

            return new BoundLiteralExpression(type, value);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var operand = BindExpression(syntax.Operand);

            var operatorToken = syntax.OperatorToken;

            if (operatorToken.Kind == TokenKind.Star)
            {
                return new BoundDereferenceExpression(operand);
            }

            var boundOperator = BoundUnaryOperator.Bind(operatorToken.Kind, operand.Type);
            if (boundOperator == null)
            {
                Diagnostics.ReportUndefinedUnaryOperator(operatorToken.Location, operatorToken.Text, operand.Type);
                return operand;
            }

            return new BoundUnaryExpression(boundOperator, operand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var left = BindExpression(syntax.Left, false);
            var right = BindExpression(syntax.Right);

            if (left.Type == TypeSymbol.Error || right.Type == TypeSymbol.Error)
                return new BoundErrorExpression();

            var operatorToken = syntax.OperatorToken;
            var boundOperator = BoundBinaryOperator.Bind(operatorToken.Kind, left.Type, right.Type);
            if (boundOperator == null)
            {
                Diagnostics.ReportUndefinedBinaryOperator(operatorToken.Location, operatorToken.Text, left.Type, right.Type);
                return left;
            }

            if (boundOperator.Kind == BoundBinaryOperatorKind.Concatenation)
            {
                if (!TryResolveMethod("String.Concat", new []{ TypeSymbol.String.MakeReference(), TypeSymbol.String.MakeReference() }, out var method))
                {
                    Diagnostics.ReportMissingExpectedFunction(syntax.OperatorToken.Location, "Concat");
                    return new BoundErrorExpression();
                }
                return new BoundMethodCallExpression(null, method, ImmutableArray.Create(left, right));
            }

            return new BoundBinaryExpression(left, boundOperator, right);
        }

        private bool TryResolve(string name, out Symbol[] symbols)
        {
            symbols = Array.Empty<Symbol>();
            var parts = name.Split(".");
            var part = parts[0];

            var found = _scope.TryLookup(Array.Empty<SymbolKind>(), part, out symbols);

            if (parts.Length == 1)
                return found;

            if (symbols.Length == 1)
            {
                var i = 1;
                var symbol = symbols[0];
                while (i < parts.Length && symbol is TypeSymbol t)
                {
                    part = parts[i];
                    i++;
                    var x = t.Members
                        .Where(m => m.Name == part)
                        .Cast<MethodSymbol>()
                        .ToArray();
                    if (x.Length == 0)
                        return false;
                    symbols = x;
                    symbol = symbols.Length > 0 ? symbols[0] : null;
                }
            }

            return true;
        }

        private bool TryResolveMethod(string name, TypeSymbol[] argTypes, [NotNullWhen(true)]out MethodSymbol? method)
        {
            method = null;
            if (!TryResolve(name, out var symbols))
                return false;

            method = symbols.OfType<MethodSymbol>()
                .SingleOrDefault(m => m.Parameters.Select(p => p.Type).SequenceEqual(argTypes));

            return true;
        }
    }
}