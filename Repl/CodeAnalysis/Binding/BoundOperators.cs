using System;
using System.Collections.Generic;
using System.Linq;

namespace Repl.CodeAnalysis.Binding
{
    public static class BoundOperators
    {
        public static T[] GetOperators<T>(Func<TypeSymbol, TypeSymbol, IEnumerable<T>> numericalOperators, Func<TypeSymbol, IEnumerable<T>> booleanOperators)
        {
            var boolType = TypeSymbol.Bool;
            var numericalTypes = new[]
            {
                TypeSymbol.I8,
                TypeSymbol.I16,
                TypeSymbol.I32,
                TypeSymbol.I64,
                TypeSymbol.U8,
                TypeSymbol.U16,
                TypeSymbol.U32,
                TypeSymbol.U64,
                TypeSymbol.Int,
                TypeSymbol.Uint,
            };

            return numericalTypes
                .SelectMany(t => numericalOperators(t, boolType))
                .Concat(booleanOperators(boolType))
                .ToArray();
        }
    }
}