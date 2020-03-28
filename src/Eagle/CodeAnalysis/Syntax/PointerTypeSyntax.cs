using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class PointerTypeSyntax : SyntaxNode
    {
        public SyntaxNode Type { get; }
        public Token AsteriskToken { get; }

        public PointerTypeSyntax(SyntaxNode type, Token asteriskToken)
        {
            Type = type;
            AsteriskToken = asteriskToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Type;
            yield return AsteriskToken;
        }
    }
}