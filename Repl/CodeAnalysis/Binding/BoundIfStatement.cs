namespace Repl.CodeAnalysis.Binding
{
    public class BoundIfStatement : BoundStatement
    {
        public BoundExpression Condition { get; }
        public BoundBlockStatement ThenBlock { get; }
        public BoundStatement ElseStatement { get; }

        public BoundIfStatement(BoundExpression condition, BoundBlockStatement thenBlock, BoundStatement elseStatement)
        {
            Condition = condition;
            ThenBlock = thenBlock;
            ElseStatement = elseStatement;
        }
    }
}