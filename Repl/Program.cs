using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
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
                    var binder = new Binder(tree, variables);
                    var expression = binder.Bind();

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

                        var evaluator = new Evaluator(variables);
                        var result = evaluator.Evaluate(expression);
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
                case TokenKind.Plus: return 3;
                default: return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Plus: return 1;
                case TokenKind.Star: return 2;
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
        public Dictionary<VariableSymbol, object> Variables { get; }

        public Evaluator(Dictionary<VariableSymbol, object> variables)
        {
            Variables = variables;
        }

        public object Evaluate(BoundExpression expression)
        {
            return EvaluateExpression(expression);
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
                case BoundBinaryOperatorKind.Multiplication: return (int)left * (int)right;
            }

            return 0;
        }
    }

    public class Binder
    {
        private readonly Dictionary<VariableSymbol, object> _variables;

        public SyntaxTree Tree { get; }
        public List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();

        public Binder(SyntaxTree tree, Dictionary<VariableSymbol, object> variables)
        {
            Tree = tree;
            _variables = variables;
        }

        public BoundExpression Bind()
        {
            return BindExpression(Tree.Root);
        }

        private BoundExpression BindExpression(ExpressionSyntax expr)
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
            var token = literalExpressionSyntax.NumberToken;
            if (!int.TryParse(token.Text, out var value))
                Diagnostics.ReportInvalidNumber(token.Span, token.Text);

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
            new BoundBinaryOperator(TokenKind.Star, BoundBinaryOperatorKind.Multiplication, typeof(int)),
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
        Multiplication
    }

    public enum BoundUnaryOperatorKind
    {
        Identity
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
            new BoundUnaryOperator(TokenKind.Plus, BoundUnaryOperatorKind.Identity, typeof(int), typeof(int))
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
        public Parser(string text)
        {
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

        public ExpressionSyntax Parse()
        {
            return ParseExpression();
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
            if (Current.Kind == TokenKind.Number)
            {
                return ParseNumberLiteralExpression();
            }
            else if (Current.Kind == TokenKind.Identifier)
            {
                return ParseNameExpression();
            }

            // diagnostic ?
            Diagnostics.ReportUnexpectedToken(Current);

            // generate token
            return new LiteralExpressionSyntax(new Token(TokenKind.Number, new TextSpan(0, 0), "0"));
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

    public class SyntaxTree
    {
        public ExpressionSyntax Root { get; }
        public Diagnostic[] Diagnostics { get; }

        public SyntaxTree(ExpressionSyntax root, IEnumerable<Diagnostic> diagnostics)
        {
            Root = root;
            Diagnostics = diagnostics.ToArray();
        }

        public static IEnumerable<Token> ParseTokens(string input)
        {
            var tokens = new List<Token>();
            var lexer = new Lexer(input);
            while (true)
            {
                var token = lexer.Lex();
                tokens.Add(token);
                if (token.Kind == TokenKind.Eof)
                    break;
            }

            return tokens;
        }

        public static SyntaxTree Parse(string input)
        {
            var parser = new Parser(input);
            var expressionSyntax = parser.Parse();

            var diagnostics = parser.Diagnostics;

            return new SyntaxTree(expressionSyntax, diagnostics);
        }
    }


    public abstract class SyntaxNode
    {
        public virtual IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>();
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
        public Token NumberToken { get; }

        public LiteralExpressionSyntax(Token numberToken)
        {
            NumberToken = numberToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return NumberToken;
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

        protected readonly string Text;

        protected LexerBase(string text)
        {
            Text = text;
        }

        protected char Current()
        {
            return Position >= Text.Length
                ? '\0'
                : Text[Position];
        }

        protected void Next()
        {
            Position++;
        }

        public abstract Token Lex();
    }

    class Lexer : LexerBase
    {
        public Lexer(string text) : base(text) { }

        public override Token Lex()
        {
            var start = Position;
            var kind = TokenKind.Eof;

            var c = Current();
            Next();


            if (char.IsWhiteSpace(c))
            {
                while (char.IsWhiteSpace(Current()))
                    Next();

                kind = TokenKind.WhiteSpace;
            }
            else if (char.IsDigit(c))
            {
                while (char.IsDigit(Current()))
                    Next();

                kind = TokenKind.Number;
            }
            else if (IsIdentifierStart(c))
            {
                while (IsIdentifierFollow(Current()))
                    Next();

                kind = TokenKind.Identifier;
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
                    case '*':
                        kind = TokenKind.Star;
                        break;
                    case '=':
                        kind = TokenKind.Equals;
                        break;
                    default:
                        kind = TokenKind.Bad;
                        Diagnostics.ReportUnexpectedCharacter(TextSpan.FromBounds(start, Position), c);
                        break;
                }
            }

            return new Token(kind, TextSpan.FromBounds(start, Position), Text.Substring(start, Position - start));
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

        Number,
        WhiteSpace,
        Identifier
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
