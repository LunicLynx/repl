using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eagle.CodeAnalysis.Binding;

namespace Eagle.CodeAnalysis
{
    public class Evaluator
    {
        private readonly BoundProgram _program;
        private readonly Dictionary<VariableSymbol, object> _globals;
        private readonly Dictionary<IInvokableSymbol, BoundBlockStatement> _functions = new Dictionary<IInvokableSymbol, BoundBlockStatement>();
        private readonly Stack<Dictionary<VariableSymbol, object>> _locals = new Stack<Dictionary<VariableSymbol, object>>();

        private object _lastValue;

        public Evaluator(BoundProgram program, Dictionary<VariableSymbol, object> variables)
        {
            _program = program;
            _globals = variables;
            _locals.Push(new Dictionary<VariableSymbol, object>());

            var current = program;
            while (current != null)
            {
                foreach (var kv in current.Functions)
                {
                    var invokable = kv.Key;
                    var body = kv.Value;
                    _functions.Add(invokable, body);
                }

                current = current.Previous;
            }
        }

        public object Evaluate()
        {
            var function = _program.MainFunction ?? _program.ScriptFunction;
            if (function == null)
                return null;

            var body = _functions[function];
            return EvaluateBlock(body);
        }

        public object EvaluateBlock(BoundBlockStatement block)
        {
            var labelToIndex = new Dictionary<BoundLabel, int>();

            var statements = block.Statements;
            for (var i = 0; i < statements.Length; i++)
            {
                if (statements[i] is BoundLabelStatement l)
                    labelToIndex.Add(l.Label, i + 1);
            }

            var index = 0;
            while (index < statements.Length)
            {
                var s = statements[index];
                switch (s)
                {
                    case BoundVariableDeclaration v:
                        EvaluateVariableDeclaration(v);
                        index++;
                        break;
                    case BoundExpressionStatement e:
                        EvaluateExpressionStatement(e);
                        index++;
                        break;
                    case BoundGotoStatement g:
                        index = labelToIndex[g.Label];
                        break;
                    case BoundConditionalGotoStatement c:
                        var condition = (bool)EvaluateExpression(c.Condition);
                        if (condition == c.JumpIfTrue)
                            index = labelToIndex[c.Label];
                        else
                            index++;
                        break;

                    case BoundLabelStatement _:
                        index++;
                        break;
                    case BoundReturnStatement r:
                        if (r.Expression != null)
                            return EvaluateExpression(r.Expression);
                        return _lastValue;
                    default:
                        throw new Exception($"Unexpected node {s.GetType()}");
                }
            }

            return _lastValue;
        }

        private object EvaluateExpression(BoundExpression expression)
        {
            switch (expression)
            {
                case BoundBinaryExpression b:
                    return EvaluateBinaryExpression(b);
                case BoundUnaryExpression u:
                    return EvaluateUnaryExpression(u);
                case BoundLiteralExpression l:
                    return EvaluateLiteralExpression(l);
                case BoundAssignmentExpression a:
                    return EvaluateAssignmentExpression(a);
                case BoundVariableExpression v:
                    return EvaluateVariableExpression(v);
                case BoundFunctionCallExpression i:
                    return EvaluateFunctionCallExpression(i);
                case BoundConversionExpression c:
                    return EvaluateCastExpression(c);
                case BoundTypeExpression t:
                    return EvaluateTypeExpression(t);
                case BoundNewInstanceExpression n:
                    return EvaluateNewInstanceExpression(n);
                case BoundPropertyExpression m:
                    return EvaluatePropertyExpression(m);
                default:
                    throw new Exception($"Unexpected node {expression.GetType()}");
            }
        }

        private object EvaluatePropertyExpression(BoundPropertyExpression node)
        {
            var method = node.Property.Getter;

            var target = EvaluateExpression(node.Target);

            var locals = new Dictionary<VariableSymbol, object>();
            _locals.Push(locals);

            var statement = _functions[method];
            var result = EvaluateBlock(statement);

            _locals.Pop();
            return result;
            //return ((IInvokable)_functions[method]).Invoke(this, target, new object[0]);
        }

        private object EvaluateNewInstanceExpression(BoundNewInstanceExpression node)
        {
            return new Dictionary<Symbol, object>();
            // TODO call all initializer
        }

        private object EvaluateTypeExpression(BoundTypeExpression node)
        {
            return new Dictionary<Symbol, object>();
            // TODO call all initializer
        }

        private object EvaluateCastExpression(BoundConversionExpression node)
        {
            var value = EvaluateExpression(node.Expression);
            return Convert.ChangeType(value, node.Type.GetClrType());
        }

        private object EvaluateFunctionCallExpression(BoundFunctionCallExpression node)
        {
            if (node.Function.Extern)
            {
                var args = node.Arguments.Select(EvaluateExpression).ToArray();

                // managed to native transition
                if (node.Function.Name == "Print")
                {
                    Console.WriteLine(args[0]);
                    return null;
                }
                else if (node.Function.Name == "Input")
                {
                    return Console.ReadLine();
                }

                Debugger.Break();
            }

            var locals = new Dictionary<VariableSymbol, object>();
            for (int i = 0; i < node.Arguments.Length; i++)
            {
                var parameter = node.Function.Parameters[i];
                var value = EvaluateExpression(node.Arguments[i]);
                locals.Add(parameter, value);
            }

            _locals.Push(locals);

            var statement = _functions[node.Function];
            var result = EvaluateBlock(statement);

            _locals.Pop();

            return result;
        }

        private void EvaluateVariableDeclaration(BoundVariableDeclaration node)
        {
            var value = EvaluateExpression(node.Initializer);
            _lastValue = value;
            Assign(node.Variable, value);
        }

        private void EvaluateExpressionStatement(BoundExpressionStatement node)
        {
            _lastValue = EvaluateExpression(node.Expression);
        }

        private object EvaluateVariableExpression(BoundVariableExpression node)
        {
            if (node.Variable is GlobalVariableSymbol)
            {
                return _globals[node.Variable];
            }

            var locals = _locals.Peek();
            return locals[node.Variable];
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression node)
        {
            var value = EvaluateExpression(node.Expression);

            if (node.Target is BoundVariableExpression v)
            {
                var variable = v.Variable;
                Assign(variable, value);
            }
            else
            {
                throw new Exception("Unsupported assignment target.");
            }

            return value;
        }

        private object EvaluateLiteralExpression(BoundLiteralExpression node)
        {
            return node.Value;
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression node)
        {
            var operand = EvaluateExpression(node.Operand);

            var @delegate = EvalOperators.UnaryOperators[node.Operator];
            return @delegate.DynamicInvoke(operand);
        }

        private object EvaluateBinaryExpression(BoundBinaryExpression node)
        {
            var left = EvaluateExpression(node.Left);
            var right = EvaluateExpression(node.Right);

            var @delegate = EvalOperators.BinaryOperators[node.Operator];
            return @delegate.DynamicInvoke(left, right);
        }

        private void Assign(VariableSymbol variable, object value)
        {
            if (variable is GlobalVariableSymbol)
            {
                _globals[variable] = value;
            }
            else
            {
                var locals = _locals.Peek();
                locals[variable] = value;
            }
        }
    }
}