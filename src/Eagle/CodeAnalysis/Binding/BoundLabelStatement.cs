using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundLabelStatement : BoundStatement
    {
        public BoundLabel Label { get; }

        public BoundLabelStatement(BoundLabel label)
        {
            Label = label;
        }
    }
}