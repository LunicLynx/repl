using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class FunctionDeclarationSyntax : StatementSyntax
    {
        public PrototypeSyntax Prototype { get; }
        public BlockStatementSyntax Body { get; }

        public FunctionDeclarationSyntax(PrototypeSyntax prototype, BlockStatementSyntax body)
        {
            Prototype = prototype;
            Body = body;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Prototype;
            yield return Body;
        }
    }
}