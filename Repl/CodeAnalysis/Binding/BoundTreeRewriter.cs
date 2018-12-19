﻿using System;
using System.Collections.Immutable;

namespace Repl.CodeAnalysis.Binding
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

        private BoundStatement RewriteConditionalGotoStatement(BoundConditionalGotoStatement node)
        {
            var condition = RewriteExpression(node.Condition);
            if (condition == node.Condition)
                return node;
            return new BoundConditionalGotoStatement(node.Label, condition, node.JumpIfTrue);
        }

        private BoundStatement RewriteGotoStatement(BoundGotoStatement node)
        {
            return node;
        }

        private BoundStatement RewriteLabelStatement(BoundLabelStatement node)
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
                default: throw new Exception($"Unexpected node '{statement.GetType().Name}'");
            }
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