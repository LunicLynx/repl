namespace Eagle.CodeAnalysis.Binding
{
    public abstract class BoundLoopStatementBase : BoundStatement
    {
        protected BoundLoopStatementBase(BoundLabel breakLabel, BoundLabel continueLabel)
        {
            BreakLabel = breakLabel;
            ContinueLabel = continueLabel;
        }

        public BoundLabel BreakLabel { get; }
        public BoundLabel ContinueLabel { get; }
    }
}