﻿using System.Collections.Generic;

namespace Repl.CodeAnalysis.Binding
{
    public class BoundReturnStatement : BoundStatement
    {
        public BoundExpression? Value { get; }

        public BoundReturnStatement(BoundExpression? value)
        {
            Value = value;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield return Value;
        }
    }
}