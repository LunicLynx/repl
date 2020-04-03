using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    internal class MethodDeclarationSyntax : MemberDeclarationSyntax
    {
        public PrototypeSyntax Prototype { get; }
        public BlockStatementSyntax Body { get; }

        public MethodDeclarationSyntax(SyntaxTree syntaxTree, PrototypeSyntax prototype, BlockStatementSyntax body)
        : base(syntaxTree, prototype.IdentifierToken, prototype.ReturnType)
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