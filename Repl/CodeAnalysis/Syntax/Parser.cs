using System.Collections.Generic;
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
            return new CompilationUnitSyntax(nodes, endOfFileToken);
        }

        public bool TryParseDeclaration(out SyntaxNode node)
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
                case TokenKind.FuncKeyword:
                    node = ParseFunctionDeclaration();
                    return true;
                case TokenKind.ConstKeyword:
                    node = ParseConstDeclaration();
                    return true;
                default:
                    return false;
            }
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
            return new ConstDeclarationSyntax(constKeyword, identifierToken, typeAnnotation, equalsToken, expression);
        }

        private SyntaxNode ParseAliasDeclaration()
        {
            var aliasKeyword = MatchToken(TokenKind.AliasKeyword);
            var identifierToken = MatchToken(TokenKind.Identifier);
            var equalsToken = MatchToken(TokenKind.Equals);
            var type = ParseType();

            return new AliasDeclarationSyntax(aliasKeyword, identifierToken, equalsToken, type);
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
            return new StructDeclarationSyntax(structKeyword, identifierToken, baseType, openBraceToken, builder.ToImmutable(), closeBraceToken);
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

            return new BaseTypeSyntax(colonToken, builder.ToImmutable());
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


            if (peek.Kind == TokenKind.OpenBrace ||
                peek.Kind == TokenKind.EqualsGreater)
            {
                // Property
                return ParsePropertyDeclaration();
            }

            // field
            return ParseFieldDeclaration();
        }

        private MemberDeclarationSyntax ParseConstructorDeclaration()
        {
            var identifierToken = MatchToken(TokenKind.Identifier);
            var parameterList = ParseParameterList();
            var body = ParseBlockStatement();
            return new ConstructorDeclarationSyntax(identifierToken, parameterList, body);
        }

        private MemberDeclarationSyntax ParseFieldDeclaration()
        {
            var identifierToken = MatchToken(TokenKind.Identifier);

            TypeAnnotationSyntax typeAnnotation = null;
            // For now types must be specified
            //if (Current.Kind == TokenKind.Colon)
            {
                typeAnnotation = ParseTypeAnnotation();
            }

            InitializerSyntax initializer = null;
            if (Current.Kind == TokenKind.Equals)
            {
                initializer = ParseInitializer();

            }

            return new FieldDeclarationSyntax(identifierToken, typeAnnotation, initializer);
        }

        private InitializerSyntax ParseInitializer()
        {
            var equalsToken = MatchToken(TokenKind.Equals);
            var expression = ParseExpression();
            return new InitializerSyntax(equalsToken, expression);
        }

        private MemberDeclarationSyntax ParsePropertyDeclaration()
        {
            var identifierToken = MatchToken(TokenKind.Identifier);

            TypeAnnotationSyntax typeAnnotation = null;
            if (Current.Kind == TokenKind.Colon)
                typeAnnotation = ParseTypeAnnotation();

            // get expression ?
            if (Current.Kind == TokenKind.EqualsGreater)
            {
                var expressionBody = ParseExpressionBody();
                return new PropertyDeclarationSyntax(identifierToken, typeAnnotation, expressionBody);
            }

            return new PropertyDeclarationSyntax(identifierToken, typeAnnotation, null);
        }

        private ExpressionBodySyntax ParseExpressionBody()
        {
            var equalsGreaterToken = MatchToken(TokenKind.EqualsGreater);
            var expression = ParseExpression();
            return new ExpressionBodySyntax(equalsGreaterToken, expression);
        }

        private MemberDeclarationSyntax ParseMethodDeclaration()
        {
            var prototype = ParsePrototype();
            var body = ParseBlockStatement();
            return new MethodDeclarationSyntax(prototype, body);
        }

        private SyntaxNode ParseExternDeclaration()
        {
            var externKeyword = MatchToken(TokenKind.ExternKeyword);
            var prototype = ParsePrototype();
            return new ExternDeclarationSyntax(externKeyword, prototype);
        }

        private SyntaxNode ParseFunctionDeclaration()
        {
            var funcKeyword = MatchToken(TokenKind.FuncKeyword);
            var prototype = ParsePrototype();
            var body = ParseBlockStatement();
            return new FunctionDeclarationSyntax(funcKeyword, prototype, body);
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

            return new PrototypeSyntax(identifierToken, parameterList, returnTypeAnnotation);
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
            return new ParameterListSyntax(openParenthesisToken, parameters.ToImmutable(), closeParenthesisToken);
        }

        private TypeAnnotationSyntax ParseTypeAnnotation()
        {
            var colonToken = MatchToken(TokenKind.Colon);
            var type = ParseType();
            return new TypeAnnotationSyntax(colonToken, type);
        }

        private TypeSyntax ParseType()
        {
            var typeOrIdentifierToken = MatchTypeOrIdentifierToken();
            return new TypeSyntax(typeOrIdentifierToken);
        }

        private ParameterSyntax ParseParameter()
        {
            var identifierToken = MatchToken(TokenKind.Identifier);
            var type = ParseTypeAnnotation();
            return new ParameterSyntax(identifierToken, type);
        }

        private Token MatchTypeOrIdentifierToken()
        {
            // TODO Parse qualified types
            if (!IsTypeOrIdentifierToken(Current.Kind))
                Diagnostics.ReportExpectedTypeOrIdentifier(Current.Span);

            return MatchToken(Current.Kind);
        }

        private bool IsTypeOrIdentifierToken(TokenKind kind)
        {
            return SyntaxFacts.IsTypeKeyword(kind) || kind == TokenKind.Identifier;
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

            return new BlockStatementSyntax(openBraceToken, statements.ToImmutable(), closeBraceToken);
        }

        private ImmutableArray<SyntaxNode> ParseStatementsOrDeclarations()
        {
            var builder = ImmutableArray.CreateBuilder<SyntaxNode>();

            var startToken = Current;
            while (Current.Kind != TokenKind.EndOfFile)
            {
                if (!TryParseDeclaration(out var node))
                    node = ParseStatement();
                builder.Add(node);

                // Make sure we consume tokens
                if (Current == startToken)
                    NextToken();

                startToken = Current;
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
                return new AssignmentExpressionSyntax(syntax, equalsToken, right);
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
            return new MemberAccessExpressionSyntax(target, dotToken, identifierToken);
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
            return new ThisExpressionSyntax(thisKeyword);
        }

        private ExpressionSyntax ParseNewExpression()
        {
            var newKeyword = MatchToken(TokenKind.NewKeyword);
            var nameExpression = (NameExpressionSyntax)ParseNameExpression();
            return new NewExpressionSyntax(newKeyword, nameExpression);
        }

        private ExpressionSyntax ParseStringLiteralExpression()
        {
            var token = MatchToken(TokenKind.StringLiteral);
            return new LiteralExpressionSyntax(token);
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

            return new InvokeExpressionSyntax(target, openParenthesis, arguments.ToImmutable(), closeParenthesis);
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
            return new ParenthesizedExpressionSyntax(openParenthesisToken, expression, closeParenthesisToken);
        }

        private bool TryParseCast(out CastExpressionSyntax castExpression)
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

            var closeParenthesisToken = MatchToken(TokenKind.CloseParenthesis);

            if (result == TypeFacts.TypeOrExpression && !CanFollowCast(Current.Kind))
            {
                ResetPosition(resetPosition);
                return false;
            }

            var expression = ParseExpression();
            castExpression = new CastExpressionSyntax(openParenthesisToken, type, closeParenthesisToken, expression);
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

        private TypeFacts TryParseType(out TypeSyntax type)
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
            return new LiteralExpressionSyntax(token);
        }

        private ExpressionSyntax ParseNameExpression()
        {
            var token = MatchToken(TokenKind.Identifier);
            return new NameExpressionSyntax(token);
        }

        public LiteralExpressionSyntax ParseNumberLiteralExpression()
        {
            var token = MatchToken(TokenKind.NumberLiteral);
            return new LiteralExpressionSyntax(token);
        }
    }
}