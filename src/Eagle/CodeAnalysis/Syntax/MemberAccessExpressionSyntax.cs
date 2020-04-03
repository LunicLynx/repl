using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class MemberAccessExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Target { get; }
        public Token DotToken { get; }
        public Token IdentifierToken { get; }

        public MemberAccessExpressionSyntax(SyntaxTree syntaxTree, ExpressionSyntax target, Token dotToken, Token identifierToken)
            : base(syntaxTree)
        {
            Target = target;
            DotToken = dotToken;
            IdentifierToken = identifierToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Target;
            yield return DotToken;
            yield return IdentifierToken;
        }
    }
}