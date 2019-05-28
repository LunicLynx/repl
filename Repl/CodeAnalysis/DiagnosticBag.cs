using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Repl.CodeAnalysis.Syntax;
using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis
{
    public class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

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
            if (_diagnostics.Any(d => d.Span == span)) return;
            _diagnostics.Add(new Diagnostic(span, message));
        }

        public void ReportUnexpectedCharacter(TextSpan span, char c)
        {
            Report(span, $"Unexpected character '{c}'.");
        }

        public void ReportUnexpectedToken(TextSpan span, TokenKind actual, TokenKind expected)
        {
            Report(span, $"Unexpected token '{actual}', expected '{expected}'.");
        }

        public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, TypeSymbol type)
        {
            Report(span, $"Unary operator '{operatorText}' is not defined for type '{type}'.");
        }

        public void ReportUndefinedBinaryOperator(TextSpan span, string operatorText, TypeSymbol leftType, TypeSymbol rightType)
        {
            Report(span, $"Binary operator '{operatorText}' is not defined for types '{leftType}' and '{rightType}'.");
        }

        public void ReportInvalidNumber(TextSpan span, string text)
        {
            Report(span, $"The number '{text}' is no a valid Number");
        }

        public void ReportUndefinedName(TextSpan span, string name)
        {
            var message = $"Symbol '{name}' doesn't exist.";
            Report(span, message);
        }

        public void ReportCannotConvert(TextSpan span, TypeSymbol from, TypeSymbol to)
        {
            var message = $"Cannot convert type '{from}' to '{to}'.";
            Report(span, message);
        }

        public void ReportSymbolAlreadyDeclared(TextSpan span, string name)
        {
            var message = $"Symbol '{name}' is already declared.";
            Report(span, message);
        }

        public void ReportCannotAssign(TextSpan span, string name)
        {
            var message = $"Variable '{name}' is read-only and cannot be assigned to.";
            Report(span, message);
        }

        public void ReportContinueOutsideLoop(TextSpan span)
        {
            var message = "The 'continue'-Statement can only be used inside a loop";
            Report(span, message);
        }

        public void ReportBreakOutsideLoop(TextSpan span)
        {
            var message = "The 'break'-Statement can only be used inside a loop";
            Report(span, message);
        }

        public void ReportExpectedTypeOrIdentifier(TextSpan span)
        {
            var message = "Expected type or identifier";
            Report(span, message);
        }

        public void ReportUnexpectedSymbol(TextSpan span, string actual, params string[] expected)
        {
            var message = $"Unexpected symbol '{actual}', expected '{string.Join("', '", expected)}'.";
            Report(span, message);
        }

        public void ReportFunctionNameExpected(TextSpan span)
        {
            var message = "Function name expected.";
            Report(span, message);
        }

        public void ReportParameterCount(TextSpan span, string name, int expected, int actual)
        {
            var message = $"Function '{name}' is called with {actual} but only accepts {expected}.";
            Report(span, message);
        }

        public void ReportNotSupported(TextSpan span)
        {
            var message = "The given expression is not supported.";
            Report(span, message);
        }

        public void ReportExpressionIsNotCompileTimeConstant(TextSpan span)
        {
            var message = "The given expression is not compile time constant.";
            Report(span, message);
        }

        public void ReportMemberMustBeTyped(TextSpan span)
        {
            var message = "Either annotate the member with a type or initialize it.";
            Report(span, message);
        }

        public void ReportCyclicDependency(TextSpan span)
        {
            var message = "Cyclic dependency.";
            Report(span, message);
        }

        public void ReportTypeDoesNotHaveMember(TextSpan span, TypeSymbol type, string memberName)
        {
            var message = $"Type '{type}' doesn't have a member called '{memberName}'.";
            Report(span, message);
        }

        public void ReportThisNotAllowed(TextSpan span)
        {
            var message = "This is not allowed in this scope.";
            Report(span, message);
        }

        public void ReportUnsupportedCast(TextSpan span, TypeSymbol from, TypeSymbol to)
        {
            var message = $"The cast from '{from}' to '{to}' is not supported.";
            Report(span, message);
        }

        public void ReportCannotConvertImplicitly(TextSpan span, TypeSymbol from, TypeSymbol to)
        {
            var message = $"The cast from '{from}' to '{to}' is not implicitly supported.";
            Report(span, message);
        }
    }
}