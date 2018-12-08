using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class Const
    {
        public static Value Int64(long number)
        {
            return new Value(LLVM.ConstInt(LLVM.Int64Type(), (ulong) number, Constants.True));
        }

        public static Value Double(double number)
        {
            return new Value(LLVM.ConstReal(LLVM.DoubleType(), number));
        }

        public static Value Int32(int number)
        {
            return new Value(LLVM.ConstInt(LLVM.Int32Type(), (ulong)number, Constants.True));
        }
    }
}