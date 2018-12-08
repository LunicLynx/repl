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

        public static XType Int32 = new XType(LLVM.Int32Type());

        public static XType Double = new XType(LLVM.DoubleType());
    }
}