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

        /*
        public static XType UInt1 = new XType(LLVM.Int1Type());
        public static XType UInt8 = new XType(LLVM.Int8Type());
        public static XType UInt16 = new XType(LLVM.Int32Type());
        public static XType UInt32 = new XType(LLVM.Int32Type());
        public static XType UInt64 = new XType(LLVM.Int32Type());
        */

        public static XType Double = new XType(LLVM.DoubleType());
    }
}