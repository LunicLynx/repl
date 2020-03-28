using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    internal class LoopStatementSyntax : StatementSyntax
    {
        public Token LoopKeyword { get; }
        public BlockStatementSyntax Body { get; }

        public LoopStatementSyntax(Token loopKeyword, BlockStatementSyntax body)
        {
            LoopKeyword = loopKeyword;
            Body = body;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LoopKeyword;
            yield return Body;
        }
    }
}