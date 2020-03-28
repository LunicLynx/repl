using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Repl.CodeAnalysis.Binding;

namespace Repl.CodeAnalysis
{
    public class EvalOperators
    {
        private static readonly Dictionary<BoundUnaryOperator, Delegate> _unaryOperators = new Dictionary<BoundUnaryOperator, Delegate>();
        private static readonly Dictionary<BoundBinaryOperator, Delegate> _binaryOperators = new Dictionary<BoundBinaryOperator, Delegate>();

        public static IReadOnlyDictionary<BoundUnaryOperator, Delegate> UnaryOperators => _unaryOperators;

        public static IReadOnlyDictionary<BoundBinaryOperator, Delegate> BinaryOperators => _binaryOperators;

        static EvalOperators()
        {
            var concatMethod = typeof(string).GetMethod("Concat", new[] { typeof(object), typeof(object) });

            foreach (var @operator in BoundUnaryOperator.Operators)
            {
                var i = (uint)1;
                //var x = -i;
                var x = i + i;
                var p = Expression.Parameter(@operator.OperandType.GetClrType());
                Expression e = p;
                switch (@operator.Kind)
                {
                    case BoundUnaryOperatorKind.Negation:
                        e = Expression.Negate(p);
                        break;
                    case BoundUnaryOperatorKind.LogicalNot:
                        e = Expression.Not(p);
                        break;
                    case BoundUnaryOperatorKind.BitwiseComplement:
                        e = Expression.OnesComplement(p);
                        break;
                    case BoundUnaryOperatorKind.Identity:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var l = Expression.Lambda(e, p);

                var @delegate = l.Compile();
                _unaryOperators[@operator] = @delegate;
            }

            foreach (var @operator in BoundBinaryOperator.Operators)
            {

                var pl = Expression.Parameter(@operator.LeftType.GetClrType());
                var pr = Expression.Parameter(@operator.RightType.GetClrType());
                Expression e;
                switch (@operator.Kind)
                {

                    case BoundBinaryOperatorKind.Addition:
                        e = Expression.Add(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.Multiplication:
                        e = Expression.Multiply(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.Subtraction:
                        e = Expression.Subtract(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.Division:
                        e = Expression.Divide(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.LogicalAnd:
                        e = Expression.AndAlso(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.LogicalOr:
                        e = Expression.OrElse(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.Equal:
                        e = Expression.Equal(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.NotEqual:
                        e = Expression.NotEqual(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.LessThan:
                        e = Expression.LessThan(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.LessOrEqual:
                        e = Expression.LessThanOrEqual(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.GreaterThan:
                        e = Expression.GreaterThan(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.GreaterOrEqual:
                        e = Expression.GreaterThanOrEqual(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.BitwiseAnd:
                        e = Expression.And(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.BitwiseOr:
                        e = Expression.Or(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.BitwiseXor:
                        e = Expression.ExclusiveOr(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.Modulo:
                        e = Expression.Modulo(pl, pr);
                        break;
                    case BoundBinaryOperatorKind.Concatenation:
                        Expression el = pl;
                        Expression er = pr;
                        if (el.Type != typeof(string))
                            el = Expression.Convert(el, typeof(object));
                        if (er.Type != typeof(string))
                            er = Expression.Convert(er, typeof(object));
                        e = Expression.Add(el, er, concatMethod);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var l = Expression.Lambda(e, pl, pr);

                var @delegate = l.Compile();
                _binaryOperators[@operator] = @delegate;
            }
        }

    }
}