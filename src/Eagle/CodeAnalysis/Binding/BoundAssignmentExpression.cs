﻿using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundAssignmentExpression : BoundExpression
    {
        public BoundExpression Target { get; }
        public BoundExpression Expression { get; }
        public override TypeSymbol Type => Expression.Type;

        public BoundAssignmentExpression(BoundExpression target, BoundExpression expression)
        {
            Target = target;
            Expression = expression;
        }
    }
}