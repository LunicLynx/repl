using System.Collections;
using System.Collections.Generic;
using Eagle.CodeAnalysis.Syntax;
using Eagle.CodeAnalysis.Text;

namespace Eagle.CodeAnalysis
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

        public void ReportUnterminatedString(TextLocation location)
        {
            var message = "Unterminated string literal.";
            Report(location, message);
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

        public void ReportParameterAlreadyDeclared(TextLocation location, string parameterName)
        {
            var message = $"A parameter with the name '{parameterName}' already exists.";
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

        public void ReportInvalidBreakOrContinue(TextLocation location, string text)
        {
            var message = $"The keyword '{text}' can only be used inside of loops.";
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

        public void ReportWrongArgumentCount(TextLocation location, string name, int expectedCount, int actualCount)
        {
            var message = $"Function '{name}' requires {expectedCount} arguments but was given {actualCount}.";
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

        public void ReportCannotConvertImplicitly(TextLocation location, TypeSymbol fromType, TypeSymbol toType)
        {
            var message = $"Cannot convert type '{fromType}' to '{toType}'. An explicit conversion exists (are you missing a cast?)";
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

        public void ReportInvalidExpressionStatement(TextLocation location)
        {
            var message = $"Only assignment and call expressions can be used as a statement.";
            Report(location, message);
        }

        public void ReportOnlyOneFileCanHaveGlobalStatements(TextLocation location)
        {
            var message = $"At most one file can have global statements.";
            Report(location, message);
        }

        public void ReportMainMustHaveCorrectSignature(TextLocation location)
        {
            var message = $"main must not take arguments and not return anything.";
            Report(location, message);
        }

        public void ReportCannotMixMainAndGlobalStatements(TextLocation location)
        {
            var message = $"Cannot declare main function when global statements are used.";
            Report(location, message);
        }

        public void ReportAllPathsMustReturn(TextLocation location)
        {
            var message = "Not all code paths return a value.";
            Report(location, message);
        }

        public void ReportUndefinedType(TextLocation location, string name)
        {
            var message = $"Type '{name}' doesn't exist.";
            Report(location, message);
        }

        public void ReportInvalidReturnExpression(TextLocation location, string functionName)
        {
            var message = $"Since the function '{functionName}' does not return a value the 'return' keyword cannot be followed by an expression.";
            Report(location, message);
        }

        public void ReportMissingReturnExpression(TextLocation location, TypeSymbol returnType)
        {
            var message = $"An expression of type '{returnType}' is expected.";
            Report(location, message);
        }

        public void ArrayMustHaveAtLeastOneDimension(TextLocation location)
        {
            var message = $"Array must have at least one dimension.";
            Report(location, message);
        }

        public void ReportInvalidEscapeSequence(TextLocation location)
        {
            var message = "Escape sequence is invalid.";
            Report(location, message);
        }

        public void ReportUnterminatedChar(TextLocation location)
        {
            var message = "Unterminated character literal.";
            Report(location, message);
        }

        public void CannotReadSetOnlyProperty(TextLocation location)
        {
            var message = "Cannot read set only property.";
            Report(location, message);
        }

        public void CannotReadSetOnlyIndexer(TextLocation location)
        {
            var message = "Cannot read set only indexer.";
            Report(location, message);
        }

        public void ReportMissingExpectedFunction(TextLocation location, string functionName)
        {
            var message = $"The expect function '{functionName}' was not found!";
            Report(location, message);
        }
    }
}