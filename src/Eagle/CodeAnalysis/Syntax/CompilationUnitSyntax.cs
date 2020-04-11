using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Syntax
{
    public class CompilationUnitSyntax : SyntaxNode
    {
        public ImmutableArray<MemberSyntax> Members { get; }
        public Token EndOfFileToken { get; }

        public CompilationUnitSyntax(SyntaxTree syntaxTree, ImmutableArray<MemberSyntax> members, Token endOfFileToken)
            : base(syntaxTree)
        {
            Members = members;
            EndOfFileToken = endOfFileToken;
        }
    }
}