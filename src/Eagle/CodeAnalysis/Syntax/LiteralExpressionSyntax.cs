﻿using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class LiteralExpressionSyntax : ExpressionSyntax
    {
        public Token LiteralToken { get; }

        public LiteralExpressionSyntax(SyntaxTree syntaxTree, Token literalToken)
            : base(syntaxTree)
        {
            LiteralToken = literalToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}