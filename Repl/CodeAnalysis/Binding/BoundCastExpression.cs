﻿using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundCastExpression : BoundExpression
    {
        public override TypeSymbol Type { get; }
        public BoundExpression Expression { get; }

        public BoundCastExpression(TypeSymbol type, BoundExpression expression)
        {
            Type = type;
            Expression = expression;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Expression;
        }
    }
}