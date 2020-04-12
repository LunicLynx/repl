namespace Eagle.CodeAnalysis.Syntax
{
    public class ConstDeclarationSyntax : MemberSyntax
    {
        public Token ConstKeyword { get; }
        public Token IdentifierToken { get; }
        public TypeAnnotationSyntax TypeAnnotation { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax Initializer { get; }

        public ConstDeclarationSyntax(SyntaxTree syntaxTree, Token constKeyword, Token identifierToken,
            TypeAnnotationSyntax typeAnnotation, Token equalsToken, ExpressionSyntax initializer)
            : base(syntaxTree)
        {
            ConstKeyword = constKeyword;
            IdentifierToken = identifierToken;
            TypeAnnotation = typeAnnotation;
            EqualsToken = equalsToken;
            Initializer = initializer;
        }
    }
}