﻿using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundParameterExpression : BoundExpression
    {
        public ParameterSymbol Parameter { get; }
        public override TypeSymbol Type => Parameter.Type;

        public BoundParameterExpression(ParameterSymbol parameter)
        {
            Parameter = parameter;
        }

        public override IEnumerable<BoundNode> GetChildren()
        {
            yield break;
        }
    }
}