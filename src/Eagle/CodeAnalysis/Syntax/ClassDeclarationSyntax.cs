using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Syntax
{
    public class ObjectDeclarationSyntax : MemberSyntax
    {
        public Token ClassKeyword { get; }
        public Token IdentifierToken { get; }
        public BaseTypeSyntax BaseType { get; }
        public Token OpenBraceToken { get; }
        public ImmutableArray<MemberDeclarationSyntax> Members { get; }
        public Token CloseBraceToken { get; }

        public ObjectDeclarationSyntax(SyntaxTree syntaxTree, Token classKeyword, Token identifierToken,
            BaseTypeSyntax baseType, Token openBraceToken, ImmutableArray<MemberDeclarationSyntax> members,
            Token closeBraceToken)
            : base(syntaxTree)
        {
            ClassKeyword = classKeyword;
            IdentifierToken = identifierToken;
            BaseType = baseType;
            OpenBraceToken = openBraceToken;
            Members = members;
            CloseBraceToken = closeBraceToken;
        }
    }
}