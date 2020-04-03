﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis.Syntax
{
    public class Parser : ParserBase
    {
        public Parser(SyntaxTree syntaxTree)
            : base(syntaxTree)
        {
            var tokens = new List<Token>();
            var lexer = new Lexer(syntaxTree);

            Token token;
            do
            {
                token = lexer.Lex();

                if (token.Kind != TokenKind.WhiteSpace &&
                    token.Kind != TokenKind.SingleLineComment &&
                    token.Kind != TokenKind.MultiLineComment &&
                    token.Kind != TokenKind.Bad)
                    tokens.Add(token);

            } while (token.Kind != TokenKind.EndOfFile);

            Tokens = tokens.ToArray();
        }

        public CompilationUnitSyntax ParseCompilationUnit()
        {
            var nodes = ParseStatementsOrDeclarations();
            var endOfFileToken = MatchToken(TokenKind.EndOfFile);
            return new CompilationUnitSyntax(SyntaxTree, nodes, endOfFileToken);
        }

        public bool TryParseDeclaration([NotNullWhen(true)]out SyntaxNode? node)
        {
            node = null;
            switch (Current.Kind)
            {
                case TokenKind.AliasKeyword:
                    node = ParseAliasDeclaration();
                    return true;
                case TokenKind.ExternKeyword:
                    node = ParseExternDeclaration();
                    return true;
                case TokenKind.StructKeyword:
                    node = ParseStructDeclaration();
                    return true;
                case TokenKind.ClassKeyword:
                    node = ParseClassKeyword();
                    return true;
                case TokenKind.ConstKeyword:
                    node = ParseConstDeclaration();
                    return true;

                /*
                 *This i quiet complex
                 * we only know this is a declaration when we hit a type clause
                 * eg:
                 *     someFunc1() : int { ... }
                 *     someFunc2(i: int) { ... }
                 *
                 * worse case is this:
                 *     someFunc() {}
                 *   whitout semicolons this is not indistinguishable from a call followed by a block
                 *
                 * So it is defenatly a call this way
                 *    someFunc();
                 */
                case TokenKind.Identifier when Peek(1).Kind == TokenKind.OpenParenthesis:
                    var result = TryParseFunctionDeclaration(out var functionDeclarationSyntax);
                    node = functionDeclarationSyntax;
                    return result;
                default:
                    return false;
            }
        }

        private SyntaxNode ParseClassKeyword()
        {
            var structKeyword = MatchToken(TokenKind.ClassKeyword);
            var identifierToken = MatchToken(TokenKind.Identifier);

            _structIdentifier = identifierToken;

            BaseTypeSyntax baseType = null;
            if (Current.Kind == TokenKind.Colon)
            {
                baseType = ParseBaseType();
            }

            var openBraceToken = MatchToken(TokenKind.OpenBrace);

            var builder = ImmutableArray.CreateBuilder<MemberDeclarationSyntax>();
            Token startToken = null;
            while (Current.Kind != TokenKind.CloseBrace &&
                   Current.Kind != TokenKind.EndOfFile)
            {
                var member = ParseMemberDeclaration();
                builder.Add(member);

                // Make sure we consume tokens
                if (Current == startToken)
                    NextToken();

                startToken = Current;
            }
            var closeBraceToken = MatchToken(TokenKind.CloseBrace);
            return new ClassDeclarationSyntax(SyntaxTree, structKeyword, identifierToken, baseType, openBraceToken, builder.ToImmutable(), closeBraceToken);
        }

        private SyntaxNode ParseConstDeclaration()
        {
            var constKeyword = MatchToken(TokenKind.ConstKeyword);
            var identifierToken = MatchToken(TokenKind.Identifier);
            TypeAnnotationSyntax typeAnnotation = null;
            if (Current.Kind == TokenKind.Colon)
            {
                typeAnnotation = ParseTypeAnnotation();
            }
            var equalsToken = MatchToken(TokenKind.Equals);
            var expression = ParseExpression();
            return new ConstDeclarationSyntax(SyntaxTree, constKeyword, identifierToken, typeAnnotation, equalsToken, expression);
        }

        private SyntaxNode ParseAliasDeclaration()
        {
            var aliasKeyword = MatchToken(TokenKind.AliasKeyword);
            var identifierToken = MatchToken(TokenKind.Identifier);
            var equalsToken = MatchToken(TokenKind.Equals);
            var type = ParseType();

            return new AliasDeclarationSyntax(SyntaxTree, aliasKeyword, identifierToken, equalsToken, type);
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
                case TokenKind.ReturnKeyword:
                    return ParseReturnStatement();
                default:
                    return ParseExpressionStatement();
            }
        }

        private StatementSyntax ParseReturnStatement()
        {
            var returnKeyword = MatchToken(TokenKind.ReturnKeyword);

            var value = ParseExpression();

            return new ReturnStatementSyntax(SyntaxTree, returnKeyword, value);
        }

        private Token _structIdentifier;
        private SyntaxNode ParseStructDeclaration()
        {
            var structKeyword = MatchToken(TokenKind.StructKeyword);
            var identifierToken = MatchToken(TokenKind.Identifier);

            _structIdentifier = identifierToken;

            BaseTypeSyntax baseType = null;
            if (Current.Kind == TokenKind.Colon)
            {
                baseType = ParseBaseType();
            }

            var openBraceToken = MatchToken(TokenKind.OpenBrace);

            var builder = ImmutableArray.CreateBuilder<MemberDeclarationSyntax>();
            Token startToken = null;
            while (Current.Kind != TokenKind.CloseBrace &&
                   Current.Kind != TokenKind.EndOfFile)
            {
                var member = ParseMemberDeclaration();
                builder.Add(member);

                // Make sure we consume tokens
                if (Current == startToken)
                    NextToken();

                startToken = Current;
            }
            var closeBraceToken = MatchToken(TokenKind.CloseBrace);
            return new StructDeclarationSyntax(SyntaxTree, structKeyword, identifierToken, baseType, openBraceToken, builder.ToImmutable(), closeBraceToken);
        }

        private BaseTypeSyntax ParseBaseType()
        {
            var builder = ImmutableArray.CreateBuilder<SyntaxNode>();
            var colonToken = MatchToken(TokenKind.Colon);
            var first = true;
            do
            {
                if (!first)
                {
                    var commaToken = MatchToken(TokenKind.Comma);
                    builder.Add(commaToken);
                }
                first = false;

                var type = ParseType();
                builder.Add(type);
            } while (
                Current.Kind == TokenKind.Comma &&
                Current.Kind != TokenKind.EndOfFile);

            return new BaseTypeSyntax(SyntaxTree, colonToken, builder.ToImmutable());
        }

        private MemberDeclarationSyntax ParseMemberDeclaration()
        {
            var peek = Peek(1);
            if (Current.Kind == TokenKind.Identifier && Current.Text == _structIdentifier.Text &&
                peek.Kind == TokenKind.OpenParenthesis)
            {
                // ctor
                return ParseConstructorDeclaration();
            }

            if (peek.Kind == TokenKind.OpenParenthesis)
            {
                // Method
                return ParseMethodDeclaration();
            }

            var identifierToken = MatchToken(TokenKind.Identifier);
            TypeAnnotationSyntax typeAnnotation = null;
            if (Current.Kind == TokenKind.Colon)
            {
                typeAnnotation = ParseTypeAnnotation();
            }

            if (Current.Kind == TokenKind.OpenBrace ||
                Current.Kind == TokenKind.EqualsGreater)
            {
                // Property
                return ParsePropertyDeclaration(identifierToken, typeAnnotation);
            }

            // field
            return ParseFieldDeclaration(identifierToken, typeAnnotation);
        }

        private MemberDeclarationSyntax ParseConstructorDeclaration()
        {
            var identifierToken = MatchToken(TokenKind.Identifier);
            var parameterList = ParseParameterList();
            var body = ParseBlockStatement();
            return new ConstructorDeclarationSyntax(SyntaxTree, identifierToken, parameterList, body);
        }

        private MemberDeclarationSyntax ParseFieldDeclaration(Token identifierToken, TypeAnnotationSyntax typeAnnotation)
        {
            InitializerSyntax initializer = null;
            if (Current.Kind == TokenKind.Equals)
            {
                initializer = ParseInitializer();

            }

            return new FieldDeclarationSyntax(SyntaxTree, identifierToken, typeAnnotation, initializer);
        }

        private InitializerSyntax ParseInitializer()
        {
            var equalsToken = MatchToken(TokenKind.Equals);
            var expression = ParseExpression();
            return new InitializerSyntax(SyntaxTree, equalsToken, expression);
        }

        private MemberDeclarationSyntax ParsePropertyDeclaration(Token identifierToken, TypeAnnotationSyntax typeAnnotation)
        {
            // get expression ?
            if (Current.Kind == TokenKind.EqualsGreater)
            {
                var expressionBody = ParseExpressionBody();
                return new PropertyDeclarationSyntax(SyntaxTree, identifierToken, typeAnnotation, expressionBody);
            }

            return new PropertyDeclarationSyntax(SyntaxTree, identifierToken, typeAnnotation, null);
        }

        private ExpressionBodySyntax ParseExpressionBody()
        {
            var equalsGreaterToken = MatchToken(TokenKind.EqualsGreater);
            var expression = ParseExpression();
            return new ExpressionBodySyntax(SyntaxTree, equalsGreaterToken, expression);
        }

        private MemberDeclarationSyntax ParseMethodDeclaration()
        {
            var prototype = ParsePrototype();
            var body = ParseBlockStatement();
            return new MethodDeclarationSyntax(SyntaxTree, prototype, body);
        }

        private SyntaxNode ParseExternDeclaration()
        {
            var externKeyword = MatchToken(TokenKind.ExternKeyword);
            var prototype = ParsePrototype();
            return new ExternDeclarationSyntax(SyntaxTree, externKeyword, prototype);
        }

        private SyntaxNode ParseFunctionDeclaration()
        {
            var prototype = ParsePrototype();
            var body = ParseBlockStatement();
            return new FunctionDeclarationSyntax(SyntaxTree, prototype, body);
        }

        private bool TryParseFunctionDeclaration([NotNullWhen(true)] out FunctionDeclarationSyntax? syntax)
        {
            syntax = null;

            // case 1: IDENTIFIER ( ) { 
            // case 2: IDENTIFIER ( ) :
            // case 3: IDENTIFIER ( IDENTIFIER :
            var t1 = Current.Kind;
            var t2 = Peek(1).Kind;
            var t3 = Peek(2).Kind;
            var t4 = Peek(3).Kind;

            // all cases expect IDENTIFIER (
            if (t1 != TokenKind.Identifier || t2 != TokenKind.OpenParenthesis)
                return false;

            // case 1 & 2
            var case1And2 = t3 == TokenKind.CloseParenthesis && (t4 == TokenKind.OpenBrace || t4 == TokenKind.Colon);
            var case3 = t3 == TokenKind.Identifier && t4 == TokenKind.Colon;

            if (!case1And2 && !case3)
                return false;

            syntax = (FunctionDeclarationSyntax)ParseFunctionDeclaration();
            return true;
        }

        private PrototypeSyntax ParsePrototype()
        {
            var identifierToken = MatchToken(TokenKind.Identifier);
            var parameterList = ParseParameterList();

            TypeAnnotationSyntax returnTypeAnnotation = null;
            if (Current.Kind == TokenKind.Colon)
            {
                returnTypeAnnotation = ParseTypeAnnotation();
            }

            return new PrototypeSyntax(SyntaxTree, identifierToken, parameterList, returnTypeAnnotation);
        }

        private ParameterListSyntax ParseParameterList()
        {
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
            return new ParameterListSyntax(SyntaxTree, openParenthesisToken, parameters.ToImmutable(), closeParenthesisToken);
        }

        private TypeAnnotationSyntax ParseTypeAnnotation()
        {
            var colonToken = MatchToken(TokenKind.Colon);
            var type = ParseType();
            return new TypeAnnotationSyntax(SyntaxTree, colonToken, type);
        }

        private SyntaxNode ParseType()
        {
            var typeOrIdentifierToken = MatchTypeOrIdentifierToken();

            SyntaxNode typeSyntax = new TypeSyntax(SyntaxTree, typeOrIdentifierToken);
            while (Current.Kind == TokenKind.Star)
                typeSyntax = new PointerTypeSyntax(SyntaxTree, typeSyntax, MatchToken(TokenKind.Star));

            return typeSyntax;
        }

        private ParameterSyntax ParseParameter()
        {
            var identifierToken = MatchToken(TokenKind.Identifier);
            var type = ParseTypeAnnotation();
            return new ParameterSyntax(SyntaxTree, identifierToken, type);
        }

        private Token MatchTypeOrIdentifierToken()
        {
            // TODO Parse qualified types
            if (!IsTypeOrIdentifierToken(Current.Kind))
                Diagnostics.ReportExpectedTypeOrIdentifier(Current.Location);

            return MatchToken(Current.Kind);
        }

        private bool IsTypeOrIdentifierToken(TokenKind kind)
        {
            return SyntaxFacts.IsTypeKeyword(kind) || kind == TokenKind.Identifier;
        }

        private StatementSyntax ParseContinueStatement()
        {
            var continueKeyword = MatchToken(TokenKind.ContinueKeyword);
            return new ContinueStatementSyntax(SyntaxTree, continueKeyword);
        }

        private StatementSyntax ParseBreakStatement()
        {
            var breakKeyword = MatchToken(TokenKind.BreakKeyword);
            return new BreakStatementSyntax(SyntaxTree, breakKeyword);
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
            return new ForStatementSyntax(SyntaxTree, forKeyword, identifierToken, equalsToken, lowerBound, toKeyword, upperBound, body);
        }

        private StatementSyntax ParseWhileStatement()
        {
            var whileKeyword = MatchToken(TokenKind.WhileKeyword);
            var condition = ParseExpression();
            var body = ParseBlockStatement();
            return new WhileStatementSyntax(SyntaxTree, whileKeyword, condition, body);
        }

        private StatementSyntax ParseLoopStatement()
        {
            var loopKeyword = MatchToken(TokenKind.LoopKeyword);
            var body = ParseBlockStatement();
            return new LoopStatementSyntax(SyntaxTree, loopKeyword, body);
        }

        private IfStatementSyntax ParseIfStatement()
        {
            var ifKeyword = MatchToken(TokenKind.IfKeyword);
            var expression = ParseExpression();
            var thenBlock = ParseBlockStatement();

            var elseClause = ParseElseClause();

            return new IfStatementSyntax(SyntaxTree, ifKeyword, expression, thenBlock, elseClause);
        }

        private ElseClauseSyntax ParseElseClause()
        {
            if (Current.Kind != TokenKind.ElseKeyword)
                return null;

            var elseKeyword = MatchToken(TokenKind.ElseKeyword);
            var elseIfStatement = Current.Kind == TokenKind.IfKeyword
                ? (StatementSyntax)ParseIfStatement()
                : ParseBlockStatement();

            return new ElseClauseSyntax(SyntaxTree, elseKeyword, elseIfStatement);
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
            return new VariableDeclarationSyntax(SyntaxTree, keyword, identifier, equalsToken, initializer);
        }

        private ExpressionStatementSyntax ParseExpressionStatement()
        {
            var expression = ParseExpression();
            return new ExpressionStatementSyntax(SyntaxTree, expression);
        }

        private BlockStatementSyntax ParseBlockStatement()
        {
            var openBraceToken = MatchToken(TokenKind.OpenBrace);

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

            var closeBraceToken = MatchToken(TokenKind.CloseBrace);

            return new BlockStatementSyntax(SyntaxTree, openBraceToken, statements.ToImmutable(), closeBraceToken);
        }

        private ImmutableArray<SyntaxNode> ParseStatementsOrDeclarations()
        {
            var builder = ImmutableArray.CreateBuilder<SyntaxNode>();

            while (Current.Kind != TokenKind.EndOfFile)
            {
                var startToken = Current;

                if (!TryParseDeclaration(out var node))
                    node = ParseStatement();
                builder.Add(node);

                // Make sure we consume tokens
                if (Current == startToken)
                    NextToken();
            }

            return builder.ToImmutable();
        }

        public ExpressionSyntax ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        public ExpressionSyntax ParseAssignmentExpression()
        {
            //if (Peek(0).Kind == TokenKind.Identifier &&
            //    Peek(1).Kind == TokenKind.Equals)
            //{
            //    var identifierToken = MatchToken(TokenKind.Identifier);
            //    var operatorToken = MatchToken(TokenKind.Equals);
            //    var right = ParseAssignmentExpression();
            //    return new AssignmentExpressionSyntax(identifierToken, operatorToken, right);
            //}

            var syntax = ParseBinaryExpression();
            if (Current.Kind == TokenKind.Equals)
            {
                var equalsToken = MatchToken(TokenKind.Equals);
                var right = ParseAssignmentExpression();
                return new AssignmentExpressionSyntax(SyntaxTree, syntax, equalsToken, right);
            }
            return syntax;
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
                left = new UnaryExpressionSyntax(SyntaxTree, operatorToken, operand);
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
                left = new BinaryExpressionSyntax(SyntaxTree, left, operatorToken, right);
            }

            return left;
        }

        public ExpressionSyntax ParsePrimaryExpression()
        {
            var result = ParsePrimaryExpressionStart();

            var done = false;
            while (!done)
            {
                switch (Current.Kind)
                {
                    case TokenKind.OpenParenthesis:
                        result = ParseInvokeExpression(result);
                        break;
                    case TokenKind.Dot:
                        result = ParseMemberAccessExpression(result);
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            return result;
        }

        private ExpressionSyntax ParseMemberAccessExpression(ExpressionSyntax target)
        {
            var dotToken = MatchToken(TokenKind.Dot);
            var identifierToken = MatchToken(TokenKind.Identifier);
            return new MemberAccessExpressionSyntax(SyntaxTree, target, dotToken, identifierToken);
        }

        public ExpressionSyntax ParsePrimaryExpressionStart()
        {
            switch (Current.Kind)
            {
                case TokenKind.ThisKeyword:
                    return ParseThisExpression();
                case TokenKind.OpenParenthesis:
                    return ParseCastOrParenthesizedExpression();
                case TokenKind.TrueKeyword:
                case TokenKind.FalseKeyword:
                    return ParseBooleanLiteralExpression(Current.Kind);
                case TokenKind.NumberLiteral:
                    return ParseNumberLiteralExpression();
                case TokenKind.StringLiteral:
                    return ParseStringLiteralExpression();
                case TokenKind.NewKeyword:
                    return ParseNewExpression();
                case TokenKind.Identifier:
                default:
                    return ParseNameExpression();
            }
        }

        private ExpressionSyntax ParseThisExpression()
        {
            var thisKeyword = MatchToken(TokenKind.ThisKeyword);
            return new ThisExpressionSyntax(SyntaxTree, thisKeyword);
        }

        private ExpressionSyntax ParseNewExpression()
        {
            var newKeyword = MatchToken(TokenKind.NewKeyword);
            var nameExpression = (NameExpressionSyntax)ParseNameExpression();
            return new NewExpressionSyntax(SyntaxTree, newKeyword, nameExpression);
        }

        private ExpressionSyntax ParseStringLiteralExpression()
        {
            var token = MatchToken(TokenKind.StringLiteral);
            return new LiteralExpressionSyntax(SyntaxTree, token);
        }

        private ExpressionSyntax ParseInvokeExpression(ExpressionSyntax target)
        {
            var openParenthesis = MatchToken(TokenKind.OpenParenthesis);

            var arguments = ImmutableArray.CreateBuilder<SyntaxNode>();
            var first = true;
            Token startToken = null;
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

                // Make sure we consume tokens
                if (Current == startToken)
                    NextToken();

                startToken = Current;
            }

            var closeParenthesis = MatchToken(TokenKind.CloseParenthesis);

            return new InvokeExpressionSyntax(SyntaxTree, target, openParenthesis, arguments.ToImmutable(), closeParenthesis);
        }

        private ExpressionSyntax ParseCastOrParenthesizedExpression()
        {
            if (TryParseCast(out var castExpression))
            {
                return castExpression;
            }

            //var x = (TokenKind) - 3;

            var openParenthesisToken = MatchToken(TokenKind.OpenParenthesis);
            var expression = ParseExpression();
            var closeParenthesisToken = MatchToken(TokenKind.CloseParenthesis);
            return new ParenthesizedExpressionSyntax(SyntaxTree, openParenthesisToken, expression, closeParenthesisToken);
        }

        private bool TryParseCast([NotNullWhen(true)]out CastExpressionSyntax? castExpression)
        {
            castExpression = null;
            var resetPosition = GetPosition();
            var openParenthesisToken = MatchToken(TokenKind.OpenParenthesis);

            var result = TryParseType(out var type);
            if (result == TypeFacts.NotType)
            {
                ResetPosition(resetPosition);
                return false;
            }

            if (Current.Kind != TokenKind.CloseParenthesis)
            {
                ResetPosition(resetPosition);
                return false;
            }

            var closeParenthesisToken = MatchToken(TokenKind.CloseParenthesis);

            if (result == TypeFacts.TypeOrExpression && !CanFollowCast(Current.Kind))
            {
                ResetPosition(resetPosition);
                return false;
            }

            var expression = ParseExpression();
            castExpression = new CastExpressionSyntax(SyntaxTree, openParenthesisToken, type, closeParenthesisToken, expression);
            return true;
        }

        private bool CanFollowCast(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Ampersand:
                case TokenKind.AmpersandAmpersand:
                case TokenKind.Bang:
                case TokenKind.BangEquals:
                case TokenKind.CloseBrace:
                case TokenKind.CloseBracket:
                case TokenKind.CloseParenthesis:
                case TokenKind.Colon:
                case TokenKind.Comma:
                case TokenKind.Dot:
                case TokenKind.EndOfFile:
                case TokenKind.Equals:
                case TokenKind.EqualsEquals:
                case TokenKind.Greater:
                case TokenKind.GreaterEquals:
                case TokenKind.Hat:
                case TokenKind.Less:
                case TokenKind.LessEquals:
                case TokenKind.Minus:
                case TokenKind.OpenBrace:
                case TokenKind.OpenBracket:
                case TokenKind.Percent:
                case TokenKind.Pipe:
                case TokenKind.PipePipe:
                case TokenKind.Plus:
                case TokenKind.Slash:
                case TokenKind.Star:
                case TokenKind.Tilde:
                    return false;
                default:
                    return true;
            }
        }

        private enum TypeFacts
        {
            NotType,
            MustBeType,

            TypeOrExpression,
        }

        private TypeFacts TryParseType(out SyntaxNode type)
        {
            type = null;
            var resetPosition = GetPosition();

            if (SyntaxFacts.IsTypeKeyword(Current.Kind))
            {
                type = ParseType();
                return TypeFacts.MustBeType;
            }

            if (Current.Kind == TokenKind.Identifier)
            {
                type = ParseType();
                return TypeFacts.TypeOrExpression;
            }

            ResetPosition(resetPosition);
            return TypeFacts.NotType;
        }

        private ExpressionSyntax ParseBooleanLiteralExpression(TokenKind kind)
        {
            var token = MatchToken(kind);
            return new LiteralExpressionSyntax(SyntaxTree, token);
        }

        private ExpressionSyntax ParseNameExpression()
        {
            var token = MatchToken(TokenKind.Identifier);
            return new NameExpressionSyntax(SyntaxTree, token);
        }

        public LiteralExpressionSyntax ParseNumberLiteralExpression()
        {
            var token = MatchToken(TokenKind.NumberLiteral);
            return new LiteralExpressionSyntax(SyntaxTree, token);
        }
    }
}