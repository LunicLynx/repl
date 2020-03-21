﻿using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ExpressionBodySyntax : SyntaxNode
    {
        public Token EqualsGreaterToken { get; }
        public ExpressionSyntax Expression { get; }

        public ExpressionBodySyntax(Token equalsGreaterToken, ExpressionSyntax expression)
        {
            EqualsGreaterToken = equalsGreaterToken;
            Expression = expression;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return EqualsGreaterToken;
            yield return Expression;
        }
    }
}