using System.Collections.Generic;

namespace Eagle.CodeAnalysis.Binding
{
    public class BoundGotoStatement : BoundStatement
    {
        public BoundLabel Label { get; }

        public BoundGotoStatement(BoundLabel label)
        {
            Label = label;
        }
    }
}