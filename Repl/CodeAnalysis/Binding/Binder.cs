using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using Repl.CodeAnalysis.Syntax;

namespace Repl.CodeAnalysis.Binding
{
    public class Binder
    {
        private readonly BoundScope _scope;

        public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

        public Binder(BoundScope parent)
        {
            _scope = new BoundScope(parent);
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnitSyntax syntax)
        {
            var parent = CreateParentScopes(previous);
            var binder = new Binder(parent);
            var expression = binder.BindExpression(syntax.Expression);
            var diagnostics = binder.Diagnostics.ToImmutableArray();
            var variables = binder._scope.GetDeclaredVariables();
            return new BoundGlobalScope(previous, diagnostics, variables, expression);
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

        public BoundExpression BindExpression(ExpressionSyntax expr)
        {
            if (expr is BinaryExpressionSyntax b)
                return BindBinaryExpression(b);
            if (expr is UnaryExpressionSyntax u)
                return BindUnaryExpression(u);
            if (expr is LiteralExpressionSyntax l)
                return BindLiteralExpression(l);
            if (expr is AssignmentExpressionSyntax a)
                return BindAssignmentExpression(a);
            if (expr is NameExpressionSyntax n)
                return BindNameExpression(n);
            if (expr is ParenthesizedExpressionSyntax p)
                return BindParenthesizedExpression(p);
            return null;
        }

        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax parenthesizedExpressionSyntax)
        {
            return BindExpression(parenthesizedExpressionSyntax.Expression);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax nameExpressionSyntax)
        {
            var name = nameExpressionSyntax.IdentifierToken.Text;

            if (!_scope.TryLookup(name, out var variable))
            {
                Diagnostics.ReportUndefinedName(nameExpressionSyntax.IdentifierToken.Span, name);
                return new BoundLiteralExpression(0);
            }

            return new BoundVariableExpression(variable);
        }

        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax assignmentExpressionSyntax)
        {
            var name = assignmentExpressionSyntax.IdentifierToken.Text;
            var value = BindExpression(assignmentExpressionSyntax.Expression);

            if (!_scope.TryLookup(name, out var variable))
            {
                variable = new VariableSymbol(name, value.Type);
                _scope.TryDeclare(variable);
            }

            if (value.Type != variable.Type)
            {
                Diagnostics.ReportCannotConvert(assignmentExpressionSyntax.Expression.Span, value.Type, variable.Type);
                return value;
            }

            return new BoundAssignmentExpression(variable, value);
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax literalExpressionSyntax)
        {
            var token = literalExpressionSyntax.LiteralToken;
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

            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, operand.Type);
            if (boundOperator == null)
            {
                Diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken, operand.Type);
                return operand;
            }

            return new BoundUnaryExpression(boundOperator, operand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax binaryExpressionSyntax)
        {
            var left = BindExpression(binaryExpressionSyntax.Left);
            var right = BindExpression(binaryExpressionSyntax.Right);

            var boundOperator = BoundBinaryOperator.Bind(binaryExpressionSyntax.OperatorToken.Kind, left.Type, right.Type);
            if (boundOperator == null)
            {
                Diagnostics.ReportUndefinedBinaryOperator(binaryExpressionSyntax.OperatorToken, left.Type, right.Type);
                return left;
            }

            return new BoundBinaryExpression(left, boundOperator, right);
        }
    }
}