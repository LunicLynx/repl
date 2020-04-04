﻿using System.Collections.Generic;

namespace Repl.CodeAnalysis.Syntax
{
    public class IfStatementSyntax : StatementSyntax
    {
        public Token IfKeyword { get; }
        public ExpressionSyntax Condition { get; }
        public StatementSyntax ThenStatement { get; }
        public ElseClauseSyntax ElseClause { get; }

        public IfStatementSyntax(SyntaxTree syntaxTree, Token ifKeyword, ExpressionSyntax condition, StatementSyntax thenStatement, ElseClauseSyntax elseClause)
            : base(syntaxTree)
        {
            IfKeyword = ifKeyword;
            Condition = condition;
            ThenStatement = thenStatement;
            ElseClause = elseClause;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IfKeyword;
            yield return Condition;
            yield return ThenStatement;

            if (ElseClause != null)
            {
                yield return ElseClause;
            }
        }
    }
}