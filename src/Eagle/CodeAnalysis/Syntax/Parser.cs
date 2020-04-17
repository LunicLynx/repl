using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Eagle.CodeAnalysis.Syntax
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

                if (token.Kind != TokenKind.Whitespace &&
                    token.Kind != TokenKind.SingleLineComment &&
                    token.Kind != TokenKind.MultiLineComment &&
                    token.Kind != TokenKind.Bad)
                    tokens.Add(token);

            } while (token.Kind != TokenKind.EndOfFile);

            Tokens = tokens.ToArray();
        }

        public CompilationUnitSyntax ParseCompilationUnit()
        {
            var members = ParseMembers();
            var endOfFileToken = MatchToken(TokenKind.EndOfFile);
            return new CompilationUnitSyntax(SyntaxTree, members, endOfFileToken);
        }

        public bool TryParseDeclaration([NotNullWhen(true)]out MemberSyntax? node)
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
                case TokenKind.ObjectKeyword:
                    node = ParseObjectDeclaration();
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

        private IndexerDeclarationSyntax ParseIndexerDeclaration()
        {
            var openBracketToken = MatchToken(TokenKind.OpenBracket);
            var parameters = ParseParameterList();
            var closeBracketToken = MatchToken(TokenKind.CloseBracket);
            var typeClause = ParseTypeAnnotation();
            var x = ParsePropertyBody();

            return new IndexerDeclarationSyntax(SyntaxTree, openBracketToken, parameters, closeBracketToken, typeClause, x);
        }

        private SyntaxNode ParsePropertyBody()
        {
            if (Current.Kind == TokenKind.EqualsGreater)
                return ParseExpressionBody();

            var openBraceToken = MatchToken(TokenKind.OpenBrace);

            GetterClauseSyntax? getterClause = null;
            SetterClauseSyntax? setterClause = null;
            if (Current.Kind == TokenKind.GetKeyword)
            {
                getterClause = ParseGetterClause();

                if (Current.Kind == TokenKind.SetKeyword)
                {
                    setterClause = ParseSetterClause();
                }
            }
            else
            {
                setterClause = ParseSetterClause();
            }

            var closeBraceToken = MatchToken(TokenKind.CloseBrace);

            return new PropertyBodySyntax(SyntaxTree, openBraceToken, getterClause, setterClause, closeBraceToken);
        }

        private SetterClauseSyntax ParseSetterClause()
        {
            var setKeyword = MatchToken(TokenKind.SetKeyword);

            var body = Current.Kind == TokenKind.EqualsGreater
                ? (SyntaxNode)ParseExpressionBody()
                    : ParseBlockStatement();

            return new SetterClauseSyntax(SyntaxTree, setKeyword, body);
        }

        private GetterClauseSyntax ParseGetterClause()
        {
            var getKeyword = MatchToken(TokenKind.GetKeyword);

            var body = Current.Kind == TokenKind.EqualsGreater
                ? (SyntaxNode)ParseExpressionBody()
                : ParseBlockStatement();

            return new GetterClauseSyntax(SyntaxTree, getKeyword, body);
        }

        private MemberSyntax ParseObjectDeclaration()
        {
            var objectKeyword = MatchToken(TokenKind.ObjectKeyword);
            var identifierToken = MatchToken(TokenKind.Identifier);

            //_structIdentifier = identifierToken;

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
            return new ObjectDeclarationSyntax(SyntaxTree, objectKeyword, identifierToken, baseType, openBraceToken, builder.ToImmutable(), closeBraceToken);
        }

        private MemberSyntax ParseConstDeclaration()
        {
            var constKeyword = MatchToken(TokenKind.ConstKeyword);
            var identifierToken = MatchToken(TokenKind.Identifier);
            TypeClauseSyntax typeClause = null;
            if (Current.Kind == TokenKind.Colon)
            {
                typeClause = ParseTypeAnnotation();
            }
            var equalsToken = MatchToken(TokenKind.Equals);
            var expression = ParseExpression();
            return new ConstDeclarationSyntax(SyntaxTree, constKeyword, identifierToken, typeClause, equalsToken, expression);
        }

        private MemberSyntax ParseAliasDeclaration()
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

            ExpressionSyntax? value = null;
            if (Current.Kind != TokenKind.CloseBrace &&
                Current.Kind != TokenKind.EndOfFile)
                value = ParseExpression();

            return new ReturnStatementSyntax(SyntaxTree, returnKeyword, value);
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

            if (peek.Kind == TokenKind.OpenParenthesis)
            {
                if (Current.Kind == TokenKind.CtorKeyword)
                    return ParseConstructorDeclaration();
                else

                    // Method
                    return ParseMethodDeclaration();
            }

            if (Current.Kind == TokenKind.OpenBracket)
            {
                return ParseIndexerDeclaration();
            }

            var identifierToken = MatchToken(TokenKind.Identifier);
            var typeClause = ParseTypeAnnotation();

            if (Current.Kind == TokenKind.OpenBrace ||
                Current.Kind == TokenKind.EqualsGreater)
            {
                // Property
                return ParsePropertyDeclaration(identifierToken, typeClause);
            }

            // field
            return ParseFieldDeclaration(identifierToken, typeClause);
        }

        private MemberDeclarationSyntax ParseConstructorDeclaration()
        {
            var ctorKeyword = MatchToken(TokenKind.CtorKeyword);
            var openParenthesisToken = MatchToken(TokenKind.OpenParenthesis);
            var parameterList = ParseParameterList();
            var closeParenthesisToken = MatchToken(TokenKind.CloseParenthesis);
            var body = ParseBlockStatement();
            return new ConstructorDeclarationSyntax(SyntaxTree, ctorKeyword, openParenthesisToken, parameterList, closeParenthesisToken, body);
        }

        private MemberDeclarationSyntax ParseFieldDeclaration(Token identifierToken, TypeClauseSyntax typeClause)
        {
            InitializerSyntax initializer = null;
            if (Current.Kind == TokenKind.Equals)
            {
                initializer = ParseInitializer();

            }

            return new FieldDeclarationSyntax(SyntaxTree, identifierToken, typeClause, initializer);
        }

        private InitializerSyntax ParseInitializer()
        {
            var equalsToken = MatchToken(TokenKind.Equals);
            var expression = ParseExpression();
            return new InitializerSyntax(SyntaxTree, equalsToken, expression);
        }

        private MemberDeclarationSyntax ParsePropertyDeclaration(Token identifierToken, TypeClauseSyntax typeClause)
        {
            // get expression ?
            if (Current.Kind == TokenKind.EqualsGreater)
            {
                var expressionBody = ParseExpressionBody();
                return new PropertyDeclarationSyntax(SyntaxTree, identifierToken, typeClause, expressionBody);
            }

            var body = ParsePropertyBody();

            return new PropertyDeclarationSyntax(SyntaxTree, identifierToken, typeClause, body);
        }

        private ExpressionBodySyntax ParseExpressionBody()
        {
            var equalsGreaterToken = MatchToken(TokenKind.EqualsGreater);
            var expression = ParseExpression();
            return new ExpressionBodySyntax(SyntaxTree, equalsGreaterToken, expression);
        }

        private MemberDeclarationSyntax ParseMethodDeclaration()
        {
            var (identifierToken, openParenthesisToken, parameterList, closeParenthesisToken, returnTypeAnnotation) = ParsePrototype();
            var body = ParseBlockStatement();
            return new MethodDeclarationSyntax(SyntaxTree, identifierToken, openParenthesisToken, parameterList, closeParenthesisToken, returnTypeAnnotation, body);
        }

        private MemberSyntax ParseExternDeclaration()
        {
            var externKeyword = MatchToken(TokenKind.ExternKeyword);
            var (identifierToken, openParenthesisToken, parameterList, closeParenthesisToken, returnTypeAnnotation) = ParsePrototype();
            return new ExternDeclarationSyntax(SyntaxTree, externKeyword, identifierToken, openParenthesisToken, parameterList, closeParenthesisToken, returnTypeAnnotation);
        }

        private SyntaxNode ParseFunctionDeclaration()
        {
            var (identifierToken, openParenthesisToken, parameters, closeParenthesisToken, type) = ParsePrototype();
            var body = ParseBlockStatement();
            return new FunctionDeclarationSyntax(SyntaxTree, identifierToken, openParenthesisToken, parameters, closeParenthesisToken, type, body);
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

        private (Token identifierToken, Token openParenthesisToken, SeparatedSyntaxList<ParameterSyntax> parameters, Token closeParenthesisToken, TypeClauseSyntax? type) ParsePrototype()
        {
            var identifierToken = MatchToken(TokenKind.Identifier);

            var openParenthesisToken = MatchToken(TokenKind.OpenParenthesis);
            var parameterList = ParseParameterList();
            var closeParenthesisToken = MatchToken(TokenKind.CloseParenthesis);

            TypeClauseSyntax? returnTypeAnnotation = null;
            if (Current.Kind == TokenKind.Colon)
            {
                returnTypeAnnotation = ParseTypeAnnotation();
            }

            return (identifierToken, openParenthesisToken, parameterList, closeParenthesisToken, returnTypeAnnotation);
        }

        private SeparatedSyntaxList<ParameterSyntax> ParseParameterList()
        {
            var nodesAndSeparators = ImmutableArray.CreateBuilder<SyntaxNode>();

            var parseNextParameter = true;
            while (parseNextParameter &&
                Current.Kind != TokenKind.CloseParenthesis &&
                   Current.Kind != TokenKind.EndOfFile)
            {
                var parameter = ParseParameter();
                nodesAndSeparators.Add(parameter);

                if (Current.Kind == TokenKind.Comma)
                {
                    var commaToken = MatchToken(TokenKind.Comma);
                    nodesAndSeparators.Add(commaToken);
                }
                else
                {
                    parseNextParameter = false;
                }
            }

            return new SeparatedSyntaxList<ParameterSyntax>(nodesAndSeparators.ToImmutable());
        }

        private TypeClauseSyntax ParseTypeAnnotation()
        {
            var colonToken = MatchToken(TokenKind.Colon);
            var type = ParseType();
            return new TypeClauseSyntax(SyntaxTree, colonToken, type);
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
            var body = ParseStatement();
            return new ForStatementSyntax(SyntaxTree, forKeyword, identifierToken, equalsToken, lowerBound, toKeyword, upperBound, body);
        }

        private StatementSyntax ParseWhileStatement()
        {
            var whileKeyword = MatchToken(TokenKind.WhileKeyword);
            var condition = ParseExpression();
            var body = ParseStatement();
            return new WhileStatementSyntax(SyntaxTree, whileKeyword, condition, body);
        }

        private StatementSyntax ParseLoopStatement()
        {
            var loopKeyword = MatchToken(TokenKind.LoopKeyword);
            var body = ParseStatement();
            return new LoopStatementSyntax(SyntaxTree, loopKeyword, body);
        }

        private IfStatementSyntax ParseIfStatement()
        {
            var ifKeyword = MatchToken(TokenKind.IfKeyword);
            var expression = ParseExpression();
            var thenBlock = ParseStatement();

            var elseClause = ParseElseClause();

            return new IfStatementSyntax(SyntaxTree, ifKeyword, expression, thenBlock, elseClause);
        }

        private ElseClauseSyntax ParseElseClause()
        {
            if (Current.Kind != TokenKind.ElseKeyword)
                return null;

            var elseKeyword = MatchToken(TokenKind.ElseKeyword);
            var elseStatement = ParseStatement();

            return new ElseClauseSyntax(SyntaxTree, elseKeyword, elseStatement);
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


            while (Current.Kind != TokenKind.EndOfFile &&
                   Current.Kind != TokenKind.CloseBrace)
            {
                var startToken = Current;

                var statement = ParseStatement();
                statements.Add(statement);

                // Make sure we consume tokens
                if (Current == startToken)
                    NextToken();
            }

            var closeBraceToken = MatchToken(TokenKind.CloseBrace);

            return new BlockStatementSyntax(SyntaxTree, openBraceToken, statements.ToImmutable(), closeBraceToken);
        }

        private ImmutableArray<MemberSyntax> ParseMembers()
        {
            var members = ImmutableArray.CreateBuilder<MemberSyntax>();

            while (Current.Kind != TokenKind.EndOfFile)
            {
                var startToken = Current;

                var member = ParseMember();
                members.Add(member);

                // Make sure we consume tokens
                if (Current == startToken)
                    NextToken();
            }

            return members.ToImmutable();
        }

        private MemberSyntax ParseMember()
        {
            if (!TryParseDeclaration(out var node))
                node = ParseGlobalStatement();

            return node;
        }

        private MemberSyntax ParseGlobalStatement()
        {
            var statement = ParseStatement();
            return new GlobalStatementSyntax(SyntaxTree, statement);
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

            var startToken = Current;

            // if this did not advance
            var syntax = ParseBinaryExpression();

            if (syntax is LiteralExpressionSyntax ||
                syntax is InvokeExpressionSyntax)
            {
                // this is not assignable
                return syntax;
            }

            // we did not parse anything
            if (Current == startToken) return syntax;

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

            if (result is LiteralExpressionSyntax)
                return result;

            var done = false;
            while (!done)
            {
                switch (Current.Kind)
                {
                    case TokenKind.OpenParenthesis:
                        result = ParseInvokeExpression(result);
                        break;
                    case TokenKind.OpenBracket:
                        result = ParseIndexExpression(result);
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
            if (Current.Kind == TokenKind.OpenParenthesis)
            {
                var openParenthesisToken = MatchToken(TokenKind.OpenParenthesis);
                var type = ParseType();
                var closeParenthesisToken = MatchToken(TokenKind.CloseParenthesis);
                return new SuffixCastExpressionSyntax(SyntaxTree, target, dotToken, openParenthesisToken, type, closeParenthesisToken);
            }
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
                case TokenKind.CharacterLiteral:
                    return ParseCharacterLiteralExpression();
                case TokenKind.NewKeyword:
                    return ParseNewExpression();
                case TokenKind.Identifier:
                default:
                    return ParseNameExpression();
            }
        }

        private ExpressionSyntax ParseCharacterLiteralExpression()
        {
            var token = MatchToken(TokenKind.CharacterLiteral);
            return new LiteralExpressionSyntax(SyntaxTree, token);
        }

        private ExpressionSyntax ParseThisExpression()
        {
            var thisKeyword = MatchToken(TokenKind.ThisKeyword);
            return new ThisExpressionSyntax(SyntaxTree, thisKeyword);
        }

        private ExpressionSyntax ParseNewExpression()
        {
            var newKeyword = MatchToken(TokenKind.NewKeyword);
            var type = ParseType();

            if (Current.Kind == TokenKind.OpenBracket)
            {
                var openBracketToken = MatchToken(TokenKind.OpenBracket);
                var arguments = ParseArguments();
                var closeBracketToken = MatchToken(TokenKind.CloseBracket);
                // new Array

                return new NewArrayExpressionSyntax(SyntaxTree, newKeyword, type, openBracketToken, arguments, closeBracketToken);
            }
            else
            {
                var openParenthesisToken = MatchToken(TokenKind.OpenParenthesis);
                var arguments = ParseArguments();
                var closeParenthesisToken = MatchToken(TokenKind.CloseParenthesis);

                return new NewInstanceExpressionSyntax(SyntaxTree, newKeyword, type, openParenthesisToken, arguments,
                    closeParenthesisToken);
            }
        }

        private ExpressionSyntax ParseStringLiteralExpression()
        {
            var token = MatchToken(TokenKind.StringLiteral);
            return new LiteralExpressionSyntax(SyntaxTree, token);
        }

        private ExpressionSyntax ParseIndexExpression(ExpressionSyntax target)
        {
            var openBracket = MatchToken(TokenKind.OpenBracket);
            var arguments = ParseArguments();
            var closeBracket = MatchToken(TokenKind.CloseBracket);
            return new IndexExpressionSyntax(SyntaxTree, target, openBracket, arguments, closeBracket);
        }

        private ExpressionSyntax ParseInvokeExpression(ExpressionSyntax target)
        {
            var openParenthesis = MatchToken(TokenKind.OpenParenthesis);

            var arguments = ParseArguments();

            var closeParenthesis = MatchToken(TokenKind.CloseParenthesis);

            return new InvokeExpressionSyntax(SyntaxTree, target, openParenthesis, arguments, closeParenthesis);
        }

        private SeparatedSyntaxList<ExpressionSyntax> ParseArguments()
        {
            var nodesAndSeparators = ImmutableArray.CreateBuilder<SyntaxNode>();

            var parseNextArgument = true;
            while (parseNextArgument &&
                   Current.Kind != TokenKind.CloseParenthesis &&
                   Current.Kind != TokenKind.EndOfFile)
            {
                var expression = ParseExpression();
                nodesAndSeparators.Add(expression);

                if (Current.Kind == TokenKind.Comma)
                {
                    var comma = MatchToken(TokenKind.Comma);
                    nodesAndSeparators.Add(comma);
                }
                else
                {
                    parseNextArgument = false;
                }
            }

            return new SeparatedSyntaxList<ExpressionSyntax>(nodesAndSeparators.ToImmutable());
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