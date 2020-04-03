using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ReturnStatementSyntax : StatementSyntax
    {
        public Token ReturnKeyword { get; }
        public ExpressionSyntax Value { get; }

        public ReturnStatementSyntax(SyntaxTree syntaxTree, Token returnKeyword, ExpressionSyntax value) 
            : base(syntaxTree)
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