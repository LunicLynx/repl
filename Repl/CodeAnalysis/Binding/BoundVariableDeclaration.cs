namespace Repl.CodeAnalysis.Binding
{
    public class BoundVariableDeclaration : BoundStatement
    {
        public VariableSymbol Variable { get; }
        public BoundExpression Initializer { get; }

        public BoundVariableDeclaration(VariableSymbol variable, BoundExpression initializer)
        {
            Variable = variable;
            Initializer = initializer;
        }
    }
}