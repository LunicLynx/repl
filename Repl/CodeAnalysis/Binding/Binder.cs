﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Repl.CodeAnalysis.Syntax;

namespace Repl.CodeAnalysis.Binding
{
    public abstract class BoundTreeRewriter
    {
        public virtual BoundStatement RewriteStatement(BoundStatement statement)
        {
            switch (statement)
            {
                case BoundVariableDeclaration v: return RewriteVariableDeclaration(v);
                case BoundIfStatement i: return RewriteIfStatement(i);
                case BoundWhileStatement w: return RewriteWhileStatement(w);
                case BoundLoopStatement l: return RewriteLoopStatement(l);
                case BoundBreakStatement b: return RewriteBreakStatement(b);
                case BoundContinueStatement c: return RewriteContinueStatement(c);
                case BoundBlockStatement b: return RewriteBlockStatement(b);
                case BoundForStatement f: return RewriteForStatement(f);
                case BoundExpressionStatement e: return RewriteExpressionStatement(e);
                default: throw new Exception($"Unexpected node '{statement.GetType().Name}'");
            }
        }

        protected virtual BoundStatement RewriteIfStatement(BoundIfStatement node)
        {
            var condition = RewriteExpression(node.Condition);
            var thenBlock = RewriteBlockStatement(node.ThenBlock);
            var elseStatement = RewriteStatement(node.ElseStatement);
            if (condition == node.Condition && thenBlock == node.ThenBlock && elseStatement == node.ElseStatement)
                return node;
            return new BoundIfStatement(condition, thenBlock, elseStatement);
        }

        protected virtual BoundStatement RewriteForStatement(BoundForStatement node)
        {
            var lowerBound = RewriteExpression(node.LowerBound);
            var upperBound = RewriteExpression(node.UpperBound);
            var body = RewriteBlockStatement(node.Body);
            if (lowerBound == node.LowerBound && upperBound == node.UpperBound && body == node.Body)
                return node;
            return new BoundForStatement(node.Variable, lowerBound, upperBound, body);
        }

        protected virtual BoundStatement RewriteExpressionStatement(BoundExpressionStatement node)
        {
            var expression = RewriteExpression(node.Expression);
            if (expression == node.Expression)
                return node;
            return new BoundExpressionStatement(expression);
        }

        protected virtual BoundBlockStatement RewriteBlockStatement(BoundBlockStatement node)
        {
            var changed = false;
            var result = ImmutableArray.CreateBuilder<BoundStatement>();
            foreach (var boundStatement in node.Statements)
            {
                var statement = RewriteStatement(boundStatement);
                if (statement != boundStatement)
                    changed = true;
                result.Add(statement);
            }

            return changed ? new BoundBlockStatement(result.ToImmutable()) : node;
        }

        protected virtual BoundStatement RewriteContinueStatement(BoundContinueStatement node)
        {
            return node;
        }

        protected virtual BoundStatement RewriteBreakStatement(BoundBreakStatement node)
        {
            return node;
        }

        protected virtual BoundStatement RewriteLoopStatement(BoundLoopStatement node)
        {
            var body = RewriteBlockStatement(node.Body);
            if (body == node.Body)
                return node;
            return new BoundLoopStatement(body);
        }

        protected virtual BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            var condition = RewriteExpression(node.Condition);
            var body = RewriteBlockStatement(node.Body);
            if (condition == node.Condition && body == node.Body)
                return node;
            return new BoundWhileStatement(condition, body);
        }

        protected virtual BoundStatement RewriteVariableDeclaration(BoundVariableDeclaration node)
        {
            var initializer = RewriteExpression(node.Initializer);
            if (initializer == node.Initializer)
                return node;
            return new BoundVariableDeclaration(node.Variable, initializer);
        }

        public virtual BoundExpression RewriteExpression(BoundExpression statement)
        {
            switch (statement)
            {
                case BoundAssignmentExpression a: return RewriteAssignmentExpression(a);
                case BoundBinaryExpression b: return RewriteBinaryExpression(b);
                case BoundUnaryExpression u: return RewriteUnaryExpression(u);
                case BoundLiteralExpression l: return RewriteLiteralExpression(l);
                case BoundVariableExpression v: return RewriteVariableExpression(v);
                default: throw new Exception($"Unexpected node '{statement.GetType().Name}'");
            }
        }

        protected virtual BoundExpression RewriteVariableExpression(BoundVariableExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteLiteralExpression(BoundLiteralExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteUnaryExpression(BoundUnaryExpression node)
        {
            var operand = RewriteExpression(node.Operand);
            if (operand == node.Operand)
                return node;
            return new BoundUnaryExpression(node.Operator, operand);
        }

        protected virtual BoundExpression RewriteBinaryExpression(BoundBinaryExpression node)
        {
            var left = RewriteExpression(node.Left);
            var right = RewriteExpression(node.Right);
            if (left == node.Left && right == node.Right)
                return node;
            return new BoundBinaryExpression(left, node.Operator, right);
        }

        protected virtual BoundExpression RewriteAssignmentExpression(BoundAssignmentExpression node)
        {
            var expression = RewriteExpression(node.Expression);
            if (expression == node.Expression)
                return node;
            return new BoundAssignmentExpression(node.Variable, expression);
        }
    }

    public class Binder
    {
        private BoundScope _scope;

        public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

        public Binder(BoundScope parent)
        {
            _scope = new BoundScope(parent);
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnitSyntax syntax)
        {
            var parent = CreateParentScopes(previous);
            var binder = new Binder(parent);
            var statement = binder.BindStatement(syntax.Statement);
            var diagnostics = binder.Diagnostics.ToImmutableArray();
            var variables = binder._scope.GetDeclaredVariables();
            return new BoundGlobalScope(previous, diagnostics, variables, statement);
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
                foreach (var v in previous.Variables)
                {
                    scope.TryDeclare(v);
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
                default:
                    throw new Exception($"Unexpected syntax {stmt.GetType()}");
            }
        }

        private BoundStatement BindForStatement(ForStatementSyntax syntax)
        {
            var lowerBound = BindExpression(syntax.LowerBound);
            var upperBound = BindExpression(syntax.UpperBound);

            _scope = new BoundScope(_scope);

            var variable = new VariableSymbol(syntax.IdentifierToken.Text, false, lowerBound.Type);
            _scope.TryDeclare(variable);

            var body = BindBlockStatement(syntax.Body);

            _scope = _scope.Parent;

            return new BoundForStatement(variable, lowerBound, upperBound, body);
        }

        private BoundStatement BindContinueStatement(ContinueStatementSyntax syntax)
        {
            return new BoundContinueStatement();
        }

        private BoundStatement BindBreakStatement(BreakStatementSyntax syntax)
        {
            return new BoundBreakStatement();
        }

        private BoundStatement BindWhileStatement(WhileStatementSyntax syntax)
        {
            var condition = BindExpression(syntax.Condition, typeof(bool));
            var body = BindBlockStatement(syntax.Body);
            return new BoundWhileStatement(condition, body);
        }

        private BoundStatement BindLoopStatement(LoopStatementSyntax syntax)
        {
            var body = BindBlockStatement(syntax.Body);
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
                Diagnostics.ReportVariableAlreadyDeclared(syntax.Identifier.Span, name);
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

        private BoundExpression BindExpression(ExpressionSyntax expr)
        {
            switch (expr)
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
                default:
                    return null;
            }
        }

        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            if (string.IsNullOrEmpty(name))
            {
                // Token was inserted by parser
                return new BoundLiteralExpression(0);
            }

            if (!_scope.TryLookup(name, out var variable))
            {
                Diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new BoundLiteralExpression(0);
            }

            return new BoundVariableExpression(variable);
        }

        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var value = BindExpression(syntax.Expression);

            if (!_scope.TryLookup(name, out var variable))
            {
                Diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
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

    public class BoundForStatement : BoundStatement
    {
        public VariableSymbol Variable { get; }
        public BoundExpression LowerBound { get; }
        public BoundExpression UpperBound { get; }
        public BoundBlockStatement Body { get; }

        public BoundForStatement(VariableSymbol variable, BoundExpression lowerBound, BoundExpression upperBound, BoundBlockStatement body)
        {
            Variable = variable;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Body = body;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return LowerBound;
            yield return UpperBound;
            yield return Body;
        }
    }
}