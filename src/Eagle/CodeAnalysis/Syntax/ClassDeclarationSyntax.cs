using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Syntax
{
    public class ClassDeclarationSyntax : SyntaxNode
    {
        public Token ClassKeyword { get; }
        public Token IdentifierToken { get; }
        public BaseTypeSyntax BaseType { get; }
        public Token OpenBraceToken { get; }
        public ImmutableArray<MemberDeclarationSyntax> Members { get; }
        public Token CloseBraceToken { get; }

        public ClassDeclarationSyntax(SyntaxTree syntaxTree, Token classKeyword, Token identifierToken,
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

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ClassKeyword;
            yield return IdentifierToken;
            yield return OpenBraceToken;
            foreach (var member in Members)
            {
                yield return member;
            }
            yield return CloseBraceToken;
        }
    }
}