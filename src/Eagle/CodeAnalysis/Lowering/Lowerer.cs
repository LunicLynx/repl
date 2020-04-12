using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Eagle.CodeAnalysis.Binding;

namespace Eagle.CodeAnalysis.Lowering
{
    internal class Lowerer : BoundTreeRewriter
    {
        private static int _labelCount;

        private Lowerer() { }

        public static BoundBlockStatement Lower(BoundStatement statement)
        {
            var lowerer = new Lowerer();
            var result = lowerer.RewriteStatement(statement);
            return Flatten(result);
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

        private static BoundLabel GenerateLabel()
        {
            var name = $"Label{++_labelCount}";
            return new BoundLabel(name);
        }

        protected override BoundStatement RewriteLoopStatement(BoundLoopStatement node)
        {
            var continueLabelStatement = new BoundLabelStatement(node.ContinueLabel);

            var gotoStart = new BoundGotoStatement(node.ContinueLabel);
            var endLabelStatement = new BoundLabelStatement(node.BreakLabel);

            var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(
                // gotoCheck
                continueLabelStatement,
                node.Body,
                // checkLabelStatement
                gotoStart,
                endLabelStatement
                ));

            return RewriteStatement(result);
        }

        protected override BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            // TODO Lower into loop statement
            // can be done by inverting the check as break eg. gotoEnd;
            var bodyLabel = GenerateLabel();

            var gotoContinue = new BoundGotoStatement(node.ContinueLabel);
            var bodyLabelStatement = new BoundLabelStatement(bodyLabel);
            var continueLabelStatement = new BoundLabelStatement(node.ContinueLabel);
            var gotoTrue = new BoundConditionalGotoStatement(bodyLabel, node.Condition);
            var breakLabelStatement = new BoundLabelStatement(node.BreakLabel);

            var result = new BoundBlockStatement(ImmutableArray.Create(
                gotoContinue,
                bodyLabelStatement,
                node.Body,
                continueLabelStatement,
                gotoTrue,
                breakLabelStatement
            ));

            return RewriteStatement(result);
        }

        protected override BoundStatement RewriteIfStatement(BoundIfStatement node)
        {
            if (node.ElseStatement == null)
            {
                var endLabel = GenerateLabel();

                var gotoFalse = new BoundConditionalGotoStatement(endLabel, node.Condition, false);
                var endLabelStatement = new BoundLabelStatement(endLabel);

                var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(
                    gotoFalse,
                    node.ThenStatement,
                    endLabelStatement));

                return RewriteStatement(result);
            }
            else
            {
                var elseLabel = GenerateLabel();
                var endLabel = GenerateLabel();

                var gotoFalse = new BoundConditionalGotoStatement(elseLabel, node.Condition, false);
                var gotoEndStatement = new BoundGotoStatement(endLabel);
                var elseLabelStatement = new BoundLabelStatement(elseLabel);
                var endLabelStatement = new BoundLabelStatement(endLabel);

                var result = new BoundBlockStatement(ImmutableArray.Create(
                    gotoFalse,
                    node.ThenStatement,
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

            var upperBoundVariable = new LocalVariableSymbol("upperBound", true, TypeSymbol.Int);
            var upperBoundDeclaration = new BoundVariableDeclaration(upperBoundVariable, node.UpperBound);
            var condition = new BoundBinaryExpression(
                variableExpression,
                BoundBinaryOperator.Bind(Syntax.TokenKind.LessEquals, TypeSymbol.Int, TypeSymbol.Int),
                new BoundVariableExpression(upperBoundVariable)
            );

            var continueLabelStatement = new BoundLabelStatement(node.ContinueLabel);
            var increment = new BoundExpressionStatement(
                new BoundAssignmentExpression(
                    new BoundVariableExpression(node.Variable),
                    new BoundBinaryExpression(
                        variableExpression,
                        BoundBinaryOperator.Bind(Syntax.TokenKind.Plus, TypeSymbol.Int, TypeSymbol.Int),
                        new BoundLiteralExpression(TypeSymbol.Int, 1))));

            var whileBody = new BoundBlockStatement(ImmutableArray.Create(
                node.Body,
                continueLabelStatement,
                increment
            ));

            var whileStatement = new BoundWhileStatement(condition, whileBody, node.BreakLabel, GenerateLabel());

            var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(
                variableDeclaration,
                upperBoundDeclaration,
                whileStatement));

            return RewriteStatement(result);
        }

        protected override BoundStructDeclaration RewriteStructDeclaration(BoundStructDeclaration node)
        {
            // TODO this should probably be at the end
            node = base.RewriteStructDeclaration(node);

            var changed = false;
            var type = node.Type;
            var members = node.Members;

            // 0. Rewrite properties to field and method
            var props = members.OfType<BoundPropertyDeclaration>().ToList();
            foreach (var prop in props)
            {
                changed = true;
                var symbol = prop.Property;
                members = members.Remove(prop);
                var getter = new BoundMethodDeclaration(symbol.Getter, prop.GetBody);
                members = members.Add(getter);
            }

            // 1. define default constructor
            var boundMembers = members.Select(m => m.Member).ToList();
            var defaultCtor = type.Members.OfType<ConstructorSymbol>().FirstOrDefault(c => boundMembers.All(m => m != c));

            if (defaultCtor != null)
            {
                changed = true;
                var body = new BoundBlockStatement(ImmutableArray<BoundStatement>.Empty);
                var constructorDeclaration = new BoundConstructorDeclaration(defaultCtor, body);
                members = members.Add(constructorDeclaration);
            }

            // 2. prepend initializers to every constructor that does not call another constructor
            var ctors = members.OfType<BoundConstructorDeclaration>().ToList();
            foreach (var ctor in ctors)
            {
                var initializerStatements = new List<BoundStatement>();

                // TODO add properties
                var fields = members.OfType<BoundFieldDeclaration>();
                foreach (var field in fields)
                {
                    if (field.Initializer != null)
                    {
                        var fieldExpression = new BoundFieldExpression(null, field.Field);
                        var assign = new BoundAssignmentExpression(fieldExpression, field.Initializer);
                        var assignStatement = new BoundExpressionStatement(assign);
                        initializerStatements.Add(assignStatement);
                        var newField = new BoundFieldDeclaration(field.Field, null);
                        members = members.Replace(field, newField);
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

        // TODO lower property declaration 
        //  field and methods


        protected override BoundMemberDeclaration RewriteMember(BoundMemberDeclaration node)
        {
            return base.RewriteMember(node);
        }

        // TODO lower property expression
        //  get for read and set for assignment

        protected override BoundExpression RewritePropertyExpression(BoundPropertyExpression node)
        {
            var call = new BoundMethodCallExpression(node.Target, node.Property.Getter, ImmutableArray<BoundExpression>.Empty);
            //return base.RewritePropertyExpression(node);
            return call;
        }

        protected override BoundExpression RewriteAssignmentExpression(BoundAssignmentExpression node)
        {
            if (node.Target is BoundPropertyExpression p)
            {
                while (false) ;
            }
            return base.RewriteAssignmentExpression(node);
        }

    }
}
