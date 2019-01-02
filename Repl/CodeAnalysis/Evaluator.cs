using System;
using System.Collections.Generic;
using System.Linq;
using Repl.CodeAnalysis.Binding;

namespace Repl.CodeAnalysis
{
    public class Evaluator
    {
        private object _lastValue;
        private readonly BoundUnit _root;
        private readonly Dictionary<ConstSymbol, object> _constants;
        private readonly Dictionary<VariableSymbol, object> _variables;
        private readonly Dictionary<FunctionSymbol, Delegate> _functions;
        private Dictionary<ParameterSymbol, object> _arguments = new Dictionary<ParameterSymbol, object>();

        public Evaluator(
            BoundUnit root,
            Dictionary<ConstSymbol, object> constants,
            Dictionary<VariableSymbol, object> variables,
            Dictionary<FunctionSymbol, Delegate> functions)
        {
            _root = root;
            _constants = constants;
            _variables = variables;
            _functions = functions;
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
                    EvaluateBlock(b, new Dictionary<ParameterSymbol, object>());
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

        }

        private void EvaluateConstDeclaration(BoundConstDeclaration node)
        {
            _constants[node.Const] = node.Value;
        }

        public object EvaluateBlock(BoundBlockStatement block, Dictionary<ParameterSymbol, object> arguments)
        {
            _arguments = arguments;

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

            _functions[node.Function] = (Func<object[], object>)(args => method.Invoke(null, args));
        }

        private void EvaluateFunctionDeclaration(BoundFunctionDeclaration node)
        {
            var body = node.Body;
            //var evaluator = new Evaluator(body, _variables, _functions);

            var value = node.Body;
            _functions[node.Function] = (Func<object[], object>)(args =>
           {
               var arguments = new Dictionary<ParameterSymbol, object>();
               for (var i = 0; i < node.Function.Parameters.Length; i++)
               {
                   var parameter = node.Function.Parameters[i];
                   arguments[parameter] = args[i];
               }

               return EvaluateBlock(body, arguments);
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
                case BoundCallExpression i:
                    return EvaluateCallExpression(i);
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
                default:
                    throw new Exception($"Unexpected node {expression.GetType()}");
            }
        }

        private object EvaluateMemberAccessExpression(BoundMemberAccessExpression node)
        {
            if (node.Target.Type == TypeSymbol.String)
            {
                var value = (string)EvaluateExpression(node.Target);
                return typeof(string).GetProperty(node.Member.Name).GetValue(value, null);
            }

            var target = (IDictionary<string, object>)EvaluateExpression(node.Target);
            return target[node.Member.Name];
        }

        private object EvaluateConstExpression(BoundConstExpression node)
        {
            return _constants[node.Const];
        }

        private object EvaluateNewExpression(BoundNewExpression node)
        {
            return new Dictionary<string, object>();
        }

        private object EvaluateTypeExpression(BoundTypeExpression node)
        {
            return new Dictionary<string, object>();
        }

        private object EvaluateCastExpression(BoundCastExpression node)
        {
            return EvaluateExpression(node.Expression);
        }

        private object EvaluateParameterExpression(BoundParameterExpression node)
        {
            return _arguments[node.Parameter];
        }

        private object EvaluateCallExpression(BoundCallExpression node)
        {
            var args = node.Arguments.Select(EvaluateExpression).ToArray();
            return _functions[node.Function].DynamicInvoke(new object[] { args });
        }

        private void EvaluateVariableDeclaration(BoundVariableDeclaration node)
        {
            var value = EvaluateExpression(node.Initializer);
            _variables[node.Variable] = value;
            _lastValue = value;
        }

        private void EvaluateExpressionStatement(BoundExpressionStatement node)
        {
            _lastValue = EvaluateExpression(node.Expression);
        }

        private object EvaluateVariableExpression(BoundVariableExpression node)
        {
            var value = _variables[node.Variable];
            return value;
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression node)
        {
            var value = EvaluateExpression(node.Expression);
            _variables[node.Variable] = value;
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