﻿using System.Collections.Generic;
using System.Linq;
using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class XType
    {
        public LLVMTypeRef TypeRef { get; }

        internal XType(LLVMTypeRef typeRef)
        {
            TypeRef = typeRef;
        }

        public static XType Void = new XType(LLVM.VoidType());

        public static XType Int1 = new XType(LLVM.Int1Type());
        public static XType Int8 = new XType(LLVM.Int8Type());
        public static XType Int16 = new XType(LLVM.Int16Type());
        public static XType Int32 = new XType(LLVM.Int32Type());
        public static XType Int64 = new XType(LLVM.Int64Type());


        //public static XType Ptr = new XType(LLVM.P);

        public static XType Double = new XType(LLVM.DoubleType());

        public static XType Struct(IEnumerable<XType> elementTypes)
        {
            var types = elementTypes.Select(m => m.TypeRef).ToArray();
            return new XType(LLVM.StructType(types, false));
        }

        public static XType StructNamed(Context ctx, string name, IEnumerable<XType> elementTypes)
        {
            var types = elementTypes.Select(m => m.TypeRef).ToArray();
            var s = LLVM.StructCreateNamed(ctx.ContextRef, name);
            LLVM.StructSetBody(s, types, false);
            return new XType(s);
        }

        public static XType Pointer(XType type)
        {
            return new XType(LLVM.PointerType(type.TypeRef, 0));
        }
    }
}