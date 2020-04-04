using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Repl.CodeAnalysis.Binding;
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

        public void Report(TextLocation location, string message)
        {
            var diagnostic = new Diagnostic(location, message);
            _diagnostics.Add(diagnostic);
        }

        public void ReportUnexpectedCharacter(TextLocation location, char c)
        {
            Report(location, $"Unexpected character '{c}'.");
        }

        public void ReportUnexpectedToken(TextLocation location, TokenKind actual, TokenKind expected)
        {
            Report(location, $"Unexpected token <{actual}>, expected <{expected}>.");
        }

        public void ReportUndefinedUnaryOperator(TextLocation location, string operatorText, TypeSymbol type)
        {
            Report(location, $"Unary operator '{operatorText}' is not defined for type '{type}'.");
        }

        public void ReportUndefinedBinaryOperator(TextLocation location, string operatorText, TypeSymbol leftType, TypeSymbol rightType)
        {
            Report(location, $"Binary operator '{operatorText}' is not defined for types '{leftType}' and '{rightType}'.");
        }

        public void ReportInvalidNumber(TextLocation location, string text)
        {
            Report(location, $"The number '{text}' is no a valid Number");
        }

        public void ReportUndefinedName(TextLocation location, string name)
        {
            var message = $"Symbol '{name}' doesn't exist.";
            Report(location, message);
        }

        public void ReportCannotConvert(TextLocation location, TypeSymbol from, TypeSymbol to)
        {
            var message = $"Cannot convert type '{from}' to '{to}'.";
            Report(location, message);
        }

        public void ReportSymbolAlreadyDeclared(TextLocation location, string name)
        {
            var message = $"Symbol '{name}' is already declared.";
            Report(location, message);
        }

        public void ReportCannotAssign(TextLocation location, string name)
        {
            var message = $"Variable '{name}' is read-only and cannot be assigned to.";
            Report(location, message);
        }

        public void ReportContinueOutsideLoop(TextLocation location)
        {
            var message = "The 'continue'-Statement can only be used inside a loop";
            Report(location, message);
        }

        public void ReportBreakOutsideLoop(TextLocation location)
        {
            var message = "The 'break'-Statement can only be used inside a loop";
            Report(location, message);
        }

        public void ReportExpectedTypeOrIdentifier(TextLocation location)
        {
            var message = "Expected type or identifier";
            Report(location, message);
        }

        public void ReportUnexpectedSymbol(TextLocation location, string actual, params string[] expected)
        {
            var message = $"Unexpected symbol '{actual}', expected '{string.Join("', '", expected)}'.";
            Report(location, message);
        }

        public void ReportFunctionNameExpected(TextLocation location)
        {
            var message = "Function name expected.";
            Report(location, message);
        }

        public void ReportParameterCount(TextLocation location, string name, int expected, int actual)
        {
            var message = $"Function '{name}' is called with {actual} but only accepts {expected}.";
            Report(location, message);
        }

        public void ReportNotSupported(TextLocation location)
        {
            var message = "The given expression is not supported.";
            Report(location, message);
        }

        public void ReportExpressionIsNotCompileTimeConstant(TextLocation location)
        {
            var message = "The given expression is not compile time constant.";
            Report(location, message);
        }

        public void ReportMemberMustBeTyped(TextLocation location)
        {
            var message = "Either annotate the member with a type or initialize it.";
            Report(location, message);
        }

        public void ReportCyclicDependency(TextLocation location)
        {
            var message = "Cyclic dependency.";
            Report(location, message);
        }

        public void ReportTypeDoesNotHaveMember(TextLocation location, TypeSymbol type, string memberName)
        {
            var message = $"Type '{type}' doesn't have a member called '{memberName}'.";
            Report(location, message);
        }

        public void ReportThisNotAllowed(TextLocation location)
        {
            var message = "This is not allowed in this scope.";
            Report(location, message);
        }

        public void ReportUnsupportedCast(TextLocation location, TypeSymbol from, TypeSymbol to)
        {
            var message = $"The cast from '{from}' to '{to}' is not supported.";
            Report(location, message);
        }

        public void ReportCannotConvertImplicitly(TextLocation location, TypeSymbol from, TypeSymbol to)
        {
            var message = $"The cast from '{from}' to '{to}' is not implicitly supported.";
            Report(location, message);
        }

        public void ReportNotLValue(TextLocation location, Symbol symbol)
        {
            var message = $"Reference '{symbol.Name}' is a '{symbol.Kind}'. The assignment target must be an assignable variable, field, property or indexer.";
            Report(location, message);
        }

        public void ReportNotAFunction(TextLocation location, Symbol symbol)
        {
            var message = $"Reference '{symbol.Name}' is a '{symbol.Kind}'. The target must be a function, method or delegate.";
            Report(location, message);
        }

        public void ReportExpressionMustHaveValue(TextLocation location)
        {
            var message = "Expression must have a value.";
            Report(location, message);
        }
    }
}