using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ElseClauseSyntax : SyntaxNode
    {
        public Token ElseKeyword { get; }
        public StatementSyntax ElseStatement { get; }

        public ElseClauseSyntax(SyntaxTree syntaxTree, Token elseKeyword, StatementSyntax elseStatement)
            : base(syntaxTree)
        {
            ElseKeyword = elseKeyword;
            ElseStatement = elseStatement;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ElseKeyword;
            yield return ElseStatement;
        }
    }
}