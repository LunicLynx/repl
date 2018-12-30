﻿using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Syntax
{
    public class StructDeclarationSyntax : SyntaxNode
    {
        public Token StructKeyword { get; }
        public Token IdentifierToken { get; }
        public Token OpenBraceToken { get; }
        public ImmutableArray<MemberDeclarationSyntax> Members { get; }
        public Token CloseBraceToken { get; }

        public StructDeclarationSyntax(Token structKeyword, Token identifierToken, Token openBraceToken, ImmutableArray<MemberDeclarationSyntax> members, Token closeBraceToken)
        {
            StructKeyword = structKeyword;
            IdentifierToken = identifierToken;
            OpenBraceToken = openBraceToken;
            Members = members;
            CloseBraceToken = closeBraceToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return StructKeyword;
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