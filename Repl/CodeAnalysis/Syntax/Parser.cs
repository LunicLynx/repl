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

                if (token.Kind != TokenKind.WhiteSpace && token.Kind != TokenKind.Bad)
                    tokens.Add(token);

            } while (token.Kind != TokenKind.EndOfFile);

            Tokens = tokens.ToArray();
        }

        public CompilationUnitSyntax ParseCompilationUnit()
        {
            var statement = ParseStatement();
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
                default:
                    return ParseExpressionStatement();
            }
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
            throw new System.NotImplementedException();
        }

        private StatementSyntax ParseWhileStatement()
        {
            var whileKeyword = MatchToken(TokenKind.WhileKeyword);
            var condition = ParseExpression();
            var block = ParseBlockStatement();
            return new WhileStatementSyntax(whileKeyword, condition, block);
        }

        private StatementSyntax ParseLoopStatement()
        {
            var loopKeyword = MatchToken(TokenKind.LoopKeyword);
            var loopBlock = ParseBlockStatement();
            return new LoopStatementSyntax(loopKeyword, loopBlock);
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
            var statements = ImmutableArray.CreateBuilder<StatementSyntax>();

            var openBraceToken = MatchToken(TokenKind.OpenBrace);

            while (Current.Kind != TokenKind.EndOfFile &&
                   Current.Kind != TokenKind.CloseBrace)
            {
                var statement = ParseStatement();
                statements.Add(statement);
            }

            var closeBraceToken = MatchToken(TokenKind.CloseBrace);

            return new BlockStatementSyntax(openBraceToken, statements.ToImmutable(), closeBraceToken);
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
            switch (Current.Kind)
            {
                case TokenKind.OpenParenthesis:
                    return ParseParenthesizedExpression();
                case TokenKind.TrueKeyword:
                case TokenKind.FalseKeyword:
                    return ParseBooleanLiteralExpression(Current.Kind);
                case TokenKind.Number:
                    return ParseNumberLiteralExpression();

                case TokenKind.Identifier:
                default:
                    return ParseNameExpression();
            }
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