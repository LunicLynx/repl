using System;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Binding
{
    public abstract class BoundTreeRewriter
    {
        public virtual BoundNode RewriteNode(BoundNode node)
        {
            switch (node)
            {
                case BoundStatement s: return RewriteStatement(s);
                case BoundExternDeclaration e: return RewriteExternDeclaration(e);
                case BoundFunctionDeclaration f: return RewriteFunctionDeclaration(f);
                case BoundStructDeclaration s: return RewriteStructDeclaration(s);
                case BoundAliasDeclaration a: return RewriteAliasDeclaration(a);
                case BoundConstDeclaration c: return RewriteConstDeclaration(c);
                default: throw new Exception($"Unexpected node '{node.GetType().Name}'");
            }
        }

        private BoundNode RewriteConstDeclaration(BoundConstDeclaration node)
        {
            return node;
        }

        private BoundNode RewriteAliasDeclaration(BoundAliasDeclaration node)
        {
            return node;
        }

        public virtual BoundStatement RewriteStatement(BoundStatement statement)
        {
            switch (statement)
            {
                case BoundVariableDeclaration v: return RewriteVariableDeclaration(v);
                case BoundIfStatement i: return RewriteIfStatement(i);
                case BoundWhileStatement w: return RewriteWhileStatement(w);
                case BoundLoopStatement l: return RewriteLoopStatement(l);
                case BoundBreakStatement b: return RewriteBreakStatement(b);
                case BoundContinueStatement c: return RewriteContinueStatement(c);
                case BoundBlockStatement b: return RewriteBlockStatement(b);
                case BoundForStatement f: return RewriteForStatement(f);
                case BoundLabelStatement l: return RewriteLabelStatement(l);
                case BoundGotoStatement g: return RewriteGotoStatement(g);
                case BoundConditionalGotoStatement c: return RewriteConditionalGotoStatement(c);
                case BoundExpressionStatement e: return RewriteExpressionStatement(e);
                default: throw new Exception($"Unexpected node '{statement.GetType().Name}'");
            }
        }

        protected virtual BoundStructDeclaration RewriteStructDeclaration(BoundStructDeclaration node)
        {
            return node;
        }

        protected virtual BoundFunctionDeclaration RewriteFunctionDeclaration(BoundFunctionDeclaration node)
        {
            var body = RewriteBlockStatement(node.Body);
            if (body == node.Body)
                return node;
            return new BoundFunctionDeclaration(node.Function, body);
        }

        protected virtual BoundExternDeclaration RewriteExternDeclaration(BoundExternDeclaration node)
        {
            return node;
        }

        protected virtual BoundStatement RewriteConditionalGotoStatement(BoundConditionalGotoStatement node)
        {
            var condition = RewriteExpression(node.Condition);
            if (condition == node.Condition)
                return node;
            return new BoundConditionalGotoStatement(node.Label, condition, node.JumpIfTrue);
        }

        protected virtual BoundStatement RewriteGotoStatement(BoundGotoStatement node)
        {
            return node;
        }

        protected virtual BoundStatement RewriteLabelStatement(BoundLabelStatement node)
        {
            return node;
        }

        protected virtual BoundStatement RewriteIfStatement(BoundIfStatement node)
        {
            var condition = RewriteExpression(node.Condition);
            var thenBlock = RewriteBlockStatement(node.ThenBlock);
            var elseStatement = RewriteStatement(node.ElseStatement);
            if (condition == node.Condition && thenBlock == node.ThenBlock && elseStatement == node.ElseStatement)
                return node;
            return new BoundIfStatement(condition, thenBlock, elseStatement);
        }

        protected virtual BoundStatement RewriteForStatement(BoundForStatement node)
        {
            var lowerBound = RewriteExpression(node.LowerBound);
            var upperBound = RewriteExpression(node.UpperBound);
            var body = RewriteBlockStatement(node.Body);
            if (lowerBound == node.LowerBound && upperBound == node.UpperBound && body == node.Body)
                return node;
            return new BoundForStatement(node.Variable, lowerBound, upperBound, body);
        }

        protected virtual BoundStatement RewriteExpressionStatement(BoundExpressionStatement node)
        {
            var expression = RewriteExpression(node.Expression);
            if (expression == node.Expression)
                return node;
            return new BoundExpressionStatement(expression);
        }

        protected virtual BoundBlockStatement RewriteBlockStatement(BoundBlockStatement node)
        {
            var changed = false;
            var result = ImmutableArray.CreateBuilder<BoundStatement>();
            foreach (var boundStatement in node.Statements)
            {
                var statement = RewriteStatement(boundStatement);
                if (statement != boundStatement)
                    changed = true;
                result.Add(statement);
            }

            return changed ? new BoundBlockStatement(result.ToImmutable()) : node;
        }

        protected virtual BoundStatement RewriteContinueStatement(BoundContinueStatement node)
        {
            return node;
        }

        protected virtual BoundStatement RewriteBreakStatement(BoundBreakStatement node)
        {
            return node;
        }

        protected virtual BoundStatement RewriteLoopStatement(BoundLoopStatement node)
        {
            var body = RewriteBlockStatement(node.Body);
            if (body == node.Body)
                return node;
            return new BoundLoopStatement(body);
        }

        protected virtual BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            var condition = RewriteExpression(node.Condition);
            var body = RewriteBlockStatement(node.Body);
            if (condition == node.Condition && body == node.Body)
                return node;
            return new BoundWhileStatement(condition, body);
        }

        protected virtual BoundStatement RewriteVariableDeclaration(BoundVariableDeclaration node)
        {
            var initializer = RewriteExpression(node.Initializer);
            if (initializer == node.Initializer)
                return node;
            return new BoundVariableDeclaration(node.Variable, initializer);
        }

        public virtual BoundExpression RewriteExpression(BoundExpression statement)
        {
            switch (statement)
            {
                case BoundAssignmentExpression a: return RewriteAssignmentExpression(a);
                case BoundBinaryExpression b: return RewriteBinaryExpression(b);
                case BoundUnaryExpression u: return RewriteUnaryExpression(u);
                case BoundLiteralExpression l: return RewriteLiteralExpression(l);
                case BoundVariableExpression v: return RewriteVariableExpression(v);
                case BoundCallExpression i: return RewriteCallExpression(i);
                case BoundParameterExpression p: return RewriteParameterExpression(p);
                case BoundCastExpression c: return RewriteCastExpression(c);
                case BoundTypeExpression t: return RewriteTypeExpression(t);
                case BoundNewExpression n: return RewriteNewExpression(n);
                case BoundMemberAccessExpression m: return RewriteMemberAccessExpression(m);
                case BoundConstExpression c: return RewriteConstExpression(c);
                default: throw new Exception($"Unexpected node '{statement.GetType().Name}'");
            }
        }

        private BoundExpression RewriteConstExpression(BoundConstExpression node)
        {
            return node;
        }

        private BoundExpression RewriteMemberAccessExpression(BoundMemberAccessExpression node)
        {
            var target = RewriteExpression(node.Target);
            if (target == node.Target)
                return node;
            return new BoundMemberAccessExpression(target, node.Member);
        }

        private BoundExpression RewriteNewExpression(BoundNewExpression node)
        {
            return node;
        }

        private BoundExpression RewriteTypeExpression(BoundTypeExpression node)
        {
            return node;
        }

        private BoundExpression RewriteCastExpression(BoundCastExpression node)
        {
            var expression = RewriteExpression(node.Expression);
            if (expression == node.Expression)
                return node;
            return new BoundCastExpression(node.Type, expression);
        }

        private BoundExpression RewriteParameterExpression(BoundParameterExpression node)
        {
            return node;
        }

        private BoundExpression RewriteCallExpression(BoundCallExpression node)
        {
            var changed = false;
            var result = ImmutableArray.CreateBuilder<BoundExpression>();
            foreach (var boundExpression in node.Arguments)
            {
                var expression = RewriteExpression(boundExpression);
                if (expression != boundExpression)
                    changed = true;
                result.Add(expression);
            }

            return changed ? new BoundCallExpression(node.Function, result.ToImmutable()) : node;
        }

        protected virtual BoundExpression RewriteVariableExpression(BoundVariableExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteLiteralExpression(BoundLiteralExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteUnaryExpression(BoundUnaryExpression node)
        {
            var operand = RewriteExpression(node.Operand);
            if (operand == node.Operand)
                return node;
            return new BoundUnaryExpression(node.Operator, operand);
        }

        protected virtual BoundExpression RewriteBinaryExpression(BoundBinaryExpression node)
        {
            var left = RewriteExpression(node.Left);
            var right = RewriteExpression(node.Right);
            if (left == node.Left && right == node.Right)
                return node;
            return new BoundBinaryExpression(left, node.Operator, right);
        }

        protected virtual BoundExpression RewriteAssignmentExpression(BoundAssignmentExpression node)
        {
            var expression = RewriteExpression(node.Expression);
            if (expression == node.Expression)
                return node;
            return new BoundAssignmentExpression(node.Variable, expression);
        }
    }
}