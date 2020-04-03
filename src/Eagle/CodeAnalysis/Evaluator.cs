using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Repl.CodeAnalysis.Binding;

namespace Repl.CodeAnalysis
{
    public class Evaluator
    {
        private readonly BoundUnit _program;
        private readonly Dictionary<Symbol, object> _globals;
        private LocalContext _locals = null;
        private readonly Stack<LocalContext> _stack = new Stack<LocalContext>();

        private object _lastValue;

        public Evaluator(
            BoundUnit program,
            Dictionary<Symbol, object> variables
            )
        {
            _program = program;
            _globals = variables;
        }

        public object Evaluate()
        {
            foreach (var node in _program.GetChildren())
            {
                EvaluateNode(node);
            }

            return _lastValue;
        }

        public void EvaluateNode(BoundNode node)
        {
            switch (node)
            {
                case BoundBlockStatement b:
                    EvaluateBlock(b);
                    break;
                case BoundFunctionDeclaration f:
                    EvaluateFunctionDeclaration(f);
                    break;
                case BoundExternDeclaration e:
                    EvaluateExternDeclaration(e);
                    break;
                case BoundAliasDeclaration a:
                    break;
                case BoundConstDeclaration c:
                    EvaluateConstDeclaration(c);
                    break;
                case BoundStructDeclaration s:
                    EvaluateStructDeclaration(s);
                    break;
                default:
                    throw new Exception($"Unexpected node {node.GetType()}");
            }
        }

        private void EvaluateStructDeclaration(BoundStructDeclaration node)
        {
            foreach (var member in node.Members)
            {
                EvaluateMemberDeclaration(member);
            }
        }

        private void EvaluateMemberDeclaration(BoundMemberDeclaration node)
        {
            switch (node)
            {
                case BoundFieldDeclaration f:
                    EvaluateFieldDeclaration(f);
                    break;
                case BoundPropertyDeclaration p:
                    EvaluatePropertyDeclaration(p);
                    break;
                case BoundMethodDeclaration m:
                    EvaluateMethodDeclaration(m);
                    break;
                case BoundConstructorDeclaration c:
                    EvaluateConstructorDeclaration(c);
                    break;
                default:
                    throw new Exception($"Unexpected node {node.GetType()}");
            }
        }

        private void EvaluateConstructorDeclaration(BoundConstructorDeclaration node)
        {
            var body = node.Body;

            var value = node.Body;

            var bb = new BB(body, node.Constructor.Parameters);
            _globals[node.Constructor] = bb;
            _lastValue = value;
        }

        private void EvaluateMethodDeclaration(BoundMethodDeclaration node)
        {
            var body = node.Body;

            var value = node.Body;

            var bb = new BB(body, node.Method.Parameters);
            _globals[node.Method] = bb;
            _lastValue = value;
        }

        private void EvaluatePropertyDeclaration(BoundPropertyDeclaration node)
        {
            var body = node.GetBody;

            var value = node.GetBody;

            var getter = node.Property.Getter;

            var bb = new BB(body, getter.Parameters);
            _globals[getter] = bb;
            _lastValue = value;
        }

        public object RunBlock(ImmutableArray<ParameterSymbol> parameters, object target, object[] args, BoundBlockStatement body)
        {
            using (StackFrame(target))
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    SetSymbolValue(parameter, args[i]);
                }

                return EvaluateBlock(body);
            }
        }

        private void EvaluateFieldDeclaration(BoundFieldDeclaration node)
        {
        }

        private void EvaluateConstDeclaration(BoundConstDeclaration node)
        {
            _globals[node.Const] = node.Value;
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
                        return EvaluateExpression(r.Value);
                    default:
                        throw new Exception($"Unexpected node {s.GetType()}");
                }
            }

            return _lastValue;
        }

        private void EvaluateExternDeclaration(BoundExternDeclaration node)
        {
            var method = typeof(Functions).GetMethod(node.Function.Name);

            if (method == null)
                throw new Exception($"Extern function {node.Function} was not found.");

            var builtInFunction = new BuiltInFunction(method);
            _globals[node.Function] = builtInFunction;
        }

        private void EvaluateFunctionDeclaration(BoundFunctionDeclaration node)
        {
            var body = node.Body;

            var value = node.Body;
            var bb = new BB(body, node.Function.Parameters);
            _globals[node.Function] = bb;
            _lastValue = value;
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
                case BoundParameterExpression p:
                    return EvaluateParameterExpression(p);
                case BoundConversionExpression c:
                    return EvaluateCastExpression(c);
                case BoundTypeExpression t:
                    return EvaluateTypeExpression(t);
                case BoundNewExpression n:
                    return EvaluateNewExpression(n);
                case BoundConstExpression c:
                    return EvaluateConstExpression(c);
                case BoundPropertyExpression m:
                    return EvaluatePropertyExpression(m);
                case BoundMethodCallExpression m:
                    return EvaluateMethodCallExpression(m);
                case BoundFieldExpression f:
                    return EvaluateFieldExpression(f);
                case BoundConstructorCallExpression c:
                    return EvaluateConstructorCallExpression(c);
                case BoundThisExpression t:
                    return EvaluateThisExpression(t);
                default:
                    throw new Exception($"Unexpected node {expression.GetType()}");
            }
        }

        private object EvaluateThisExpression(BoundThisExpression node)
        {
            return _locals.This;
        }

        private object EvaluateConstructorCallExpression(BoundConstructorCallExpression node)
        {
            var args = node.Arguments.Select(EvaluateExpression).ToArray();

            var target = new Dictionary<Symbol, object>();
            var fields = node.Type.Members.OfType<FieldSymbol>();
            foreach (var field in fields)
            {
                target[field] = GetDefaultValue(field.Type);
            }

            ((IInvokable)_globals[node.Constructor]).Invoke(this, target, args);
            return target;
        }

        private static T GetDefaultValue<T>()
        {
            return default;
        }

        private object GetDefaultValue(TypeSymbol type)
        {
            var m = GetType().GetMethod(nameof(GetDefaultValue), BindingFlags.Static | BindingFlags.NonPublic);
            return m.MakeGenericMethod(type.GetClrType()).Invoke(null, new object[0]);
        }

        private object EvaluateFieldExpression(BoundFieldExpression node)
        {
            object target = null;
            if (node.Target != null)
            {
                target = EvaluateExpression(node.Target);
            }

            target = target ?? _locals.This;

            var dic = (Dictionary<Symbol, object>)target;

            return dic[node.Field];
        }

        private object EvaluateMethodCallExpression(BoundMethodCallExpression node)
        {
            var target = EvaluateExpression(node.Target);
            var args = node.Arguments.Select(EvaluateExpression).ToArray();

            return ((IInvokable)_globals[node.Method]).Invoke(this, target, args);
        }

        private IDisposable StackFrame(object @this)
        {
            PushStackFrame(@this);
            return new DelegateDisposable(PopStackFrame);
        }

        private void PushStackFrame(object @this)
        {
            if (_locals != null)
                _stack.Push(_locals);
            _locals = new LocalContext(@this);
        }

        private void PopStackFrame()
        {
            _locals = _stack.Count > 0 ? _stack.Pop() : null;
        }

        private object EvaluatePropertyExpression(BoundPropertyExpression node)
        {
            var method = node.Property.Getter;

            var target = EvaluateExpression(node.Target);

            return ((IInvokable)_globals[method]).Invoke(this, target, new object[0]);
        }

        private object EvaluateConstExpression(BoundConstExpression node)
        {
            return _globals[node.Const];
        }

        private object EvaluateNewExpression(BoundNewExpression node)
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

        private object EvaluateParameterExpression(BoundParameterExpression node)
        {
            return GetSymbolValue(node.Parameter);
        }

        private object EvaluateFunctionCallExpression(BoundFunctionCallExpression node)
        {
            var args = node.Arguments.Select(EvaluateExpression).ToArray();
            return ((IInvokable)_globals[node.Function]).Invoke(this, null, args);
        }

        private void EvaluateVariableDeclaration(BoundVariableDeclaration node)
        {
            var value = EvaluateExpression(node.Initializer);
            SetSymbolValue(node.Variable, value);
            _lastValue = value;
        }

        private Dictionary<Symbol, object> GetValueStore()
        {
            return _locals?.Store ?? _globals;
        }

        private void SetSymbolValue(Symbol symbol, object value)
        {
            var store = GetValueStore();
            store[symbol] = value;
        }

        private object GetSymbolValue(Symbol symbol)
        {
            var store = GetValueStore();
            return store[symbol];
        }

        private void EvaluateExpressionStatement(BoundExpressionStatement node)
        {
            _lastValue = EvaluateExpression(node.Expression);
        }

        private object EvaluateVariableExpression(BoundVariableExpression node)
        {
            return GetSymbolValue(node.Variable);
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression node)
        {
            Dictionary<Symbol, object> target;
            Symbol symbol;
            Type t;
            if (node.Target is BoundVariableExpression v)
            {
                target = GetValueStore();
                symbol = v.Variable;
                t = v.Variable.Type.GetClrType();
            }
            else if (node.Target is BoundFieldExpression f)
            {
                target = (Dictionary<Symbol, object>)EvaluateExpression(f.Target);
                symbol = f.Field;
                t = f.Field.Type.GetClrType();
            }
            else if (node.Target is BoundPropertyExpression m)
            {
                target = (Dictionary<Symbol, object>)EvaluateExpression(m.Target);
                symbol = m.Property;
                t = m.Property.Type.GetClrType();
            }
            else
            {
                throw new Exception("Unsupported assignment target.");
            }

            var value = EvaluateExpression(node.Expression);
            target[symbol] = Convert.ChangeType(value, t);

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
    }
}