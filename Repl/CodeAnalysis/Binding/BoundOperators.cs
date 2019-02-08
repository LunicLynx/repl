using System;
using System.Collections.Generic;
using System.Linq;

namespace Repl.CodeAnalysis.Binding
{
    public static class BoundOperators
    {
        public static T[] GetOperators<T>(IScope scope,
            Func<TypeSymbol, TypeSymbol, IEnumerable<T>> numericalOperatorsSigned,
            Func<TypeSymbol, TypeSymbol, IEnumerable<T>> numericalOperatorsUnsigned,
            Func<TypeSymbol, IEnumerable<T>> booleanOperators)
        {

            //scope.TryLookup(NativeTypeNames.UInt16);

            var boolType = TypeSymbol.Bool;
            var numericalTypesSigned = new[]
            {
                //TypeSymbol.I8,
                TypeSymbol.I16,
                TypeSymbol.I32,
                TypeSymbol.I64,
                TypeSymbol.Int,

            };

            var numericalTypesUnsigned = new[]
            {
                //TypeSymbol.U8,
                TypeSymbol.U16,
                TypeSymbol.U32,
                TypeSymbol.U64,
                TypeSymbol.Uint
            };

            return Enumerable.Empty<T>()
                .Concat(numericalTypesSigned.SelectMany(t => numericalOperatorsSigned(t, boolType)))
                .Concat(numericalTypesUnsigned.SelectMany(t => numericalOperatorsUnsigned(t, boolType)))
                .Concat(booleanOperators(boolType))
                .ToArray();
        }
    }
}