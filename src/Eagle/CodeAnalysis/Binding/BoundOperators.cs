﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Repl.CodeAnalysis.Binding
{
    public static class BoundOperators
    {
        public static T[] GetOperators<T>(//IScope scope,
            Func<TypeSymbol, TypeSymbol, IEnumerable<T>> numericalOperatorsSigned,
            Func<TypeSymbol, TypeSymbol, IEnumerable<T>> numericalOperatorsUnsigned,
            Func<TypeSymbol, IEnumerable<T>> booleanOperators,
            Func<TypeSymbol, TypeSymbol, IEnumerable<T>> stringConcatOperators = null,
            Func<TypeSymbol, TypeSymbol, IEnumerable<T>> stringOperators = null
            )
        {

            var stringType = TypeSymbol.String;
            //scope.TryLookup(NativeTypeNames.UInt16);
            var boolType = TypeSymbol.Bool;
            //scope.GetTypeSymbol(NativeTypeNames.Boolean);
            var numericalTypesSigned = new[]
            {
                //scope.GetTypeSymbol(NativeTypeNames.Int16),
                //scope.GetTypeSymbol(NativeTypeNames.Int32),
                //scope.GetTypeSymbol(NativeTypeNames.Int64),
                //TypeSymbol.I8,
                TypeSymbol.I16,
                TypeSymbol.I32,
                TypeSymbol.I64,
                TypeSymbol.Int,
            }.Where(x => x != null);

            var numericalTypesUnsigned = new[]
            {
                //scope.GetTypeSymbol(NativeTypeNames.UInt16),
                //scope.GetTypeSymbol(NativeTypeNames.UInt32),
                //scope.GetTypeSymbol(NativeTypeNames.UInt64),
                //TypeSymbol.U8,
                TypeSymbol.U16,
                TypeSymbol.U32,
                TypeSymbol.U64,
                TypeSymbol.UInt
            }.Where(x => x != null);


            var allTypesExceptString =
                new[] { boolType }
                    .Concat(numericalTypesUnsigned)
                    .Concat(numericalTypesSigned)
                    .ToList();

            var operators = Enumerable.Empty<T>()
            .Concat(numericalTypesSigned.SelectMany(t => numericalOperatorsSigned(t, boolType)))
            .Concat(numericalTypesUnsigned.SelectMany(t => numericalOperatorsUnsigned(t, boolType)))
            .Concat(booleanOperators(boolType));

            if (stringConcatOperators != null)
            {
                operators = operators.Concat(allTypesExceptString.SelectMany(t => stringConcatOperators(stringType, t)));
            }

            if(stringOperators != null)
            {
                operators = operators.Concat(stringOperators(stringType, boolType));
            }

            return operators.ToArray();
        }
    }
}