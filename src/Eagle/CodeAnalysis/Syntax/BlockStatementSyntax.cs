﻿using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Syntax
{
    public class BlockStatementSyntax : StatementSyntax
    {
        public Token OpenBraceToken { get; }
        public ImmutableArray<StatementSyntax> Statements { get; }
        public Token CloseBraceToken { get; }

        public BlockStatementSyntax(SyntaxTree syntaxTree, Token openBraceToken, ImmutableArray<StatementSyntax> statements, Token closeBraceToken)
            : base(syntaxTree)
        {
            OpenBraceToken = openBraceToken;
            Statements = statements;
            CloseBraceToken = closeBraceToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenBraceToken;
            foreach (var statement in Statements)
            {
                yield return statement;
            }
            yield return CloseBraceToken;
        }
    }
}