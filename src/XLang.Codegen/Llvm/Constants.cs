using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    internal static class Constants
    {
        public static LLVMBool True = new LLVMBool(0);
        public static LLVMBool Falsy = new LLVMBool(-1);
    }
}