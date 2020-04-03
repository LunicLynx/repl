using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class FunctionDeclarationSyntax : SyntaxNode
    {
        public PrototypeSyntax Prototype { get; }
        public BlockStatementSyntax Body { get; }

        public FunctionDeclarationSyntax(SyntaxTree syntaxTree, PrototypeSyntax prototype, BlockStatementSyntax body) 
            : base(syntaxTree)
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