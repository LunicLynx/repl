using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ExternDeclarationSyntax : StatementSyntax
    {
        public Token ExternKeyword { get; }
        public PrototypeSyntax Prototype { get; }

        public ExternDeclarationSyntax(Token externKeyword, PrototypeSyntax prototype)
        {
            ExternKeyword = externKeyword;
            Prototype = prototype;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ExternKeyword;
            yield return Prototype;
        }
    }
}