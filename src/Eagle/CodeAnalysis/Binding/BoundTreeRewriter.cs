using System;
using System.Collections.Immutable;

namespace Eagle.CodeAnalysis.Binding
{
    public abstract class BoundTreeRewriter
    {
        public virtual BoundStatement RewriteStatement(BoundStatement statement)
        {
            switch (statement)
            {
                case BoundVariableDeclaration v: return RewriteVariableDeclaration(v);
                case BoundIfStatement i: return RewriteIfStatement(i);
                case BoundWhileStatement w: return RewriteWhileStatement(w);
                case BoundLoopStatement l: return RewriteLoopStatement(l);
                case BoundBlockStatement b: return RewriteBlockStatement(b);
                case BoundForStatement f: return RewriteForStatement(f);
                case BoundLabelStatement l: return RewriteLabelStatement(l);
                case BoundGotoStatement g: return RewriteGotoStatement(g);
                case BoundConditionalGotoStatement c: return RewriteConditionalGotoStatement(c);
                case BoundExpressionStatement e: return RewriteExpressionStatement(e);
                case BoundReturnStatement r: return RewriteReturnStatement(r);
                default: throw new Exception($"Unexpected node '{statement.GetType().Name}'");
            }
        }

        private BoundStatement RewriteReturnStatement(BoundReturnStatement node)
        {
            BoundExpression? value = null;
            if (node.Value != null)
                value = RewriteExpression(node.Value);
            if (value == node.Value)
                return node;
            return new BoundReturnStatement(value);
        }

        protected virtual BoundStructDeclaration RewriteStructDeclaration(BoundStructDeclaration node)
        {
            var changed = false;
            var result = ImmutableArray.CreateBuilder<BoundMemberDeclaration>();
            foreach (var boundMember in node.Members)
            {
                var member = RewriteMember(boundMember);
                if (member != boundMember)
                    changed = true;
                result.Add(member);
            }

            return changed ? new BoundStructDeclaration(node.Type, result.ToImmutable()) : node;
        }

        protected virtual BoundMemberDeclaration RewriteMember(BoundMemberDeclaration node)
        {
            switch (node)
            {
                case BoundFieldDeclaration f: return RewriteFieldDeclaration(f);
                case BoundPropertyDeclaration p: return RewritePropertyDeclaration(p);
                case BoundMethodDeclaration m: return RewriteMethodDeclaration(m);
                case BoundConstructorDeclaration c: return RewriteConstructorDeclaration(c);
                default:
                    throw new Exception($"Unexpected node {node.GetType()}");
            }
        }

        protected virtual BoundMemberDeclaration RewriteConstructorDeclaration(BoundConstructorDeclaration node)
        {
            var body = RewriteBlockStatement(node.Body);
            if (body == node.Body)
                return node;
            return new BoundConstructorDeclaration(node.Constructor, body);
        }

        protected virtual BoundMemberDeclaration RewriteMethodDeclaration(BoundMethodDeclaration node)
        {
            var body = RewriteBlockStatement(node.Body);
            if (body == node.Body)
                return node;
            return new BoundMethodDeclaration(node.Method, body);
        }

        protected virtual BoundMemberDeclaration RewritePropertyDeclaration(BoundPropertyDeclaration node)
        {
            var initializer = node.Initializer == null ? null : RewriteExpression(node.Initializer);
            if (initializer == node.Initializer)
                return node;
            return new BoundPropertyDeclaration(node.Property, initializer);
        }

        protected virtual BoundMemberDeclaration RewriteFieldDeclaration(BoundFieldDeclaration node)
        {
            var initializer = node.Initializer == null ? null : RewriteExpression(node.Initializer);
            if (initializer == node.Initializer)
                return node;
            return new BoundFieldDeclaration(node.Field, initializer);
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
            var thenStatement = RewriteStatement(node.ThenStatement);
            var elseStatement = RewriteStatement(node.ElseStatement);
            if (condition == node.Condition && thenStatement == node.ThenStatement && elseStatement == node.ElseStatement)
                return node;
            return new BoundIfStatement(condition, thenStatement, elseStatement);
        }

        protected virtual BoundStatement RewriteForStatement(BoundForStatement node)
        {
            var lowerBound = RewriteExpression(node.LowerBound);
            var upperBound = RewriteExpression(node.UpperBound);
            var body = RewriteStatement(node.Body);
            if (lowerBound == node.LowerBound && upperBound == node.UpperBound && body == node.Body)
                return node;
            return new BoundForStatement(node.Variable, lowerBound, upperBound, body, node.BreakLabel, node.ContinueLabel);
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

        protected virtual BoundStatement RewriteLoopStatement(BoundLoopStatement node)
        {
            var body = RewriteStatement(node.Body);
            if (body == node.Body)
                return node;
            return new BoundLoopStatement(body, node.BreakLabel, node.ContinueLabel);
        }

        protected virtual BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            var condition = RewriteExpression(node.Condition);
            var body = RewriteStatement(node.Body);
            if (condition == node.Condition && body == node.Body)
                return node;
            return new BoundWhileStatement(condition, body, node.BreakLabel, node.ContinueLabel);
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
                case BoundFunctionCallExpression i: return RewriteCallExpression(i);
                case BoundParameterExpression p: return RewriteParameterExpression(p);
                case BoundConversionExpression c: return RewriteCastExpression(c);
                case BoundTypeExpression t: return RewriteTypeExpression(t);
                case BoundNewExpression n: return RewriteNewExpression(n);
                case BoundPropertyExpression m: return RewritePropertyExpression(m);
                case BoundConstExpression c: return RewriteConstExpression(c);
                case BoundFieldExpression f: return RewriteFieldExpression(f);
                case BoundMethodCallExpression m: return RewriteMethodCallExpression(m);
                case BoundConstructorCallExpression c: return RewriteConstructorCallExpression(c);
                case BoundErrorExpression e: return RewriteErrorExpression(e);
                default: throw new Exception($"Unexpected node '{statement.GetType().Name}'");
            }
        }

        protected virtual BoundExpression RewriteErrorExpression(BoundErrorExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewritePropertyExpression(BoundPropertyExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteConstructorCallExpression(BoundConstructorCallExpression node)
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

            return changed ? new BoundConstructorCallExpression(node.Constructor, result.ToImmutable()) : node;
        }

        protected virtual BoundExpression RewriteMethodCallExpression(BoundMethodCallExpression node)
        {
            var target = RewriteExpression(node.Target);

            var changed = false;
            var result = ImmutableArray.CreateBuilder<BoundExpression>();
            foreach (var boundExpression in node.Arguments)
            {
                var expression = RewriteExpression(boundExpression);
                if (expression != boundExpression)
                    changed = true;
                result.Add(expression);
            }

            return target != node.Target || changed ? new BoundMethodCallExpression(target, node.Method, result.ToImmutable()) : node;
        }

        protected virtual BoundExpression RewriteFieldExpression(BoundFieldExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteConstExpression(BoundConstExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteNewExpression(BoundNewExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteTypeExpression(BoundTypeExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteCastExpression(BoundConversionExpression node)
        {
            var expression = RewriteExpression(node.Expression);
            if (expression == node.Expression)
                return node;
            return new BoundConversionExpression(node.Type, expression);
        }

        protected virtual BoundExpression RewriteParameterExpression(BoundParameterExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteCallExpression(BoundFunctionCallExpression node)
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

            return changed ? new BoundFunctionCallExpression(node.Function, result.ToImmutable()) : node;
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
            var target = RewriteExpression(node.Target);
            var expression = RewriteExpression(node.Expression);
            if (target == node.Target && expression == node.Expression)
                return node;
            return new BoundAssignmentExpression(target, expression);
        }
    }
}