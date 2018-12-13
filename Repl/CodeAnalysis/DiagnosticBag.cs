using System;
using System.Collections;
using System.Collections.Generic;
using Repl.CodeAnalysis.Syntax;
using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis
{
    public class DiagnosticBag : IEnumerable<Diagnostic>
    {
        readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator()
        {
            return _diagnostics.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Report(TextSpan span, string message)
        {
            _diagnostics.Add(new Diagnostic(span, message));
        }

        public void ReportUnexpectedCharacter(TextSpan span, char c)
        {
            Report(span, $"Unexpected character '{c}'.");
        }

        public void ReportUnexpectedToken(Token token)
        {
            Report(token.Span, $"Unexpected token '{token.Kind}'");
        }

        public void ReportUndefinedUnaryOperator(Token operatorToken,
            Type type)
        {
            Report(operatorToken.Span, $"Unary operator '{operatorToken.Text}' is not defined for type '{type}'.");
        }

        public void ReportUndefinedBinaryOperator(Token operatorToken,
            Type leftType, Type rightType)
        {
            Report(operatorToken.Span, $"Binary operator '{operatorToken.Text}' is not defined for types '{leftType}' and '{rightType}'.");
        }

        public void ReportInvalidNumber(TextSpan span, string text)
        {
            Report(span, $"The number '{text}' is no a valid Number");
        }

        public void ReportUndefinedName(TextSpan span, string name)
        {
            var message = $"Variable '{name}' doesn't exist.";
            Report(span, message);
        }

        public void ReportVariableAlreadyDeclared(TextSpan span, string name)
        {
            var message = $"Variable '{name}' already declared.";
            Report(span, message);
        }
    }
}