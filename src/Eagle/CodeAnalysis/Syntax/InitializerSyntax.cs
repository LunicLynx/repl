using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class InitializerSyntax : SyntaxNode
    {
        public Token EqualsToken { get; }
        public ExpressionSyntax Expression { get; }

        public InitializerSyntax(SyntaxTree syntaxTree, Token equalsToken, ExpressionSyntax expression)
            : base(syntaxTree)
        {
            EqualsToken = equalsToken;
            Expression = expression;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return EqualsToken;
            yield return Expression;
        }
    }
}