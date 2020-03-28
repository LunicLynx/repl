using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class PrototypeSyntax : SyntaxNode
    {
        public Token IdentifierToken { get; }
        public ParameterListSyntax ParameterList { get; }

        public TypeAnnotationSyntax ReturnType { get; }

        public PrototypeSyntax(Token identifierToken, ParameterListSyntax parameterList, TypeAnnotationSyntax returnType)
        {
            IdentifierToken = identifierToken;
            ParameterList = parameterList;
            ReturnType = returnType;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return ParameterList;
            if (ReturnType != null)
            {
                yield return ReturnType;
            }
        }
    }
}