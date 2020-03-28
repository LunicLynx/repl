﻿using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class TypeAnnotationSyntax : SyntaxNode
    {
        public Token ColonToken { get; }
        public SyntaxNode Type { get; }

        public TypeAnnotationSyntax(Token colonToken, SyntaxNode type)
        {
            ColonToken = colonToken;
            Type = type;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ColonToken;
            yield return Type;
        }
    }
}