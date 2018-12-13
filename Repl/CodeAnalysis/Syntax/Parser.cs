using System.Collections.Generic;
using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis.Syntax
{
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
            if (Current.Kind == TokenKind.OpenParenthesis)
            {
                return ParseParenthesizedExpression();
            }

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

        private ExpressionSyntax ParseParenthesizedExpression()
        {
            var openParenthesisToken = MatchToken(TokenKind.OpenParenthesis);
            var expression = ParseExpression();
            var closeParenthesisToken = MatchToken(TokenKind.CloseParenthesis);
            return new ParenthesizedExpressionSyntax(openParenthesisToken, expression, closeParenthesisToken);
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
}