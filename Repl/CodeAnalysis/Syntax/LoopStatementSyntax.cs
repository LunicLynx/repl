using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    internal class LoopStatementSyntax : StatementSyntax
    {
        public Token LoopKeyword { get; }
        public BlockStatementSyntax Block { get; }

        public LoopStatementSyntax(Token loopKeyword, BlockStatementSyntax block)
        {
            LoopKeyword = loopKeyword;
            Block = block;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LoopKeyword;
            yield return Block;
        }
    }
}