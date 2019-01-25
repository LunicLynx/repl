using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Repl.CodeAnalysis.Binding;

namespace Repl.CodeAnalysis
{
    public class Evaluator
    {
        private object _lastValue;
        private readonly BoundUnit _root;
        //private readonly Dictionary<ConstSymbol, object> _constants;
        //private readonly Dictionary<VariableSymbol, object> _variables;
        //private readonly Dictionary<FunctionSymbol, Delegate> _functions;
        //private readonly Dictionary<MethodSymbol, Delegate> _methods;
        //private Dictionary<ParameterSymbol, object> _arguments = new Dictionary<ParameterSymbol, object>();

        private readonly Dictionary<Symbol, object> _globals;

        private LocalContext _locals = null;
        private readonly Stack<LocalContext> _stack = new Stack<LocalContext>();

        public Evaluator(
            BoundUnit root,
            //Dictionary<ConstSymbol, object> constants,
            Dictionary<Symbol, object> globals
            //,
            //Dictionary<VariableSymbol, object> variables,
            //Dictionary<FunctionSymbol, Delegate> functions,
            //Dictionary<MethodSymbol, Delegate> methods
            )
        {
            _root = root;
            _globals = globals;
            //_constants = constants;
            //_variables = variables;
            //_functions = functions;
            //_methods = methods;
        }

        public object Evaluate()
        {
            foreach (var node in _root.GetChildren())
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

            _globals[node.Constructor] = (Func<object, object[], object>)((target, args) =>
            {
                for (var i = 0; i < node.Constructor.Parameters.Length; i++)
                {
                    var parameter = node.Constructor.Parameters[i];
                    SetSymbolValue(parameter, args[i]);
                }

                return EvaluateBlock(body);
            });
            _lastValue = value;
        }

        private void EvaluateMethodDeclaration(BoundMethodDeclaration node)
        {
            var body = node.Body;

            var value = node.Body;

            _globals[node.Method] = (Func<object, object[], object>)((target, args) =>
           {
               for (var i = 0; i < node.Method.Parameters.Length; i++)
               {
                   var parameter = node.Method.Parameters[i];
                   SetSymbolValue(parameter, args[i]);
               }

               return EvaluateBlock(body);
           });
            _lastValue = value;
        }

        private void EvaluatePropertyDeclaration(BoundPropertyDeclaration node)
        {
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
            var labelToIndex = new Dictionary<LabelSymbol, int>();

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

            _globals[node.Function] = (Func<object[], object>)(args => method.Invoke(null, args));
        }

        private void EvaluateFunctionDeclaration(BoundFunctionDeclaration node)
        {
            var body = node.Body;
            //var evaluator = new Evaluator(body, _variables, _functions);

            var value = node.Body;
            _globals[node.Function] = (Func<object[], object>)(args =>
           {
               for (var i = 0; i < node.Function.Parameters.Length; i++)
               {
                   var parameter = node.Function.Parameters[i];
                   SetSymbolValue(parameter, args[i]);
               }

               return EvaluateBlock(body);
           });
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
                case BoundCastExpression c:
                    return EvaluateCastExpression(c);
                case BoundTypeExpression t:
                    return EvaluateTypeExpression(t);
                case BoundNewExpression n:
                    return EvaluateNewExpression(n);
                case BoundConstExpression c:
                    return EvaluateConstExpression(c);
                case BoundMemberAccessExpression m:
                    return EvaluateMemberAccessExpression(m);
                case BoundMethodCallExpression m:
                    return EvaluateMethodCallExpression(m);
                case BoundFieldExpression f:
                    return EvaluateFieldExpression(f);
                case BoundConstructorCallExpression c:
                    return EvaluateConstructorCallExpression(c);
                default:
                    throw new Exception($"Unexpected node {expression.GetType()}");
            }
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
            using (StackFrame(target))
            {
                ((Delegate)_globals[node.Constructor]).DynamicInvoke(target, args);
                return target;
            }
        }

        private static T GetDefaultValue<T>()
        {
            return default;
        }

        private object GetDefaultValue(TypeSymbol type)
        {
            var m = GetType().GetMethod(nameof(GetDefaultValue), BindingFlags.Static | BindingFlags.NonPublic);
            return m.MakeGenericMethod(type.ClrType).Invoke(null, new object[0]);
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

            using (StackFrame(target))
            {
                return ((Delegate)_globals[node.Method]).DynamicInvoke(target, args);
            }
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

        private object EvaluateMemberAccessExpression(BoundMemberAccessExpression node)
        {
            if (node.Target.Type == TypeSymbol.String)
            {
                var value = (string)EvaluateExpression(node.Target);
                return typeof(string).GetProperty(node.Member.Name).GetValue(value, null);
            }

            var target = (IDictionary<Symbol, object>)EvaluateExpression(node.Target);
            return target[node.Member];
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

        private object EvaluateCastExpression(BoundCastExpression node)
        {
            return EvaluateExpression(node.Expression);
        }

        private object EvaluateParameterExpression(BoundParameterExpression node)
        {
            return GetSymbolValue(node.Parameter);
        }

        private object EvaluateFunctionCallExpression(BoundFunctionCallExpression node)
        {
            var args = node.Arguments.Select(EvaluateExpression).ToArray();
            using (StackFrame(null))
            {
                return ((Delegate)_globals[node.Function]).DynamicInvoke(new object[] { args });
            }
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
                t = v.Variable.Type.ClrType;
                //SetSymbolValue(v.Variable, value);
                //return value;
            }
            else if (node.Target is BoundFieldExpression f)
            {
                target = (Dictionary<Symbol, object>)_locals.This;
                symbol = f.Field;
                t = f.Field.Type.ClrType;
            }
            else if (node.Target is BoundMemberAccessExpression m)
            {
                target = (Dictionary<Symbol, object>)EvaluateExpression(m.Target);
                symbol = m.Member;
                t = m.Member.Type.ClrType;
            }
            else
            {
                //int x = 5L;
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

            switch (node.Operator.Kind)
            {
                case BoundUnaryOperatorKind.Identity: return (int)operand;
                case BoundUnaryOperatorKind.Negation: return -(int)operand;
                case BoundUnaryOperatorKind.LogicalNot: return !(bool)operand;
                case BoundUnaryOperatorKind.BitwiseComplement: return ~(int)operand;
                default: throw new Exception("Operator not implemented");
            }
        }

        private object EvaluateBinaryExpression(BoundBinaryExpression node)
        {
            var left = EvaluateExpression(node.Left);
            var right = EvaluateExpression(node.Right);

            switch (node.Operator.Kind)
            {
                case BoundBinaryOperatorKind.Addition: return (int)left + (int)right;
                case BoundBinaryOperatorKind.Subtraction: return (int)left - (int)right;
                case BoundBinaryOperatorKind.Multiplication: return (int)left * (int)right;
                case BoundBinaryOperatorKind.Division: return (int)left / (int)right;
                case BoundBinaryOperatorKind.Modulo: return (int)left % (int)right;
                case BoundBinaryOperatorKind.LogicalAnd: return (bool)left && (bool)right;
                case BoundBinaryOperatorKind.LogicalOr: return (bool)left || (bool)right;
                case BoundBinaryOperatorKind.Equals: return left == right;
                case BoundBinaryOperatorKind.NotEquals: return left != right;
                case BoundBinaryOperatorKind.LessThan: return (int)left < (int)right;
                case BoundBinaryOperatorKind.LessOrEquals: return (int)left <= (int)right;
                case BoundBinaryOperatorKind.GreaterThan: return (int)left > (int)right;
                case BoundBinaryOperatorKind.GreaterOrEquals: return (int)left >= (int)right;
                case BoundBinaryOperatorKind.BitwiseAnd when left is bool l && right is bool r: return l & r;
                case BoundBinaryOperatorKind.BitwiseAnd when left is int l && right is int r: return l & r;
                case BoundBinaryOperatorKind.BitwiseOr when left is bool l && right is bool r: return l | r;
                case BoundBinaryOperatorKind.BitwiseOr when left is int l && right is int r: return l | r;
                case BoundBinaryOperatorKind.BitwiseXor when left is bool l && right is bool r: return l ^ r;
                case BoundBinaryOperatorKind.BitwiseXor when left is int l && right is int r: return l ^ r;
                default: throw new Exception("Operator not implemented");

            }
        }
    }
}