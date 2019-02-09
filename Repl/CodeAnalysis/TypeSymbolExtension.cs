using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis
{
    public static class TypeSymbolExtension
    {
        private static readonly IReadOnlyDictionary<string, Type> TypeMap = new Dictionary<string, Type>
        {
            {"Void", typeof(void) },
            {"Int8", typeof(sbyte) },
            {"Int16", typeof(short) },
            {"Int32", typeof(int) },
            {"Int64", typeof(long) },
            {"UInt8", typeof(byte) },
            {"UInt16", typeof(ushort) },
            {"UInt32", typeof(uint) },
            {"UInt64", typeof(ulong) },
            {"Boolean", typeof(bool) },
            {"String", typeof(string) },
        };

        public static Type GetClrType(this TypeSymbol typeSymbol)
        {
            if (TypeMap.TryGetValue(typeSymbol.Name, out var type))
                return type;
            return typeof(object);
        }
    }
}