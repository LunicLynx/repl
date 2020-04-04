using System;
using System.Collections.Generic;

namespace Repl.CodeAnalysis
{
    public static class TypeSymbolExtensions
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

            {"UInt", typeof(ulong) },
            {"Int", typeof(long) },
        };

        public static Type GetClrType(this TypeSymbol typeSymbol) =>
            TypeMap.TryGetValue(typeSymbol.Name, out var type)
                ? type
                : typeof(object);

        public static bool IsInteger(this TypeSymbol typeSymbol) =>
            typeSymbol == TypeSymbol.I8 ||
            typeSymbol == TypeSymbol.I16 ||
            typeSymbol == TypeSymbol.I32 ||
            typeSymbol == TypeSymbol.I64 ||
            typeSymbol == TypeSymbol.U8 ||
            typeSymbol == TypeSymbol.U16 ||
            typeSymbol == TypeSymbol.U32 ||
            typeSymbol == TypeSymbol.U64;

        public static int GetBits(this TypeSymbol typeSymbol)
        {
            if (typeSymbol == TypeSymbol.I8) return 8;
            if (typeSymbol == TypeSymbol.I16) return 16;
            if (typeSymbol == TypeSymbol.I32) return 32;
            if (typeSymbol == TypeSymbol.I64) return 64;
            if (typeSymbol == TypeSymbol.U8) return 8;
            if (typeSymbol == TypeSymbol.U16) return 16;
            if (typeSymbol == TypeSymbol.U32) return 32;
            if (typeSymbol == TypeSymbol.U64) return 64;
            throw new Exception("Not an integer");
        }

        public static bool IsSigned(this TypeSymbol typeSymbol)
        {
            if (typeSymbol == TypeSymbol.I8) return true;
            if (typeSymbol == TypeSymbol.I16) return true;
            if (typeSymbol == TypeSymbol.I32) return true;
            if (typeSymbol == TypeSymbol.I64) return true;
            if (typeSymbol == TypeSymbol.U8) return false;
            if (typeSymbol == TypeSymbol.U16) return false;
            if (typeSymbol == TypeSymbol.U32) return false;
            if (typeSymbol == TypeSymbol.U64) return false;
            throw new Exception("Not an integer");
        }

        public static bool IsFloat(this TypeSymbol typeSymbol)
        {
            return false;
        }

        //public static Type GetClrType(TypeSymbol typeSymbol)
        //{
        //    if (_clrTypes.TryGetValue(typeSymbol, out var type))
        //        return type;
        //    return typeof(object);
        //}

        public static bool IsClrAssignableFrom(this TypeSymbol to, TypeSymbol from)
        {
            var fromType = GetClrType(from);
            var toType = GetClrType(to);
            return toType.IsAssignableFrom(fromType);
        }

        public static bool IsAssignableFrom(this TypeSymbol to, TypeSymbol from)
        {
            if (from == to) return true;
            if (to.IsClrAssignableFrom(from)) return true;

            try
            {
                var toProperties = GetIntegerTypeProperties(to);
                var fromProperties = GetIntegerTypeProperties(from);
                return toProperties.signed == fromProperties.signed && toProperties.size >= fromProperties.size
                       || toProperties.signed && toProperties.size > fromProperties.size;
            }
            catch
            {
                return false;
            }
        }

        public static (bool signed, int size) GetIntegerTypeProperties(this TypeSymbol type)
        {
            return (type.IsSigned(), type.GetBits() / 8);
            //if (type == TypeSymbol.I64) return (true, 8);
            //if (type == TypeSymbol.I32) return (true, 4);
            //if (type == TypeSymbol.I16) return (true, 2);
            //if (type == TypeSymbol.I8) return (true, 1);
            //if (type == TypeSymbol.U64) return (false, 8);
            //if (type == TypeSymbol.U32) return (false, 4);
            //if (type == TypeSymbol.U16) return (false, 2);
            //if (type == TypeSymbol.U8) return (false, 1);
            //throw new Exception("Non integer types are not supported");
        }
    }
}