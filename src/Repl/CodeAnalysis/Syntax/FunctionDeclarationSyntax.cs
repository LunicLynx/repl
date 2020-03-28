using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class FunctionDeclarationSyntax : SyntaxNode
    {
        public Token FuncKeyword { get; }
        public PrototypeSyntax Prototype { get; }
        public BlockStatementSyntax Body { get; }

        public FunctionDeclarationSyntax(Token funcKeyword, PrototypeSyntax prototype, BlockStatementSyntax body)
        {
            FuncKeyword = funcKeyword;
            Prototype = prototype;
            Body = body;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return FuncKeyword;
            yield return Prototype;
            yield return Body;
        }
    }
}