﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Repl.CodeAnalysis.Syntax;

namespace Repl.CodeAnalysis.Binding
{
    public class Binder
    {
        private bool _inLoop;
        private BoundScope _scope;

        public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

        public Binder(BoundScope parent)
        {
            _scope = new BoundScope(parent);
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnitSyntax syntax)
        {
            previous = previous ?? new BoundGlobalScope(null, ImmutableArray<Diagnostic>.Empty,
                           ImmutableArray.Create<Symbol>(new TypeSymbol("void", typeof(void))),
                           new BoundExpressionStatement(new BoundLiteralExpression(0)));
            var parent = CreateParentScopes(previous);
            var binder = new Binder(parent);
            var statement = binder.BindStatement(syntax.Statement);
            var diagnostics = binder.Diagnostics.ToImmutableArray();
            var symbols = binder._scope.GetDeclaredSymbols();
            return new BoundGlobalScope(previous, diagnostics, symbols, statement);
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
                case ExternDeclarationSyntax e:
                    return BindExternDeclaration(e);
                case FunctionDeclarationSyntax f:
                    return BindFunctionDeclaration(f);
                default:
                    throw new Exception($"Unexpected syntax {stmt.GetType()}");
            }
        }

        private BoundStatement BindFunctionDeclaration(FunctionDeclarationSyntax syntax)
        {
            var function = BindPrototype(syntax.Prototype);
            if (function == null)
                return new BoundExpressionStatement(new BoundLiteralExpression(0));

            _scope = new BoundScope(_scope);
            var body = BindBlockStatement(syntax.Body);
            _scope = _scope.Parent;

            return new BoundFunctionDeclaration(function, body);
        }

        private FunctionSymbol BindPrototype(PrototypeSyntax syntax)
        {
            var typeIdentifierToken = syntax.ReturnType.TypeOrIdentifierToken;
            var typeName = typeIdentifierToken.Text;

            if (!_scope.TryLookup(typeName, out var symbol))
            {
                Diagnostics.ReportUndefinedName(typeIdentifierToken.Span, typeName);
                return null;
            }

            if (!(symbol is TypeSymbol type))
            {
                Diagnostics.ReportUnexpectedSymbol(typeIdentifierToken.Span, symbol.GetType().Name, nameof(TypeSymbol));
                return null;
            }

            var identifierToken = syntax.IdentifierToken;
            var name = identifierToken.Text;

            var function = new FunctionSymbol(type, name);

            if (!_scope.TryDeclare(function))
            {
                Diagnostics.ReportSymbolAlreadyDeclared(identifierToken.Span, name);
            }

            return function;
        }


        private BoundStatement BindExternDeclaration(ExternDeclarationSyntax syntax)
        {
            var function = BindPrototype(syntax.Prototype);
            return new BoundExternDeclaration(function);
        }

        private BoundStatement BindForStatement(ForStatementSyntax syntax)
        {
            var lowerBound = BindExpression(syntax.LowerBound, typeof(int));
            var upperBound = BindExpression(syntax.UpperBound, typeof(int));

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
            var condition = BindExpression(syntax.Condition, typeof(bool));
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
            var condition = BindExpression(syntax.Condition, typeof(bool));

            var thenBlock = BindBlockStatement(syntax.ThenBlock);

            // Example for ?: operator -> if null its null of the right expression.type
            // var elseStatement = syntax.ElseClause ?: BindStatement(syntax.ElseClause.ElseStatement);
            var elseStatement = syntax.ElseClause == null ? null : BindStatement(syntax.ElseClause.ElseStatement);
            return new BoundIfStatement(condition, thenBlock, elseStatement);
        }

        private BoundStatement BindVariableDeclaration(VariableDeclarationSyntax syntax)
        {
            var name = syntax.Identifier.Text;
            var isReadOnly = syntax.Keyword.Kind == TokenKind.LetKeyword;
            var initializer = BindExpression(syntax.Initializer);
            var variable = new VariableSymbol(name, isReadOnly, initializer.Type);

            if (!_scope.TryDeclare(variable))
            {
                Diagnostics.ReportSymbolAlreadyDeclared(syntax.Identifier.Span, name);
            }

            return new BoundVariableDeclaration(variable, initializer);
        }

        private BoundBlockStatement BindBlockStatement(BlockStatementSyntax syntax)
        {
            var statements = ImmutableArray.CreateBuilder<BoundStatement>();
            _scope = new BoundScope(_scope);

            foreach (var statementSyntax in syntax.Statements)
            {
                var statement = BindStatement(statementSyntax);
                statements.Add(statement);
            }

            _scope = _scope.Parent;

            return new BoundBlockStatement(statements.ToImmutable());
        }

        private BoundStatement BindExpressionStatement(ExpressionStatementSyntax syntax)
        {
            var expression = BindExpression(syntax.Expression);
            return new BoundExpressionStatement(expression);
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax, Type targetType)
        {
            var expression = BindExpression(syntax);
            if (expression.Type != targetType)
                Diagnostics.ReportCannotConvert(syntax.Span, expression.Type, targetType);
            return expression;
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax)
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
                    return BindNameExpression(n);
                case ParenthesizedExpressionSyntax p:
                    return BindParenthesizedExpression(p);
                case InvokeExpressionSyntax i:
                    return BindInvokeExpression(i);
                default:
                    throw new Exception($"Unexpected syntax {syntax.GetType()}");
            }
        }

        private BoundExpression BindInvokeExpression(InvokeExpressionSyntax syntax)
        {
            if (!(syntax.Target is NameExpressionSyntax n))
            {
                Diagnostics.ReportFunctionNameExpected(syntax.Target.Span);
                return new BoundLiteralExpression(0);
            }

            var function = GetSymbol<FunctionSymbol>(n.IdentifierToken);
            if (function == null)
                return new BoundLiteralExpression(0);

            return new BoundInvokeExpression(function);
        }

        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }

        private T GetSymbol<T>(Token identifierToken) where T : Symbol
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

            if (!(symbol is T s))
            {
                Diagnostics.ReportUnexpectedSymbol(identifierToken.Span, symbol.GetType().Name, typeof(T).Name);
                return null;
            }

            return s;
        }

        private BoundVariableExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            var variable = GetSymbol<VariableSymbol>(syntax.IdentifierToken);
            return new BoundVariableExpression(variable);
        }

        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var value = BindExpression(syntax.Expression);

            if (!_scope.TryLookup(name, out var symbol))
            {
                Diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return value;
            }

            if (!(symbol is VariableSymbol variable))
            {
                Diagnostics.ReportUnexpectedSymbol(syntax.IdentifierToken.Span, symbol.GetType().Name, nameof(VariableSymbol));
                return value;
            }

            if (variable.IsReadOnly)
            {
                Diagnostics.ReportCannotAssign(syntax.EqualsToken.Span, name);
                return value;
            }

            if (value.Type != variable.Type)
            {
                Diagnostics.ReportCannotConvert(syntax.Expression.Span, value.Type, variable.Type);
                return value;
            }

            return new BoundAssignmentExpression(variable, value);
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var token = syntax.LiteralToken;
            object value = null;

            switch (token.Kind)
            {
                case TokenKind.TrueKeyword:
                case TokenKind.FalseKeyword:
                    value = token.Kind == TokenKind.TrueKeyword;
                    break;
                case TokenKind.Number:
                    if (!int.TryParse(token.Text, out var number))
                        Diagnostics.ReportInvalidNumber(token.Span, token.Text);
                    value = number;
                    break;
            }

            return new BoundLiteralExpression(value);
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
            var left = BindExpression(syntax.Left);
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