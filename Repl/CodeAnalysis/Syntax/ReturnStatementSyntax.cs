using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ReturnStatementSyntax : StatementSyntax
    {
        public Token ReturnKeyword { get; }
        public ExpressionSyntax Value { get; }

        public ReturnStatementSyntax(Token returnKeyword, ExpressionSyntax value)
        {
            ReturnKeyword = returnKeyword;
            Value = value;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ReturnKeyword;
            yield return Value;
        }
    }
}