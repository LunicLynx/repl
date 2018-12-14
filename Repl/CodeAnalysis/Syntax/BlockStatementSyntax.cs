using System.Collections.Generic;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Syntax
{
    public class BlockStatementSyntax : StatementSyntax
    {
        public Token OpenBraceToken { get; }
        public ImmutableArray<StatementSyntax> Statements { get; }
        public Token CloseBraceToken { get; }

        public BlockStatementSyntax(Token openBraceToken, ImmutableArray<StatementSyntax> statements, Token closeBraceToken)
        {
            OpenBraceToken = openBraceToken;
            Statements = statements;
            CloseBraceToken = closeBraceToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenBraceToken;
            foreach (var statement in Statements)
            {
                yield return statement;
            }
            yield return CloseBraceToken;
        }
    }
}