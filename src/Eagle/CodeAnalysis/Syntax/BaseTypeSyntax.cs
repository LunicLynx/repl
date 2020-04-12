using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Syntax
{
    public class BaseTypeSyntax : SyntaxNode
    {
        public Token ColonToken { get; }
        public ImmutableArray<SyntaxNode> Types { get; }

        public BaseTypeSyntax(SyntaxTree syntaxTree, Token colonToken, ImmutableArray<SyntaxNode> types)
            : base(syntaxTree)
        {
            ColonToken = colonToken;
            Types = types;
        }
    }
}