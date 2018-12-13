using System.Collections.Generic;
using System.Linq;
using Repl.CodeAnalysis.Syntax;

namespace Repl.CodeAnalysis.Binding
{
    public class Binder
    {
        private readonly Dictionary<VariableSymbol, object> _variables;

        public List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();

        public Binder(Dictionary<VariableSymbol, object> variables)
        {
            _variables = variables;
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
            var variable = _variables.Keys.FirstOrDefault(v => v.Name == name);

            if (variable == null)
            {
                Diagnostics.ReportUndefinedName(nameExpressionSyntax.IdentifierToken.Span, name);
                return new BoundLiteralExpression(0);
            }

            return new BoundVariableExpression(variable);
        }

        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax assignmentExpressionSyntax)
        {
            var name = assignmentExpressionSyntax.IdentifierToken.Text;

            var count = Diagnostics.Count;

            var value = BindExpression(assignmentExpressionSyntax.Expression);

            // if diagnostics changed we cant declare the variable
            if (Diagnostics.Count != count)
                return new BoundLiteralExpression(0);

            var oldVariable = _variables.Keys.FirstOrDefault(v => v.Name == name);
            if (oldVariable != null)
            {
                _variables.Remove(oldVariable);
            }

            var variable = new VariableSymbol(name, value.Type);
            _variables[variable] = null;

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