using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    internal class ConstructorDeclarationSyntax : MemberDeclarationSyntax
    {
        public ParameterListSyntax ParameterList { get; }
        public BlockStatementSyntax Body { get; }

        public ConstructorDeclarationSyntax(Token identifierToken, ParameterListSyntax parameterList, BlockStatementSyntax body)
            : base(identifierToken, null)
        {
            ParameterList = parameterList;
            Body = body;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return ParameterList;
        }
    }
}