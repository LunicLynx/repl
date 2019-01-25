using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Repl.CodeAnalysis.Syntax;
using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis.Binding
{
    public class Binder
    {
        private bool _inLoop;
        private IScope _scope;

        public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

        public Binder(BoundScope parent)
        {
            _scope = new BoundScope(parent);
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnitSyntax syntax)
        {
            var builtInSymbols = ImmutableArray.Create<Symbol>(
                TypeSymbol.Void,
                TypeSymbol.Bool,
                TypeSymbol.I8,
                TypeSymbol.I16,
                TypeSymbol.I32,
                TypeSymbol.I64,
                TypeSymbol.U8,
                TypeSymbol.U16,
                TypeSymbol.U32,
                TypeSymbol.U64,
                TypeSymbol.Int,
                TypeSymbol.Uint,
                TypeSymbol.String
            );

            previous = previous ?? new BoundGlobalScope(null, ImmutableArray<Diagnostic>.Empty,
                           builtInSymbols, null);
            var parent = CreateParentScopes(previous);
            var binder = new Binder(parent);

            // create types
            binder.DeclareTypes(syntax.Nodes);
            // create members
            binder.DeclareMembers(syntax.Nodes);

            // bind together
            var nodes = binder.BindNodes(syntax.Nodes);
            var diagnostics = binder.Diagnostics.ToImmutableArray();
            var symbols = binder._scope.GetDeclaredSymbols();
            return new BoundGlobalScope(previous, diagnostics, symbols, new BoundScriptUnit(nodes));
        }

        private void DeclareMembers(ImmutableArray<SyntaxNode> nodes)
        {
            var functions = nodes.OfType<FunctionDeclarationSyntax>();

            foreach (var function in functions)
            {
                DeclareFunction(function);
            }

            var structs = nodes.OfType<StructDeclarationSyntax>();
            foreach (var @struct in structs)
            {
                DeclareTypeMembers(@struct);
            }
        }

        private void DeclareTypeMembers(StructDeclarationSyntax syntax)
        {
            var symbol = GetSymbol<TypeSymbol>(syntax.IdentifierToken);

            if (syntax.BaseType != null)
            {
                var baseType = LookupBaseType(syntax.BaseType);

                // check for cyclic dependency
                if (IsCyclicDependency(baseType, symbol))
                {
                    Diagnostics.ReportCyclicDependency(syntax.BaseType.Span);
                }

                symbol.BaseType = baseType;
            }

            _scope = new TypeScope(_scope, symbol);

            foreach (var member in syntax.Members)
            {
                DeclareTypeMember(member);
            }

            var hasConstructor = false;
            if (!hasConstructor)
            {
                var constructorSymbol = new ConstructorSymbol(symbol, Array.Empty<ParameterSymbol>());
                DeclareSymbol(constructorSymbol, syntax.IdentifierToken);
            }

            symbol.Lock();
            _scope = _scope.Parent;
        }

        private void DeclareTypeMember(MemberDeclarationSyntax syntax)
        {
            switch (syntax)
            {
                case FieldDeclarationSyntax f:
                    DeclareField(f);
                    break;
                case MethodDeclarationSyntax m:
                    DeclareMethod(m);
                    break;
                default:
                    throw new Exception($"Unexpected syntax {syntax.GetType()}");
            }
        }

        private void DeclareMethod(MethodDeclarationSyntax syntax)
        {
            var method = BindPrototype2(syntax.Prototype);
            DeclareSymbol(method, syntax.Prototype.IdentifierToken);
        }

        private void DeclareField(FieldDeclarationSyntax syntax)
        {
            var type = LookupTypeAnnotation(syntax.TypeAnnotation);
            var field = new FieldSymbol(syntax.IdentifierToken.Text, type);
            DeclareSymbol(field, syntax.IdentifierToken);
        }

        private void DeclareFunction(FunctionDeclarationSyntax syntax)
        {
            var function = BindPrototype(syntax.Prototype);
            DeclareSymbol(function, syntax.Prototype.IdentifierToken);
        }

        private void DeclareTypes(ImmutableArray<SyntaxNode> nodes)
        {
            var structs = nodes.OfType<StructDeclarationSyntax>();
            var types = structs;

            foreach (var type in types)
            {
                DeclareType(type);
            }

            var aliases = nodes.OfType<AliasDeclarationSyntax>();
            foreach (var alias in aliases)
            {
                DeclareAlias(alias);
            }
        }

        private void DeclareAlias(AliasDeclarationSyntax alias)
        {
            var identifierToken = alias.IdentifierToken;
            var symbol = new AliasSymbol(alias.IdentifierToken.Text, null);
            DeclareSymbol(symbol, identifierToken);
        }

        private void DeclareType(SyntaxNode type)
        {
            Symbol symbol;
            Token identifierToken;
            switch (type)
            {
                case StructDeclarationSyntax s:
                    identifierToken = s.IdentifierToken;
                    symbol = new TypeSymbol(identifierToken.Text, typeof(object), ImmutableArray<TypeSymbol>.Empty, ImmutableArray<MemberSymbol>.Empty);
                    break;
                default:
                    throw new Exception($"Unexpected syntax {type.GetType()}");
            }
            DeclareSymbol(symbol, identifierToken);
        }
        
        private static BoundScope CreateParentScopes(BoundGlobalScope previous)
        {
            var stack = new Stack<BoundGlobalScope>();
            while (previous != null)
            {
                stack.Push(previous);
                previous = previous.Previous;
            }

            BoundScope parent = null;

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

        public BoundNode BindNode(SyntaxNode node)
        {
            switch (node)
            {
                case StatementSyntax s:
                    return BindStatement(s);
                case AliasDeclarationSyntax a:
                    return BindAliasDeclaration(a);
                case ExternDeclarationSyntax e:
                    return BindExternDeclaration(e);
                case FunctionDeclarationSyntax f:
                    return BindFunctionDeclaration(f);
                case StructDeclarationSyntax s:
                    return BindStructDeclaration(s);
                case ConstDeclarationSyntax c:
                    return BindConstDeclaration(c);
                default:
                    throw new Exception($"Unsupported node {node}");
            }
        }

        private BoundNode BindConstDeclaration(ConstDeclarationSyntax syntax)
        {
            var initializer = BindExpression(syntax.Initializer);
            if (!TryEvalConstExpression(initializer, out var type, out var value))
            {
                Diagnostics.ReportExpressionIsNotCompileTimeConstant(syntax.Initializer.Span);
                type = TypeSymbol.I32;
                value = 0;
            }

            if (syntax.TypeAnnotation != null)
            {
                type = LookupTypeAnnotation(syntax.TypeAnnotation);
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
                case BoundCastExpression c:
                    if (!TryEvalConstExpression(c.Expression, out var t1, out var v1)) return false;
                    type = c.Type;
                    value = v1;
                    return true;
                case BoundUnaryExpression u:
                    if (!TryEvalConstExpression(u.Operand, out var t2, out var v2)) return false;
                    type = t2;
                    switch (u.Operator.Kind)
                    {
                        case BoundUnaryOperatorKind.Identity:
                            value = +(int)v2;
                            break;
                        case BoundUnaryOperatorKind.Negation:
                            value = -(int)v2;
                            break;
                        case BoundUnaryOperatorKind.LogicalNot:
                            value = !(bool)v2;
                            break;
                        case BoundUnaryOperatorKind.BitwiseComplement:
                            value = ~(int)v2;
                            break;
                    }
                    return true;
                case BoundBinaryExpression b:
                    if (!TryEvalConstExpression(b.Left, out var lt, out var lv)) return false;
                    if (!TryEvalConstExpression(b.Right, out var rt, out var rv)) return false;
                    return true;
                case BoundLiteralExpression l:
                    type = l.Type;
                    value = l.Value;
                    return true;
                default:
                    return false;
            }
        }

        private BoundNode BindAliasDeclaration(AliasDeclarationSyntax syntax)
        {
            var typeSymbol = LookupType(syntax.Type);
            //var identifierToken = syntax.IdentifierToken;
            var aliasSymbol = GetSymbol<AliasSymbol>(syntax.IdentifierToken);
            aliasSymbol.Type = typeSymbol;
            aliasSymbol.Lock();
            //var aliasSymbol = new AliasSymbol(identifierToken.Text, typeSymbol);
            //DeclareSymbol(aliasSymbol, identifierToken);
            return new BoundAliasDeclaration(aliasSymbol);
        }

        public BoundStatement BindStatement(StatementSyntax stmt)
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
                default:
                    throw new Exception($"Unexpected syntax {stmt.GetType()}");
            }
        }

        private BoundStructDeclaration BindStructDeclaration(StructDeclarationSyntax syntax)
        {
            var symbol = GetSymbol<TypeSymbol>(syntax.IdentifierToken);

            _scope = new TypeScope(_scope, symbol);

            var members = syntax.Members.Select(BindMemberDeclaration).ToList();
            _scope = _scope.Parent;

            return new BoundStructDeclaration(symbol, members.ToImmutableArray());
        }

        private bool IsCyclicDependency(ImmutableArray<TypeSymbol> baseType, TypeSymbol symbol)
        {
            return false;
        }

        private ImmutableArray<TypeSymbol> LookupBaseType(BaseTypeSyntax syntax)
        {
            return syntax.Types.OfType<TypeSyntax>().Select(LookupType).ToImmutableArray();
        }

        private BoundMemberDeclaration BindMemberDeclaration(MemberDeclarationSyntax syntax)
        {
            switch (syntax)
            {
                case FieldDeclarationSyntax f:
                    return BindFieldDeclaration(f);
                case MethodDeclarationSyntax m:
                    return BindMethodDeclaration(m);
                default:
                    throw new Exception($"Unexpected node {syntax.GetType()}");
            }
        }

        private BoundMemberDeclaration BindMethodDeclaration(MethodDeclarationSyntax syntax)
        {
            var method = GetSymbol<MethodSymbol>(syntax.IdentifierToken);
            _scope = new BoundScope(_scope);

            var parameters = syntax.Prototype.Parameters.OfType<ParameterSyntax>().ToArray();
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var symbol = method.Parameters[i];
                DeclareSymbol(symbol, parameter.IdentifierToken);
            }

            var body = BindBlockStatement(syntax.Body);
            _scope = _scope.Parent;

            return new BoundMethodDeclaration(method, body);
        }

        private BoundMemberDeclaration BindFieldDeclaration(FieldDeclarationSyntax syntax)
        {
            var field = GetSymbol<FieldSymbol>(syntax.IdentifierToken);

            BoundExpression initializer = null;
            if (syntax.Initializer != null)
            {
                initializer = BindExpression(syntax.Initializer.Expression);
            }

            TypeSymbol type = null;
            if (syntax.TypeAnnotation != null)
            {
                type = LookupTypeAnnotation(syntax.TypeAnnotation);
            }
            else if (initializer == null)
            {
                Diagnostics.ReportMemberMustBeTyped(syntax.IdentifierToken.Span);
                type = TypeSymbol.I32;
            }
            else
            {
                type = initializer.Type;
            }


            return new BoundFieldDeclaration(field, initializer);
        }

        private BoundFunctionDeclaration BindFunctionDeclaration(FunctionDeclarationSyntax syntax)
        {
            var function = GetSymbol<FunctionSymbol>(syntax.Prototype.IdentifierToken);
            _scope = new BoundScope(_scope);

            var parameters = syntax.Prototype.Parameters.OfType<ParameterSyntax>().ToArray();
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var symbol = function.Parameters[i];
                DeclareSymbol(symbol, parameter.IdentifierToken);
            }

            var body = BindBlockStatement(syntax.Body);
            _scope = _scope.Parent;

            return new BoundFunctionDeclaration(function, body);
        }

        private FunctionSymbol BindPrototype(PrototypeSyntax syntax)
        {
            var typeSyntax = syntax.ReturnType;

            var type = typeSyntax != null ? LookupTypeAnnotation(typeSyntax) : TypeSymbol.Void;

            var identifierToken = syntax.IdentifierToken;
            var name = identifierToken.Text;

            var parameters = syntax.Parameters.OfType<ParameterSyntax>().Select(BindParameter).ToArray();

            var function = new FunctionSymbol(type, name, parameters);

            return function;
        }

        private MethodSymbol BindPrototype2(PrototypeSyntax syntax)
        {
            var typeSyntax = syntax.ReturnType;

            var type = typeSyntax != null ? LookupTypeAnnotation(typeSyntax) : TypeSymbol.Void;

            var identifierToken = syntax.IdentifierToken;
            var name = identifierToken.Text;

            var parameters = syntax.Parameters.OfType<ParameterSyntax>().Select(BindParameter).ToArray();

            var function = new MethodSymbol(type, name, parameters);

            return function;
        }

        private ParameterSymbol BindParameter(ParameterSyntax syntax, int index)
        {
            var type = LookupTypeAnnotation(syntax.Type);
            var name = syntax.IdentifierToken.Text;
            var parameter = new ParameterSymbol(type, name, index);

            return parameter;
        }

        private TypeSymbol LookupTypeAnnotation(TypeAnnotationSyntax syntax)
        {
            return LookupType(syntax.Type);
        }

        private TypeSymbol LookupType(TypeSyntax syntax)
        {
            var typeIdentifierToken = syntax.TypeOrIdentifierToken;
            var type = GetSymbol<AliasSymbol, TypeSymbol>(typeIdentifierToken) ?? TypeSymbol.Int;

            while (type is AliasSymbol a)
                type = a.Type;

            return (TypeSymbol)type;
        }

        private BoundExternDeclaration BindExternDeclaration(ExternDeclarationSyntax syntax)
        {
            var function = BindPrototype(syntax.Prototype);
            DeclareSymbol(function, syntax.Prototype.IdentifierToken);
            return new BoundExternDeclaration(function);
        }

        private BoundStatement BindForStatement(ForStatementSyntax syntax)
        {
            var lowerBound = BindExpression(syntax.LowerBound, TypeSymbol.I32);
            var upperBound = BindExpression(syntax.UpperBound, TypeSymbol.I32);

            _scope = new BoundScope(_scope);

            var variable = new VariableSymbol(syntax.IdentifierToken.Text, false, lowerBound.Type);
            _scope.TryDeclare(variable);

            var oldInLoop = _inLoop;
            _inLoop = true;
            var body = BindBlockStatement(syntax.Body);
            _inLoop = oldInLoop;

            _scope = _scope.Parent;

            return new BoundForStatement(variable, lowerBound, upperBound, body);
        }


        private BoundStatement BindContinueStatement(ContinueStatementSyntax syntax)
        {
            if (!_inLoop)
            {
                Diagnostics.ReportContinueOutsideLoop(syntax.ContinueKeyword.Span);
            }
            return new BoundContinueStatement();
        }

        private BoundStatement BindBreakStatement(BreakStatementSyntax syntax)
        {
            if (!_inLoop)
            {
                Diagnostics.ReportBreakOutsideLoop(syntax.BreakKeyword.Span);
            }
            return new BoundBreakStatement();
        }

        private BoundStatement BindWhileStatement(WhileStatementSyntax syntax)
        {
            var condition = BindExpression(syntax.Condition, TypeSymbol.Bool);
            var oldInLoop = _inLoop;
            _inLoop = true;
            var body = BindBlockStatement(syntax.Body);
            _inLoop = oldInLoop;
            return new BoundWhileStatement(condition, body);
        }

        private BoundStatement BindLoopStatement(LoopStatementSyntax syntax)
        {
            var oldInLoop = _inLoop;
            _inLoop = true;
            var body = BindBlockStatement(syntax.Body);
            _inLoop = oldInLoop;
            return new BoundLoopStatement(body);
        }

        private BoundStatement BindIfStatement(IfStatementSyntax syntax)
        {
            var condition = BindExpression(syntax.Condition, TypeSymbol.Bool);

            var thenBlock = BindBlockStatement(syntax.ThenBlock);

            // Example for ?: operator -> if null its null of the right expression.type
            // var elseStatement = syntax.ElseClause ?: BindStatement(syntax.ElseClause.ElseStatement);
            var elseStatement = syntax.ElseClause == null ? null : BindStatement(syntax.ElseClause.ElseStatement);
            return new BoundIfStatement(condition, thenBlock, elseStatement);
        }

        private BoundStatement BindVariableDeclaration(VariableDeclarationSyntax syntax)
        {
            var identifierToken = syntax.IdentifierToken;
            var name = identifierToken.Text;
            var isReadOnly = syntax.Keyword.Kind == TokenKind.LetKeyword;
            var initializer = BindExpression(syntax.Initializer);
            var variable = new VariableSymbol(name, isReadOnly, initializer.Type);

            DeclareSymbol(variable, identifierToken);

            return new BoundVariableDeclaration(variable, initializer);
        }

        private void DeclareSymbol(Symbol symbol, Token identifierToken)
        {
            if (!_scope.TryDeclare(symbol))
            {
                Diagnostics.ReportSymbolAlreadyDeclared(identifierToken.Span, identifierToken.Text);
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

        private ImmutableArray<BoundNode> BindNodes(ImmutableArray<SyntaxNode> syntaxNodes)
        {
            var nodes = ImmutableArray.CreateBuilder<BoundNode>();
            foreach (var syntaxNode in syntaxNodes)
            {
                var node = BindNode(syntaxNode);
                nodes.Add(node);
            }
            return nodes.ToImmutable();
        }

        private BoundStatement BindExpressionStatement(ExpressionStatementSyntax syntax)
        {
            var expression = BindExpression(syntax.Expression);
            return new BoundExpressionStatement(expression);
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax, TypeSymbol targetType)
        {
            var expression = BindExpression(syntax);
            if (!IsAssignable(targetType, expression.Type))
                Diagnostics.ReportCannotConvert(syntax.Span, expression.Type, targetType);
            if (expression.Type != targetType)
                return new BoundCastExpression(targetType, expression);
            return expression;
        }

        private bool IsAssignable(TypeSymbol to, TypeSymbol from)
        {
            if (from == to) return true;
            if (to.IsAssignableFrom(from)) return true;

            try
            {
                var toProperties = GetIntegerTypeProperties(to);
                var fromProperties = GetIntegerTypeProperties(from);
                return toProperties.signed == fromProperties.signed && toProperties.size >= fromProperties.size
                       || toProperties.signed && toProperties.size > fromProperties.size;
            }
            catch
            {
                return false;
            }
        }

        private (bool signed, int size) GetIntegerTypeProperties(TypeSymbol type)
        {
            if (type == TypeSymbol.I64) return (true, 8);
            if (type == TypeSymbol.I32) return (true, 4);
            if (type == TypeSymbol.I16) return (true, 2);
            if (type == TypeSymbol.I8) return (true, 1);
            if (type == TypeSymbol.U64) return (false, 8);
            if (type == TypeSymbol.U32) return (false, 4);
            if (type == TypeSymbol.U16) return (false, 2);
            if (type == TypeSymbol.U8) return (false, 1);
            throw new Exception("Non integer types are not supported");
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax, bool allowTypes = true)
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
                case NewExpressionSyntax n:
                    return BindNewExpression(n);
                case MemberAccessExpressionSyntax m:
                    return BindMemberAccessExpression(m);
                case CastExpressionSyntax c:
                    return BindCastExpression(c);
                default:
                    throw new Exception($"Unexpected syntax {syntax.GetType()}");
            }
        }

        private BoundExpression BindCastExpression(CastExpressionSyntax syntax)
        {
            var type = LookupType(syntax.Type);
            var expression = BindExpression(syntax.Expression);
            return new BoundCastExpression(type, expression);
        }

        private BoundExpression BindMemberAccessExpression(MemberAccessExpressionSyntax syntax)
        {
            var identifierToken = syntax.IdentifierToken;
            var name = identifierToken.Text;
            var target = BindExpression(syntax.Target);
            var memberSymbol = target.Type.Members.SingleOrDefault(m => m.Name == name);
            if (memberSymbol == null)
            {
                Diagnostics.ReportTypeDoesNotHaveMember(identifierToken.Span, target.Type, name);
                return new BoundLiteralExpression(TypeSymbol.I32, 0);
            }
            return new BoundMemberAccessExpression(target, memberSymbol);
        }

        private BoundExpression BindNewExpression(NewExpressionSyntax syntax)
        {
            var identifierToken = syntax.TypeName.IdentifierToken;
            var typeSymbol = GetSymbol<TypeSymbol>(identifierToken);
            return new BoundNewExpression(typeSymbol);
        }

        private BoundExpression BindInvokeExpression(InvokeExpressionSyntax syntax)
        {
            var target = syntax.Target;
            if (target is MemberAccessExpressionSyntax m)
            {
                var t = BindExpression(m.Target);
                if (!(t.Type.Members.FirstOrDefault(me => me.Name == m.IdentifierToken.Text) is MethodSymbol method))
                {
                    Diagnostics.ReportTypeDoesNotHaveMember(m.IdentifierToken.Span, t.Type, m.IdentifierToken.Text);
                    return new BoundLiteralExpression(TypeSymbol.I32, 0);
                }

                var parameters = method.Parameters;
                var arguments = syntax.Arguments.OfType<ExpressionSyntax>().ToArray();

                if (!TryBindArguments(method.Name, parameters, arguments, out var boundArguments))
                    return new BoundLiteralExpression(TypeSymbol.I32, 0);

                return new BoundMethodCallExpression(t, method, boundArguments);
            }

            if (target is NewExpressionSyntax ne)
            {
                target = ne.TypeName;
            }

            if (!(target is NameExpressionSyntax n))
            {
                Diagnostics.ReportFunctionNameExpected(target.Span);
                return new BoundLiteralExpression(TypeSymbol.I32, 0);
            }

            var allowedTypes = new[]
            {
                typeof(TypeSymbol),
                typeof(FunctionSymbol),
                typeof(MethodSymbol)
            };
            var symbol = GetSymbol(allowedTypes, n.IdentifierToken);

            if (symbol is FunctionSymbol function)
            {
                var arguments = syntax.Arguments.OfType<ExpressionSyntax>().ToArray();
                var parameters = function.Parameters;

                if (!TryBindArguments(function.Name, parameters, arguments, out var boundArguments))
                    return new BoundLiteralExpression(TypeSymbol.I32, 0);

                return new BoundFunctionCallExpression(function, boundArguments);
            }

            if (symbol is TypeSymbol type)
            {
                var constructor = type.Members.OfType<ConstructorSymbol>().First();

                var arguments = syntax.Arguments.OfType<ExpressionSyntax>().ToArray();
                var parameters = constructor.Parameters;

                if (!TryBindArguments(constructor.Name, parameters, arguments, out var boundArguments))
                    return new BoundLiteralExpression(TypeSymbol.I32, 0);

                return new BoundConstructorCallExpression(constructor, boundArguments);
            }

            if (symbol is MethodSymbol method1)
            {

                //syntax.Target
                var arguments = syntax.Arguments.OfType<ExpressionSyntax>().ToArray();
                var parameters = method1.Parameters;

                if (!TryBindArguments(method1.Name, parameters, arguments, out var boundArguments))
                    return new BoundLiteralExpression(TypeSymbol.I32, 0);

                return new BoundMethodCallExpression(null, method1, boundArguments);
            }

            return new BoundLiteralExpression(TypeSymbol.I32, 0);
        }

        private bool TryBindArguments(string name, ParameterSymbol[] parameters, ExpressionSyntax[] arguments,
            out ImmutableArray<BoundExpression> boundExpressions)
        {
            if (parameters.Length != arguments.Length)
            {
                var start = arguments.First().Span.Start;
                var end = arguments.Last().Span.End;

                Diagnostics.ReportParameterCount(TextSpan.FromBounds(start, end), name,
                    parameters.Length, arguments.Length);
                {

                    return false;
                }
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

        private T GetSymbol<T>(Token identifierToken) where T : Symbol
        {
            return (T)GetSymbol(new[] { typeof(T) }, identifierToken);
        }

        private Symbol GetSymbol<T1, T2>(Token identifierToken)
            where T1 : Symbol
            where T2 : Symbol
        {
            return GetSymbol(new[] { typeof(T1), typeof(T2) }, identifierToken);
        }

        private bool TryGetSymbol<T>(Token identifierToken, out T symbol)
            where T : Symbol
        {
            symbol = default;
            if (!TryGetSymbol(new[] { typeof(T) }, identifierToken, out var s)) return false;
            symbol = (T)s;
            return true;
        }

        private bool TryGetTypeSymbol(Token identifierToken, out TypeSymbol type)
        {
            type = null;
            var b = TryGetSymbol<AliasSymbol, TypeSymbol>(identifierToken, out var symbol);
            if (!b) return false;

            while (symbol is AliasSymbol a)
                symbol = a.Type;

            type = (TypeSymbol)symbol;
            return true;
        }

        private bool TryGetSymbol<T1, T2>(Token identifierToken, out Symbol symbol)
            where T1 : Symbol
            where T2 : Symbol
        {
            return TryGetSymbol(new[] { typeof(T1), typeof(T2) }, identifierToken, out symbol);
        }

        private bool TryGetSymbol(Type[] allowedTypes, Token identifierToken, out Symbol symbol)
        {
            var name = identifierToken.Text;
            if (!_scope.TryLookup(name, out symbol)) return false;
            var type = symbol.GetType();
            return allowedTypes.Any(a => a.IsAssignableFrom(type));
        }

        private Symbol GetSymbol(Type[] allowedTypes, Token identifierToken)
        {
            var symbol = GetSymbol(identifierToken);
            if (symbol == null)
            {
                return null;
            }

            var type = symbol.GetType();
            if (!allowedTypes.Any(a => a.IsAssignableFrom(type)))
            {
                Diagnostics.ReportUnexpectedSymbol(identifierToken.Span, symbol.GetType().Name, allowedTypes.Select(t => t.Name).ToArray());
                return null;
            }

            return symbol;
        }

        private Symbol GetSymbol(Token identifierToken)
        {
            var name = identifierToken.Text;
            if (string.IsNullOrEmpty(name))
            {
                // Token was inserted by parser
                return null;
            }

            if (!_scope.TryLookup(name, out var symbol))
            {
                Diagnostics.ReportUndefinedName(identifierToken.Span, name);
                return null;
            }

            return symbol;
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax syntax, bool allowTypes)
        {
            var symbol = GetSymbol<Symbol>(syntax.IdentifierToken);

            while (symbol is AliasSymbol a)
                symbol = a.Type;

            switch (symbol)
            {
                case VariableSymbol v: return new BoundVariableExpression(v);
                case ParameterSymbol p: return new BoundParameterExpression(p);
                case TypeSymbol t when allowTypes: return new BoundTypeExpression(t);
                case ConstSymbol c: return new BoundConstExpression(c);
                case FieldSymbol f: return new BoundFieldExpression(null, f);
                default:
                    Diagnostics.ReportNotSupported(syntax.IdentifierToken.Span);
                    return new BoundLiteralExpression(TypeSymbol.I32, 0);
            }
        }

        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
        {
            var target = BindExpression(syntax.Target);
            var expression = BindExpression(syntax.Expression);
            return new BoundAssignmentExpression(target, expression);

            //var name = syntax.IdentifierToken.Text;
            //var value = BindExpression(syntax.Expression);

            //if (!_scope.TryLookup(name, out var symbol))
            //{
            //    Diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
            //    return value;
            //}

            //if (!(symbol is VariableSymbol variable))
            //{
            //    Diagnostics.ReportUnexpectedSymbol(syntax.IdentifierToken.Span, symbol.GetType().Name, nameof(VariableSymbol));
            //    return value;
            //}

            //if (variable.IsReadOnly)
            //{
            //    Diagnostics.ReportCannotAssign(syntax.EqualsToken.Span, name);
            //    return value;
            //}

            //if (value.Type != variable.Type)
            //{
            //    Diagnostics.ReportCannotConvert(syntax.Expression.Span, value.Type, variable.Type);
            //    return value;
            //}

            //return new BoundAssignmentExpression(variable, value);
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var token = syntax.LiteralToken;
            object value = null;

            TypeSymbol type = TypeSymbol.I32;
            switch (token.Kind)
            {
                case TokenKind.TrueKeyword:
                case TokenKind.FalseKeyword:
                    value = token.Kind == TokenKind.TrueKeyword;
                    type = TypeSymbol.Bool;
                    break;
                case TokenKind.NumberLiteral:
                    if (!int.TryParse(token.Text, out var number))
                        Diagnostics.ReportInvalidNumber(token.Span, token.Text);
                    value = number;
                    type = TypeSymbol.I32;
                    break;
                case TokenKind.StringLiteral:
                    value = token.Text.Substring(1, token.Text.Length - 2)
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
            var boundOperator = BoundUnaryOperator.Bind(operatorToken.Kind, operand.Type);
            if (boundOperator == null)
            {
                Diagnostics.ReportUndefinedUnaryOperator(operatorToken.Span, operatorToken.Text, operand.Type);
                return operand;
            }

            return new BoundUnaryExpression(boundOperator, operand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var left = BindExpression(syntax.Left, false);
            var right = BindExpression(syntax.Right);

            var operatorToken = syntax.OperatorToken;
            var boundOperator = BoundBinaryOperator.Bind(operatorToken.Kind, left.Type, right.Type);
            if (boundOperator == null)
            {
                Diagnostics.ReportUndefinedBinaryOperator(operatorToken.Span, operatorToken.Text, left.Type, right.Type);
                return left;
            }

            return new BoundBinaryExpression(left, boundOperator, right);
        }
    }
}