using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Repl.CodeAnalysis.Binding;

namespace Repl.CodeAnalysis.Lowering
{
    internal class Lowerer : BoundTreeRewriter
    {
        private static int _labelId;

        private Lowerer() { }

        public static BoundUnit Lower(BoundUnit unit)
        {
            var lowerer = new Lowerer();

            var statements = unit.GetChildren().OfType<BoundStatement>().ToImmutableArray();
            var declarations = unit.GetChildren().Except(statements).Select(lowerer.RewriteNode);
            var boundBlockStatement = new BoundBlockStatement(statements);

            var x = Flatten(lowerer.RewriteStatement(boundBlockStatement));

            return new BoundScriptUnit(declarations.Concat(new[] { x }).ToImmutableArray());
        }

        private static BoundBlockStatement Flatten(BoundStatement statement)
        {
            var builder = ImmutableArray.CreateBuilder<BoundStatement>();
            var stack = new Stack<BoundStatement>();
            stack.Push(statement);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (current is BoundBlockStatement block)
                {
                    foreach (var s in block.Statements.Reverse())
                        stack.Push(s);
                }
                else
                {
                    builder.Add(current);
                }
            }

            return new BoundBlockStatement(builder.ToImmutable());
        }

        private static LabelSymbol GenerateLabel(string prefix)
        {
            return new LabelSymbol($"{prefix}-L{++_labelId}");
        }

        private LabelSymbol _continueLabel;
        private LabelSymbol _breakLabel;

        protected override BoundStatement RewriteContinueStatement(BoundContinueStatement node)
        {
            return new BoundGotoStatement(_continueLabel);
        }

        protected override BoundStatement RewriteBreakStatement(BoundBreakStatement node)
        {
            return new BoundGotoStatement(_breakLabel);
        }

        protected override BoundStatement RewriteLoopStatement(BoundLoopStatement node)
        {
            var continueLabel = GenerateLabel("continue");
            // checkLabel
            var endLabel = GenerateLabel("end");

            // gotoCheck
            var continueLabelStatement = new BoundLabelStatement(continueLabel);
            // checkLabelStatement

            var gotoStart = new BoundGotoStatement(continueLabel);
            var endLabelStatement = new BoundLabelStatement(endLabel);

            var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(
                // gotoCheck
                continueLabelStatement,
                node.Body,
                // checkLabelStatement
                gotoStart,
                endLabelStatement
                ));

            var oldContinue = _continueLabel;
            var oldBreak = _breakLabel;
            _continueLabel = continueLabel;
            _breakLabel = endLabel;

            var r = RewriteStatement(result);

            _continueLabel = oldContinue;
            _breakLabel = oldBreak;

            return r;
        }

        protected override BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            // TODO Lower into loop statement
            // can be done by inverting the check as break eg. gotoEnd;
            var continueLabel = GenerateLabel("continue");
            var checkLabel = GenerateLabel("check");
            var endLabel = GenerateLabel("end");

            var gotoCheck = new BoundGotoStatement(checkLabel);
            var continueLabelStatement = new BoundLabelStatement(continueLabel);
            var checkLabelStatement = new BoundLabelStatement(checkLabel);

            var gotoTrue = new BoundConditionalGotoStatement(continueLabel, node.Condition);
            var endLabelStatement = new BoundLabelStatement(endLabel);

            var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(
                gotoCheck,
                continueLabelStatement,
                node.Body,
                checkLabelStatement,
                gotoTrue,
                endLabelStatement));

            var oldContinue = _continueLabel;
            var oldBreak = _breakLabel;
            _continueLabel = continueLabel;
            _breakLabel = endLabel;

            var r = RewriteStatement(result);

            _continueLabel = oldContinue;
            _breakLabel = oldBreak;

            return r;
        }

        protected override BoundStatement RewriteIfStatement(BoundIfStatement node)
        {
            if (node.ElseStatement == null)
            {
                var endLabel = GenerateLabel("end");

                var gotoFalse = new BoundConditionalGotoStatement(endLabel, node.Condition, false);
                var endLabelStatement = new BoundLabelStatement(endLabel);

                var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(
                    gotoFalse,
                    node.ThenBlock,
                    endLabelStatement));

                return RewriteStatement(result);
            }
            else
            {
                var elseLabel = GenerateLabel("else");
                var endLabel = GenerateLabel("end");

                var gotoFalse = new BoundConditionalGotoStatement(elseLabel, node.Condition, false);
                var gotoEndStatement = new BoundGotoStatement(endLabel);
                var elseLabelStatement = new BoundLabelStatement(elseLabel);
                var endLabelStatement = new BoundLabelStatement(endLabel);

                var result = new BoundBlockStatement(ImmutableArray.Create(
                    gotoFalse,
                    node.ThenBlock,
                    gotoEndStatement,
                    elseLabelStatement,
                    node.ElseStatement,
                    endLabelStatement));

                return RewriteStatement(result);
            }
        }

        protected override BoundStatement RewriteForStatement(BoundForStatement node)
        {
            var variableDeclaration = new BoundVariableDeclaration(node.Variable, node.LowerBound);
            var variableExpression = new BoundVariableExpression(node.Variable);

            var upperBoundVariable = new VariableSymbol("upperBound", true, TypeSymbol.I32);
            var upperBoundDeclaration = new BoundVariableDeclaration(upperBoundVariable, node.UpperBound);
            var condition = new BoundBinaryExpression(
                variableExpression,
                BoundBinaryOperator.Bind(Syntax.TokenKind.LessEquals, TypeSymbol.I32, TypeSymbol.I32),
                new BoundVariableExpression(upperBoundVariable)
            );

            var increment = new BoundExpressionStatement(
                new BoundAssignmentExpression(
                    node.Variable,
                    new BoundBinaryExpression(
                        variableExpression,
                        BoundBinaryOperator.Bind(Syntax.TokenKind.Plus, TypeSymbol.I32, TypeSymbol.I32),
                        new BoundLiteralExpression(TypeSymbol.I32, 1))));

            var whileBody = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(node.Body, increment));
            var whileStatement = new BoundWhileStatement(condition, whileBody);

            var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(
                variableDeclaration,
                upperBoundDeclaration,
                whileStatement));

            return RewriteBlockStatement(result);
        }
    }
}
