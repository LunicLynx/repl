using System;
using System.Collections.Generic;
using Repl.CodeAnalysis.Syntax;
using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis
{
    public static class DiagnosticCollectionExtensions
    {
        public static void Report(this ICollection<Diagnostic> self, TextSpan span, string message)
        {
            self.Add(new Diagnostic(span, message));
        }

        public static void ReportUnexpectedCharacter(this ICollection<Diagnostic> self, TextSpan span, char c)
        {
            self.Report(span, $"Unexpected character '{c}'.");
        }

        public static void ReportUnexpectedToken(this ICollection<Diagnostic> self, Token token)
        {
            self.Report(token.Span, $"Unexpected token '{token.Kind}'");
        }

        public static void ReportUndefinedUnaryOperator(this ICollection<Diagnostic> self, Token operatorToken,
            Type type)
        {
            self.Report(operatorToken.Span, $"The unary operator '{operatorToken.Text}' is undefined for '{type}'.");
        }

        public static void ReportUndefinedBinaryOperator(this ICollection<Diagnostic> self, Token operatorToken,
            Type leftType, Type rightType)
        {
            self.Report(operatorToken.Span, $"The binary operator '{operatorToken.Text}' is undefined for '{leftType}' and '{rightType}'.");
        }

        public static void ReportInvalidNumber(this ICollection<Diagnostic> self, TextSpan span, string text)
        {
            self.Report(span, $"The number '{text}' is no a valid Number");
        }

        public static void ReportUndefinedName(this ICollection<Diagnostic> self, TextSpan span, string name)
        {
            var message = $"Variable '{name}' doesn't exist.";
            self.Report(span, message);
        }
    }
}