using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class ContinueStatementSyntax : StatementSyntax
    {
        public Token ContinueKeyword { get; }

        public ContinueStatementSyntax(SyntaxTree syntaxTree, Token continueKeyword)
            : base(syntaxTree)
        {
            ContinueKeyword = continueKeyword;
        }
    }
}