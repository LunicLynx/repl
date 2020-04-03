using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ExternDeclarationSyntax : SyntaxNode
    {
        public Token ExternKeyword { get; }
        public PrototypeSyntax Prototype { get; }

        public ExternDeclarationSyntax(SyntaxTree syntaxTree, Token externKeyword, PrototypeSyntax prototype)
            : base(syntaxTree)
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