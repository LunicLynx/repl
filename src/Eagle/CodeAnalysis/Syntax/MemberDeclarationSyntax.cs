namespace Eagle.CodeAnalysis.Syntax
{
    public abstract class MemberDeclarationSyntax : SyntaxNode
    {
        public TypeAnnotationSyntax? TypeAnnotation { get; }

        public Token IdentifierToken { get; }

        protected MemberDeclarationSyntax(SyntaxTree syntaxTree, Token identifierToken,
            TypeAnnotationSyntax? typeAnnotation)
            : base(syntaxTree)
        {
            IdentifierToken = identifierToken;
            TypeAnnotation = typeAnnotation;
        }
    }
}