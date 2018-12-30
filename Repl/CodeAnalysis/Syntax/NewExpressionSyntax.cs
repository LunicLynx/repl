using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class NewExpressionSyntax : ExpressionSyntax
    {
        public Token NewKeyword { get; }
        public NameExpressionSyntax TypeName { get; }

        public NewExpressionSyntax(Token newKeyword, NameExpressionSyntax typeName)
        {
            NewKeyword = newKeyword;
            TypeName = typeName;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return NewKeyword;
            yield return TypeName;
        }
    }
}