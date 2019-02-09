using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Repl.CodeAnalysis.Binding;

namespace Repl.CodeAnalysis.Lowering
{
    internal class Lowerer : BoundTreeRewriter
    {
        private static int _labelId;

        private Lowerer(ImmutableArray<Symbol> symbols)
        {
            _intType = symbols.OfType<TypeSymbol>().FirstOrDefault(s => s.Name == "Int64");
        }

        public static BoundUnit Lower(BoundGlobalScope globalScope)
        {
            var lowerer = new Lowerer(globalScope.Symbols);

            var unit = globalScope.Unit;
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
        private TypeSymbol _intType;

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

            var upperBoundVariable = new VariableSymbol("upperBound", true, _intType);
            var upperBoundDeclaration = new BoundVariableDeclaration(upperBoundVariable, node.UpperBound);
            var condition = new BoundBinaryExpression(
                variableExpression,
                BoundBinaryOperator.Bind(Syntax.TokenKind.LessEquals, _intType, _intType),
                new BoundVariableExpression(upperBoundVariable)
            );

            var increment = new BoundExpressionStatement(
                new BoundAssignmentExpression(
                    new BoundVariableExpression(node.Variable),
                    new BoundBinaryExpression(
                        variableExpression,
                        BoundBinaryOperator.Bind(Syntax.TokenKind.Plus, _intType, _intType),
                        new BoundLiteralExpression(_intType, 1))));

            var whileBody = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(node.Body, increment));
            var whileStatement = new BoundWhileStatement(condition, whileBody);

            var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(
                variableDeclaration,
                upperBoundDeclaration,
                whileStatement));

            return RewriteBlockStatement(result);
        }

        protected override BoundStructDeclaration RewriteStructDeclaration(BoundStructDeclaration node)
        {
            node = base.RewriteStructDeclaration(node);

            var changed = false;
            // 1. define default constructor

            var type = node.Type;

            var boundMembers = node.Members.Select(m => m.Member).ToList();
            var defaultCtor = type.Members.OfType<ConstructorSymbol>().FirstOrDefault(c => boundMembers.All(m => m != c));

            var members = node.Members;
            if (defaultCtor != null)
            {
                changed = true;
                var body = new BoundBlockStatement(ImmutableArray<BoundStatement>.Empty);
                var constructorDeclaration = new BoundConstructorDeclaration(defaultCtor, body);
                members = members.Add(constructorDeclaration);
            }

            // 2. prepend initializers to every constructor that does not call another constructor
            //var newCtors = new List<>
            var ctors = members.OfType<BoundConstructorDeclaration>().ToList();
            foreach (var ctor in ctors)
            {
                var initializerStatements = new List<BoundStatement>();

                var fields = members.OfType<BoundFieldDeclaration>();
                foreach (var field in fields)
                {
                    if (field.Initializer != null)
                    {
                        var fieldExpression = new BoundFieldExpression(null, field.Field);
                        var assign = new BoundAssignmentExpression(fieldExpression, field.Initializer);
                        var assignStatement = new BoundExpressionStatement(assign);
                        initializerStatements.Add(assignStatement);
                    }
                }

                if (initializerStatements.Any())
                {
                    changed = true;
                    var newStatements = ctor.Body.Statements.InsertRange(0, initializerStatements);
                    var newBody = new BoundBlockStatement(newStatements);
                    var newCtor = new BoundConstructorDeclaration(ctor.Constructor, newBody);
                    members = members.Replace(ctor, newCtor);
                }
            }

            return changed ? new BoundStructDeclaration(node.Type, members) : node;
        }
    }
}
