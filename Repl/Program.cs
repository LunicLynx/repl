using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using LLVMSharp;
using XLang.Codegen;
using XLang.Codegen.Llvm;

namespace Repl
{

    /*
     * Possbile syntax
     *
     * Not null shortcut
     * something !? DoSomething();
     *
     */
    class Program
    {
        static void Main(string[] args)
        {
            //string x = "";
            //string y = null;

            //var s = x ?? throw new InvalidOperationException();

            //var s1 = x?    .GetType(); 

            //             v   
            //           e o e
            //var type = x ? y; // .GetType();
            //           e o e o e       
            //var type = x ? y : z; // .GetType();

            Statics.InitializeX86Target();

            var variables = new Dictionary<VariableSymbol, object>();
            var variablePtrs = new Dictionary<VariableSymbol, Value>();

            XModule module = new XModule("test");
            var functionType = new FunctionType(XType.Int32);
            var function = module.AddFunction(functionType, "__anon_expr");
            var basicBlock = function.AppendBasicBlock();
            using (var builder = new Builder())
            {
                builder.PositionAtEnd(basicBlock);

                while (true)
                {
                    Console.Write("> ");
                    var input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        break;
                    }

                    var tree = SyntaxTree.Parse(input);
                    var binder = new Binder(variables);
                    var expression = binder.BindExpression(tree.Root.Expression);

                    var diagnostics = tree.Diagnostics.Concat(binder.Diagnostics).ToList();

                    if (!diagnostics.Any())
                    {
                        Print(tree.Root);

                        //var keys = variablePtrs.Keys.ToArray();
                        //foreach (var key in keys)
                        //    if (!variables.ContainsKey(key))
                        //        variablePtrs.Remove(key);

                        var codeGenerator = new CodeGenerator(builder, variablePtrs);
                        var value = codeGenerator.Generate(expression);

                        var v = builder.Ret(value);

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        //code.Print(Console.Out);
                        //codeGenerator.Module.Print(Console.Out);
                        basicBlock.Print(Console.Out);
                        Console.WriteLine();
                        Console.ResetColor();

                        var evaluator = new Evaluator(expression, variables);
                        var result = evaluator.Evaluate();
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(result);
                        Console.ResetColor();

                        var objFilename = "entry.obj";
                        if (module.TryEmitObj(objFilename, out var error))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Cl.InvokeCl(objFilename);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Cl.InvokeMain();
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(error);
                            Console.ResetColor();
                        }

                        v.RemoveFromParent();
                    }
                    else
                    {
                        foreach (var diagnostic in diagnostics)
                        {
                            var prefix = input.Substring(0, diagnostic.Span.Start);
                            var error = input.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
                            var suffix = input.Substring(diagnostic.Span.Start + diagnostic.Span.Length);

                            Console.Write("    " + prefix);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(error);
                            Console.ResetColor();
                            Console.WriteLine(suffix);
                            Console.WriteLine(diagnostic.Message);
                        }
                    }
                }
            }
        }

        private static void Print(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.Write(indent);
            Console.Write(marker);

            Console.ForegroundColor = node is Token ? ConsoleColor.Blue : ConsoleColor.Cyan;

            if (node is Token t)
            {
                Console.Write(t);
            }
            else
            {
                Console.Write(node.GetType().Name);
            }

            Console.ResetColor();

            Console.WriteLine();

            indent += isLast ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                Print(child, indent, child == lastChild);
        }
    }

    class SyntaxFacts
    {
        public static int GetUnaryOperatorPrecedence(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Plus:
                case TokenKind.Minus:
                case TokenKind.Bang:
                    return 6;

                default: return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.EqualsEquals:
                case TokenKind.BangEquals:
                    return 1;
                case TokenKind.PipePipe:
                    return 2;
                case TokenKind.AmpersandAmpersand:
                    return 3;
                case TokenKind.Plus:
                case TokenKind.Minus:
                    return 4;
                case TokenKind.Star:
                case TokenKind.Slash:
                    return 5;
                default: return 0;
            }
        }
    }

    public class ParserBase
    {
        protected Token[] Tokens;

        private int _position = 0;
        public List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();

        protected Token NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        protected Token Current => Peek(0);

        protected Token Peek(int offset)
        {
            var position = _position + offset;

            return position >= Tokens.Length
                ? Tokens[Tokens.Length - 1]
                : Tokens[position];
        }

        protected Token MatchToken(TokenKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            return new Token(kind, new TextSpan(0, 0), "");
        }
    }

    public class Evaluator
    {
        public BoundExpression Expression { get; }
        public Dictionary<VariableSymbol, object> Variables { get; }

        public Evaluator(BoundExpression expression, Dictionary<VariableSymbol, object> variables)
        {
            Expression = expression;
            Variables = variables;
        }

        public object Evaluate()
        {
            return EvaluateExpression(Expression);
        }

        private object EvaluateExpression(BoundExpression expression)
        {
            if (expression is BoundBinaryExpression b)
                return EvaluateBinaryExpression(b);
            if (expression is BoundUnaryExpression u)
                return EvaluateUnaryExpression(u);
            if (expression is BoundLiteralExpression l)
                return EvaluateLiteralExpression(l);
            if (expression is BoundAssignmentExpression a)
                return EvaluateAssignmentExpression(a);
            if (expression is BoundVariableExpression v)
                return EvaluateVariableExpression(v);
            return 0;
        }

        private object EvaluateVariableExpression(BoundVariableExpression boundVariableExpression)
        {
            var value = Variables[boundVariableExpression.Variable];
            return value;
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression boundAssignmentExpression)
        {
            var value = EvaluateExpression(boundAssignmentExpression.Expression);

            Variables[boundAssignmentExpression.Variable] = value;

            return value;
        }

        private object EvaluateLiteralExpression(BoundLiteralExpression boundLiteralExpression)
        {
            return boundLiteralExpression.Value;
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression boundUnaryExpression)
        {
            var operand = EvaluateExpression(boundUnaryExpression.Operand);

            switch (boundUnaryExpression.Operator.Kind)
            {
                case BoundUnaryOperatorKind.Identity: return (int)operand;
                case BoundUnaryOperatorKind.Negation: return -(int)operand;
                case BoundUnaryOperatorKind.LogicalNot: return !(bool)operand;
            }

            return 0;
        }

        private object EvaluateBinaryExpression(BoundBinaryExpression boundBinaryExpression)
        {
            var left = EvaluateExpression(boundBinaryExpression.Left);
            var right = EvaluateExpression(boundBinaryExpression.Right);

            switch (boundBinaryExpression.Operator.Kind)
            {
                case BoundBinaryOperatorKind.Addition: return (int)left + (int)right;
                case BoundBinaryOperatorKind.Subtraction: return (int)left - (int)right;
                case BoundBinaryOperatorKind.Multiplication: return (int)left * (int)right;
                case BoundBinaryOperatorKind.Division: return (int)left / (int)right;
                case BoundBinaryOperatorKind.LogicalAnd: return (bool)left && (bool)right;
                case BoundBinaryOperatorKind.LogicalOr: return (bool)left || (bool)right;
                case BoundBinaryOperatorKind.Equals: return left == right;
                case BoundBinaryOperatorKind.NotEquals: return left != right;
            }

            return 0;
        }
    }

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
            return null;
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
            var value = BindExpression(assignmentExpressionSyntax.Expression);

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

    public abstract class BoundExpression
    {
        public abstract Type Type { get; }
    }

    public class BoundLiteralExpression : BoundExpression
    {
        public object Value { get; }

        public override Type Type => Value.GetType();

        public BoundLiteralExpression(object value)
        {
            Value = value;
        }
    }

    public class VariableSymbol
    {
        public string Name { get; }
        public Type Type { get; }

        public VariableSymbol(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }

    public class BoundVariableExpression : BoundExpression
    {
        public VariableSymbol Variable { get; }
        public override Type Type => Variable.Type;

        public BoundVariableExpression(VariableSymbol variable)
        {
            Variable = variable;
        }
    }

    public class BoundAssignmentExpression : BoundExpression
    {
        public VariableSymbol Variable { get; }
        public BoundExpression Expression { get; }
        public override Type Type => Expression.Type;

        public BoundAssignmentExpression(VariableSymbol variable, BoundExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }
    }

    public class BoundBinaryExpression : BoundExpression
    {
        public BoundExpression Left { get; }
        public BoundBinaryOperator Operator { get; }
        public BoundExpression Right { get; }
        public override Type Type => Operator.ResultType;

        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator @operator, BoundExpression right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
    }

    public class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryOperator Operator { get; }
        public BoundExpression Operand { get; }
        public override Type Type => Operator.ResultType;

        public BoundUnaryExpression(BoundUnaryOperator @operator, BoundExpression operand)
        {
            Operator = @operator;
            Operand = operand;
        }
    }

    public class BoundBinaryOperator
    {
        public TokenKind TokenKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ResultType { get; }

        public BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, Type type)
            : this(tokenKind, kind, type, type, type) { }

        public BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, Type type, Type resultType)
            : this(tokenKind, kind, type, type, resultType) { }

        public BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType)
        {
            TokenKind = tokenKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            ResultType = resultType;
        }

        private static readonly BoundBinaryOperator[] Operators = {
            new BoundBinaryOperator(TokenKind.Plus, BoundBinaryOperatorKind.Addition, typeof(int)),
            new BoundBinaryOperator(TokenKind.Minus, BoundBinaryOperatorKind.Subtraction, typeof(int)),
            new BoundBinaryOperator(TokenKind.Star, BoundBinaryOperatorKind.Multiplication, typeof(int)),
            new BoundBinaryOperator(TokenKind.Slash, BoundBinaryOperatorKind.Division, typeof(int)),

            new BoundBinaryOperator(TokenKind.AmpersandAmpersand, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(TokenKind.PipePipe, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),

            new BoundBinaryOperator(TokenKind.EqualsEquals, BoundBinaryOperatorKind.Equals, typeof(int),typeof(bool)),
            new BoundBinaryOperator(TokenKind.EqualsEquals, BoundBinaryOperatorKind.Equals, typeof(bool)),
            new BoundBinaryOperator(TokenKind.BangEquals, BoundBinaryOperatorKind.NotEquals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(TokenKind.BangEquals, BoundBinaryOperatorKind.NotEquals, typeof(bool)),
        };

        public static BoundBinaryOperator Bind(TokenKind operatorTokenKind, Type leftType, Type rightType)
        {
            foreach (var @operator in Operators)
            {
                if (@operator.TokenKind == operatorTokenKind && @operator.LeftType == leftType &&
                    @operator.RightType == rightType)
                    return @operator;
            }

            return null;
        }
    }

    public enum BoundBinaryOperatorKind
    {
        Addition,
        Multiplication,
        Subtraction,
        Division,
        LogicalAnd,
        LogicalOr,
        Equals,
        NotEquals
    }

    public enum BoundUnaryOperatorKind
    {
        Identity,
        Negation,
        LogicalNot
    }

    public class BoundUnaryOperator
    {
        public TokenKind TokenKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type ResultType { get; }

        public BoundUnaryOperator(TokenKind tokenKind, BoundUnaryOperatorKind kind, Type type)
            : this(tokenKind, kind, type, type)
        {

        }

        public BoundUnaryOperator(TokenKind tokenKind, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
        {
            TokenKind = tokenKind;
            Kind = kind;
            OperandType = operandType;
            ResultType = resultType;
        }

        private static readonly BoundUnaryOperator[] Operators = {
            new BoundUnaryOperator(TokenKind.Plus, BoundUnaryOperatorKind.Identity, typeof(int)),
            new BoundUnaryOperator(TokenKind.Minus, BoundUnaryOperatorKind.Negation, typeof(int)),
            new BoundUnaryOperator(TokenKind.Bang, BoundUnaryOperatorKind.LogicalNot, typeof(bool))
        };

        public static BoundUnaryOperator Bind(TokenKind operatorTokenKind, Type operandType)
        {
            foreach (var @operator in Operators)
            {
                if (@operator.TokenKind == operatorTokenKind && @operator.OperandType == operandType)
                    return @operator;
            }

            return null;
        }
    }

    public class Parser : ParserBase
    {
        public SourceText Text { get; }

        public Parser(SourceText text)
        {
            Text = text;
            var tokens = new List<Token>();
            var lexer = new Lexer(text);

            Token token;
            do
            {
                token = lexer.Lex();

                if (token.Kind != TokenKind.WhiteSpace && token.Kind != TokenKind.Bad)
                    tokens.Add(token);

            } while (token.Kind != TokenKind.Eof);

            Tokens = tokens.ToArray();
        }

        public CompilationUnitSyntax ParseCompilationUnit()
        {
            var expr = ParseExpression();
            var endOfFileToken = MatchToken(TokenKind.Eof);
            return new CompilationUnitSyntax(expr, endOfFileToken);
        }

        public ExpressionSyntax ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        public ExpressionSyntax ParseAssignmentExpression()
        {
            if (Peek(0).Kind == TokenKind.Identifier &&
                Peek(1).Kind == TokenKind.Equals)
            {
                var identifierToken = MatchToken(TokenKind.Identifier);
                var operatorToken = MatchToken(TokenKind.Equals);
                var right = ParseAssignmentExpression();
                return new AssignmentExpressionSyntax(identifierToken, operatorToken, right);
            }

            return ParseBinaryExpression();
        }

        public ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;

            var unaryOperatorPrecedence = SyntaxFacts.GetUnaryOperatorPrecedence(Current.Kind);
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseBinaryExpression(unaryOperatorPrecedence);
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }
            else
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var precedence = SyntaxFacts.GetBinaryOperatorPrecedence(Current.Kind);
                if (precedence == 0 || precedence <= parentPrecedence)
                    break;

                var operatorToken = NextToken();
                var right = ParseBinaryExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        public ExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.Kind == TokenKind.TrueKeyword ||
                Current.Kind == TokenKind.FalseKeyword)
            {
                return ParseBooleanLiteralExpression(Current.Kind);
            }

            if (Current.Kind == TokenKind.Number)
            {
                return ParseNumberLiteralExpression();
            }

            if (Current.Kind == TokenKind.Identifier)
            {
                return ParseNameExpression();
            }

            // diagnostic ?
            Diagnostics.ReportUnexpectedToken(Current);

            // generate token
            return new LiteralExpressionSyntax(new Token(TokenKind.Number, new TextSpan(0, 0), "0"));
        }

        private ExpressionSyntax ParseBooleanLiteralExpression(TokenKind kind)
        {
            var token = MatchToken(kind);
            return new LiteralExpressionSyntax(token);
        }

        private ExpressionSyntax ParseNameExpression()
        {
            var token = MatchToken(TokenKind.Identifier);
            return new NameExpressionSyntax(token);
        }

        public LiteralExpressionSyntax ParseNumberLiteralExpression()
        {
            var token = MatchToken(TokenKind.Number);
            return new LiteralExpressionSyntax(token);
        }
    }

    public class SourceText
    {
        private readonly string _text;



        private SourceText(string text)
        {
            _text = text;
            Lines = ParseLines(this, _text);
        }

        public ImmutableArray<TextLine> Lines { get; }

        public int GetLineIndex(int position)
        {
            var lower = 0;
            var upper = _text.Length - 1;

            while (lower <= upper)
            {
                var index = lower + (upper - lower) / 2;
                var start = Lines[index].Start;

                if (position == start)
                    return index;

                if (start > position)
                {
                    upper = index - 1;
                }
                else
                {
                    lower = index + 1;
                }
            }

            return lower - 1;
        }

        private static ImmutableArray<TextLine> ParseLines(SourceText sourceText, string text)
        {
            var result = ImmutableArray.CreateBuilder<TextLine>();

            var position = 0;
            var lineStart = 0;

            while (position < text.Length)
            {
                var lineBreakWidth = GetLineBreakWidth(text, position);

                if (lineBreakWidth == 0)
                {
                    position++;
                }
                else
                {
                    AddLine(sourceText, position, lineStart, lineBreakWidth);

                    position += lineBreakWidth;
                    lineStart = position;
                }
            }

            if (position >= lineStart)
                AddLine(sourceText, position, lineStart, 0);

            return result.ToImmutable();
        }

        private static void AddLine(SourceText sourceText, int position, int lineStart, int lineBreakWidth)
        {
            var lineLength = position - lineStart;
            var lineLengthIncludingLineBreak = lineLength + lineBreakWidth;
            var line = new TextLine(sourceText, lineStart, lineLength, lineLengthIncludingLineBreak);
        }

        private static int GetLineBreakWidth(string text, int position)
        {
            var c = text[position];
            var l = position + 1 >= text.Length ? '\0' : text[position + 1];

            if (c == '\r' && l == '\n')
                return 2;

            if (c == '\r' || c == '\n')
                return 1;

            return 0;
        }

        public static SourceText From(string text)
        {
            return new SourceText(text);
        }



        public override string ToString() => _text;

        public string ToString(int start, int length) => _text.Substring(start, length);
        public string ToString(TextSpan span) => ToString(span.Start, span.Length);

        public int Length => _text.Length;

        public char this[int index] => _text[index];
    }

    public class TextLine
    {
        public SourceText Text { get; }
        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length;
        public int LengthIncludingLineBreak { get; }
        public TextSpan Span => new TextSpan(Start, Length);
        public TextSpan SpanIncludingLineBreak => new TextSpan(Start, LengthIncludingLineBreak);


        public TextLine(SourceText text, int start, int length, int lengthIncludingLineBreak)
        {
            Text = text;
            Start = start;
            Length = length;
            LengthIncludingLineBreak = lengthIncludingLineBreak;
        }

        public override string ToString() => Text.ToString(Span);
    }

    public class Compilation
    {
        public SyntaxTree SyntaxTree { get; }

        public Compilation(SyntaxTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables)
        {
            var binder = new Binder(variables);
            var boundExpression = binder.BindExpression(SyntaxTree.Root.Expression);

            var diagnostics = SyntaxTree.Diagnostics.Concat(binder.Diagnostics).ToImmutableArray();
            if (diagnostics.Any())
                return new EvaluationResult(diagnostics, null);

            var evaluator = new Evaluator(boundExpression, variables);
            var value = evaluator.Evaluate();
            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
        }
    }

    public class EvaluationResult
    {
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public object Value { get; }

        public EvaluationResult(ImmutableArray<Diagnostic> diagnostics, object value)
        {
            Diagnostics = diagnostics;
            Value = value;
        }
    }

    public class SyntaxTree
    {
        public SourceText Text { get; }
        public CompilationUnitSyntax Root { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }

        private SyntaxTree(SourceText text)
        {
            var parser = new Parser(text);
            var root = parser.ParseCompilationUnit();
            var diagnostics = parser.Diagnostics.ToImmutableArray();

            Diagnostics = diagnostics;
            Text = text;
            Root = root;
        }

        public static SyntaxTree Parse(string text)
        {
            var sourceText = SourceText.From(text);
            return Parse(sourceText);
        }

        public static SyntaxTree Parse(SourceText text)
        {
            return new SyntaxTree(text);
        }

        public static IEnumerable<Token> ParseTokens(string text)
        {
            var sourceText = SourceText.From(text);
            return ParseTokens(sourceText);
        }

        public static IEnumerable<Token> ParseTokens(SourceText text)
        {
            var lexer = new Lexer(text);
            while (true)
            {
                var token = lexer.Lex();
                if (token.Kind == TokenKind.Eof)
                    break;

                yield return token;
            }
        }
    }


    public abstract class SyntaxNode
    {
        public virtual IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>();
        }
    }

    public class CompilationUnitSyntax : SyntaxNode
    {
        public ExpressionSyntax Expression { get; }
        public Token EndOfFileToken { get; }

        public CompilationUnitSyntax(ExpressionSyntax expression, Token endOfFileToken)
        {
            Expression = expression;
            EndOfFileToken = endOfFileToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Expression;
            yield return EndOfFileToken;
        }
    }

    public abstract class ExpressionSyntax : SyntaxNode
    {
        public abstract override IEnumerable<SyntaxNode> GetChildren();
    }

    public class UnaryExpressionSyntax : ExpressionSyntax
    {
        public Token OperatorToken { get; }
        public ExpressionSyntax Operand { get; }

        public UnaryExpressionSyntax(Token operatorToken, ExpressionSyntax operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }


        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OperatorToken;
            yield return Operand;
        }
    }

    public class BinaryExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Left { get; }
        public Token OperatorToken { get; }
        public ExpressionSyntax Right { get; }

        public BinaryExpressionSyntax(ExpressionSyntax left, Token operatorToken, ExpressionSyntax right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }

    public class LiteralExpressionSyntax : ExpressionSyntax
    {
        public Token LiteralToken { get; }

        public LiteralExpressionSyntax(Token literalToken)
        {
            LiteralToken = literalToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }

    public class NameExpressionSyntax : ExpressionSyntax
    {
        public Token IdentifierToken { get; }

        public NameExpressionSyntax(Token identifierToken)
        {
            IdentifierToken = identifierToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
        }
    }

    public class AssignmentExpressionSyntax : ExpressionSyntax
    {
        public Token IdentifierToken { get; }
        public Token OperatorToken { get; }
        public ExpressionSyntax Expression { get; }

        public AssignmentExpressionSyntax(Token identifierToken, Token operatorToken, ExpressionSyntax expression)
        {
            IdentifierToken = identifierToken;
            OperatorToken = operatorToken;
            Expression = expression;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return OperatorToken;
            yield return Expression;
        }
    }

    public class Diagnostic
    {
        public TextSpan Span { get; }
        public string Message { get; }

        public Diagnostic(TextSpan span, string message)
        {
            Span = span;
            Message = message;
        }
    }

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

    abstract class LexerBase
    {
        public List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();

        protected int Position = 0;

        protected readonly SourceText Text;

        protected LexerBase(SourceText text)
        {
            Text = text;
        }

        protected char Current
            => Position >= Text.Length
                ? '\0'
                : Text[Position];

        protected void Next()
        {
            Position++;
        }

        public abstract Token Lex();
    }

    class Lexer : LexerBase
    {
        private static readonly Dictionary<string, TokenKind> KeywordKinds = new Dictionary<string, TokenKind>
        {
            {"true", TokenKind.TrueKeyword},
            {"false", TokenKind.FalseKeyword}
        };

        public Lexer(SourceText text) : base(text) { }

        public override Token Lex()
        {
            var start = Position;
            var kind = TokenKind.Eof;

            var c = Current;
            Next();

            if (char.IsWhiteSpace(c))
            {
                while (char.IsWhiteSpace(Current))
                    Next();

                kind = TokenKind.WhiteSpace;
            }
            else if (char.IsDigit(c))
            {
                while (char.IsDigit(Current))
                    Next();

                kind = TokenKind.Number;
            }
            else if (IsIdentifierStart(c))
            {
                while (IsIdentifierFollow(Current))
                    Next();

                var text = Text.ToString(start, Position - start);

                kind = KeywordKinds.TryGetValue(text, out var k)
                    ? k
                    : TokenKind.Identifier;
            }
            else
            {
                switch (c)
                {
                    case '\0':
                        start = Text.Length;
                        Position = Text.Length;
                        break;
                    case '+':
                        kind = TokenKind.Plus;
                        break;
                    case '-':
                        kind = TokenKind.Minus;
                        break;
                    case '*':
                        kind = TokenKind.Star;
                        break;
                    case '/':
                        kind = TokenKind.Slash;
                        break;
                    case '=' when Current == '=':
                        Next();
                        kind = TokenKind.EqualsEquals;
                        break;
                    case '=':
                        kind = TokenKind.Equals;
                        break;
                    case '!' when Current == '=':
                        Next();
                        kind = TokenKind.BangEquals;
                        break;
                    case '!':
                        kind = TokenKind.Bang;
                        break;
                    case '&' when Current == '&':
                        Next();
                        kind = TokenKind.AmpersandAmpersand;
                        break;
                    case '|' when Current == '|':
                        Next();
                        kind = TokenKind.PipePipe;
                        break;
                    default:
                        kind = TokenKind.Bad;
                        Diagnostics.ReportUnexpectedCharacter(TextSpan.FromBounds(start, Position), c);
                        break;
                }
            }

            return new Token(kind, TextSpan.FromBounds(start, Position), Text.ToString(start, Position - start));
        }

        private bool IsIdentifierStart(char c)
        {
            return char.IsLetter(c) || c == '_';
        }

        private bool IsIdentifierFollow(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }
    }

    public enum TokenKind
    {
        Eof,
        Bad,

        Plus,
        Star,
        Equals,

        EqualsEquals,
        BangEquals,
        Bang,
        Minus,
        Slash,

        Number,
        WhiteSpace,
        Identifier,

        AmpersandAmpersand,
        PipePipe,
        TrueKeyword,
        FalseKeyword
    }

    public class Token : SyntaxNode
    {
        public TokenKind Kind { get; }
        public TextSpan Span { get; }
        public string Text { get; }

        public Token(TokenKind kind, TextSpan span, string text)
        {
            Kind = kind;
            Span = span;
            Text = text;
        }

        public override string ToString()
        {
            return $"{Kind}: {Text}";
        }
    }

    public class TextSpan
    {
        public int Start { get; }
        public int Length { get; }

        public TextSpan(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public static TextSpan FromBounds(int start, int end)
        {
            return new TextSpan(start, end - start);
        }
    }
}
