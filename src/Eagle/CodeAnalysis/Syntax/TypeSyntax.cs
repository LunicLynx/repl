using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class TypeSyntax : SyntaxNode
    {
        public Token TypeOrIdentifierToken { get; }

        public TypeSyntax(SyntaxTree syntaxTree, Token typeOrIdentifierToken)
            : base(syntaxTree)
        {
            TypeOrIdentifierToken = typeOrIdentifierToken;
        }
    }
}