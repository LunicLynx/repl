﻿using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Syntax
{
    public class CompilationUnitSyntax : SyntaxNode
    {
        public ImmutableArray<SyntaxNode> Nodes { get; }
        public Token EndOfFileToken { get; }

        public CompilationUnitSyntax(SyntaxTree syntaxTree, ImmutableArray<SyntaxNode> nodes, Token endOfFileToken)
            : base(syntaxTree)
        {
            Nodes = nodes;
            EndOfFileToken = endOfFileToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            foreach (var node in Nodes)
            {
                yield return node;
            }
            yield return EndOfFileToken;
        }
    }
}