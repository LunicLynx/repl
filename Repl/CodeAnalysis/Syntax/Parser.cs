﻿using System.Collections.Generic;
using System.Collections.Immutable;
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

                if (token.Kind != TokenKind.WhiteSpace && token.Kind != TokenKind.SingleLineComment && token.Kind != TokenKind.Bad)
                    tokens.Add(token);

            } while (token.Kind != TokenKind.EndOfFile);

            Tokens = tokens.ToArray();
        }

        public CompilationUnitSyntax ParseCompilationUnit()
        {
            var statement = ParseStatements();
            var endOfFileToken = MatchToken(TokenKind.EndOfFile);
            return new CompilationUnitSyntax(statement, endOfFileToken);
        }

        public StatementSyntax ParseStatement()
        {
            switch (Current.Kind)
            {
                case TokenKind.OpenBrace:
                    return ParseBlockStatement();
                case TokenKind.LetKeyword:
                case TokenKind.VarKeyword:
                    return ParseVariableDeclaration();
                case TokenKind.IfKeyword:
                    return ParseIfStatement();
                case TokenKind.LoopKeyword:
                    return ParseLoopStatement();
                case TokenKind.WhileKeyword:
                    return ParseWhileStatement();
                case TokenKind.ForKeyword:
                    return ParseForStatement();
                case TokenKind.BreakKeyword:
                    return ParseBreakStatement();
                case TokenKind.ContinueKeyword:
                    return ParseContinueStatement();
                case TokenKind.ExternKeyword:
                    return ParseExternDeclaration();
                case TokenKind.Identifier when Peek(1).Kind == TokenKind.Identifier:
                    return ParseFunctionDeclaration();
                default:
                    if (IsTypeToken(Current.Kind))
                        return ParseFunctionDeclaration();
                    return ParseExpressionStatement();
            }
        }

        private StatementSyntax ParseExternDeclaration()
        {
            var externKeyword = MatchToken(TokenKind.ExternKeyword);
            var prototype = ParsePrototype();
            return new ExternDeclarationSyntax(externKeyword, prototype);
        }

        private StatementSyntax ParseFunctionDeclaration()
        {
            var prototype = ParsePrototype();
            var body = ParseBlockStatement();
            return new FunctionDeclarationSyntax(prototype, body);
        }

        private PrototypeSyntax ParsePrototype()
        {
            var returnType = ParseType();
            var identifierToken = MatchToken(TokenKind.Identifier);
            var openParenthesisToken = MatchToken(TokenKind.OpenParenthesis);

            var parameters = ImmutableArray.CreateBuilder<SyntaxNode>();

            var first = true;
            while (Current.Kind != TokenKind.CloseParenthesis &&
                   Current.Kind != TokenKind.EndOfFile)
            {
                if (!first)
                {
                    var commaToken = MatchToken(TokenKind.Comma);
                    parameters.Add(commaToken);
                }
                first = false;

                var parameter = ParseParameter();
                parameters.Add(parameter);
            }

            var closeParenthesisToken = MatchToken(TokenKind.CloseParenthesis);
            return new PrototypeSyntax(returnType, identifierToken, openParenthesisToken, parameters.ToImmutable(), closeParenthesisToken);
        }

        private TypeSyntax ParseType()
        {
            var typeOrIdentifierToken = MatchTypeOrIdentifierToken();
            return new TypeSyntax(typeOrIdentifierToken);
        }

        private ParameterSyntax ParseParameter()
        {
            var type = ParseType();
            var identifierToken = MatchToken(TokenKind.Identifier);
            return new ParameterSyntax(type, identifierToken);
        }

        private Token MatchTypeOrIdentifierToken()
        {
            if (!IsTypeOrIdentifierToken(Current.Kind))
                Diagnostics.ReportExpectedTypeOrIdentifier(Current.Span);

            return MatchToken(Current.Kind);
        }

        private bool IsTypeToken(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.VoidKeyword:
                case TokenKind.BoolKeyword:
                case TokenKind.I8Keyword:
                case TokenKind.I16Keyword:
                case TokenKind.I32Keyword:
                case TokenKind.I64Keyword:
                case TokenKind.I128Keyword:
                case TokenKind.U8Keyword:
                case TokenKind.U16Keyword:
                case TokenKind.U32Keyword:
                case TokenKind.U64Keyword:
                case TokenKind.U128Keyword:
                case TokenKind.IntKeyword:
                case TokenKind.UintKeyword:
                case TokenKind.StringKeyword:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsTypeOrIdentifierToken(TokenKind kind)
        {
            return IsTypeToken(kind) || kind == TokenKind.Identifier;
        }

        private StatementSyntax ParseContinueStatement()
        {
            var continueKeyword = MatchToken(TokenKind.ContinueKeyword);
            return new ContinueStatementSyntax(continueKeyword);
        }

        private StatementSyntax ParseBreakStatement()
        {
            var breakKeyword = MatchToken(TokenKind.BreakKeyword);
            return new BreakStatementSyntax(breakKeyword);
        }

        private StatementSyntax ParseForStatement()
        {
            var forKeyword = MatchToken(TokenKind.ForKeyword);
            var identifierToken = MatchToken(TokenKind.Identifier);
            var equalsToken = MatchToken(TokenKind.Equals);
            var lowerBound = ParseExpression();
            var toKeyword = MatchToken(TokenKind.ToKeyword);
            var upperBound = ParseExpression();
            var body = ParseBlockStatement();
            return new ForStatementSyntax(forKeyword, identifierToken, equalsToken, lowerBound, toKeyword, upperBound, body);
        }

        private StatementSyntax ParseWhileStatement()
        {
            var whileKeyword = MatchToken(TokenKind.WhileKeyword);
            var condition = ParseExpression();
            var body = ParseBlockStatement();
            return new WhileStatementSyntax(whileKeyword, condition, body);
        }

        private StatementSyntax ParseLoopStatement()
        {
            var loopKeyword = MatchToken(TokenKind.LoopKeyword);
            var body = ParseBlockStatement();
            return new LoopStatementSyntax(loopKeyword, body);
        }

        private IfStatementSyntax ParseIfStatement()
        {
            var ifKeyword = MatchToken(TokenKind.IfKeyword);
            var expression = ParseExpression();
            var thenBlock = ParseBlockStatement();

            var elseClause = ParseElseClause();

            return new IfStatementSyntax(ifKeyword, expression, thenBlock, elseClause);
        }

        private ElseClauseSyntax ParseElseClause()
        {
            if (Current.Kind != TokenKind.ElseKeyword)
                return null;

            var elseKeyword = MatchToken(TokenKind.ElseKeyword);
            var elseIfStatement = Current.Kind == TokenKind.IfKeyword
                ? (StatementSyntax)ParseIfStatement()
                : ParseBlockStatement();

            return new ElseClauseSyntax(elseKeyword, elseIfStatement);
        }

        private StatementSyntax ParseVariableDeclaration()
        {
            var expected = Current.Kind == TokenKind.LetKeyword
                ? TokenKind.LetKeyword
                : TokenKind.VarKeyword;

            var keyword = MatchToken(expected);
            var identifier = MatchToken(TokenKind.Identifier);
            var equalsToken = MatchToken(TokenKind.Equals);
            var initializer = ParseExpression();
            return new VariableDeclarationSyntax(keyword, identifier, equalsToken, initializer);
        }

        private ExpressionStatementSyntax ParseExpressionStatement()
        {
            var expression = ParseExpression();
            return new ExpressionStatementSyntax(expression);
        }

        private BlockStatementSyntax ParseBlockStatement()
        {
            var openBraceToken = MatchToken(TokenKind.OpenBrace);

            var statements = ParseStatements();

            var closeBraceToken = MatchToken(TokenKind.CloseBrace);

            return new BlockStatementSyntax(openBraceToken, statements, closeBraceToken);
        }

        private ImmutableArray<StatementSyntax> ParseStatements()
        {
            var statements = ImmutableArray.CreateBuilder<StatementSyntax>();

            var startToken = Current;
            while (Current.Kind != TokenKind.EndOfFile &&
                   Current.Kind != TokenKind.CloseBrace)
            {
                var statement = ParseStatement();
                statements.Add(statement);

                // Make sure we consume tokens
                if (Current == startToken)
                    NextToken();

                startToken = Current;
            }

            return statements.ToImmutable();
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

        //  void x() {6}
        //
        //  d = -x()
        //   1. x
        //   2. ()
        //   3. -
        // d => -6

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
            switch (Current.Kind)
            {
                case TokenKind.OpenParenthesis:
                    return ParseParenthesizedExpression();
                case TokenKind.TrueKeyword:
                case TokenKind.FalseKeyword:
                    return ParseBooleanLiteralExpression(Current.Kind);
                case TokenKind.Number:
                    return ParseNumberLiteralExpression();
                case TokenKind.String:
                    return ParseStringLiteralExpression();
                case TokenKind.Identifier when Peek(1).Kind == TokenKind.OpenParenthesis:
                    return ParseInvokeExpression();
                case TokenKind.Identifier:
                default:
                    return ParseNameExpression();
            }
        }

        private ExpressionSyntax ParseStringLiteralExpression()
        {
            var token = MatchToken(TokenKind.String);
            return new LiteralExpressionSyntax(token);
        }

        private ExpressionSyntax ParseInvokeExpression()
        {
            var target = ParseNameExpression();
            var openParenthesis = MatchToken(TokenKind.OpenParenthesis);

            var arguments = ImmutableArray.CreateBuilder<SyntaxNode>();
            var first = true;
            while (Current.Kind != TokenKind.CloseParenthesis &&
                   Current.Kind != TokenKind.EndOfFile)
            {
                if (!first)
                {
                    var commaToken = MatchToken(TokenKind.Comma);
                    arguments.Add(commaToken);
                }
                first = false;
                var argument = ParseExpression();
                arguments.Add(argument);
            }

            var closeParenthesis = MatchToken(TokenKind.CloseParenthesis);

            return new InvokeExpressionSyntax(target, openParenthesis, arguments.ToImmutable(), closeParenthesis);
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