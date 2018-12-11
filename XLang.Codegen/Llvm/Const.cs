using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class Const
    {
        public static Value Double(double number)
        {
            return new Value(LLVM.ConstReal(LLVM.DoubleType(), number));
        }

        public static Value Int1(bool set)
        {
            unchecked
            {
                return new Value(LLVM.ConstInt(LLVM.Int1Type(), (ulong)(set ? 1 : 0), Constants.True));
            }
        }

        public static Value UInt1(bool set)
        {
            unchecked
            {
                return new Value(LLVM.ConstInt(LLVM.Int1Type(), (ulong)(set ? 1 : 0), Constants.Falsy));
            }
        }

        public static Value Int8(sbyte number)
        {
            unchecked
            {
                return new Value(LLVM.ConstInt(LLVM.Int8Type(), (ulong)number, Constants.True));
            }
        }

        public static Value UInt8(byte number)
        {
            return new Value(LLVM.ConstInt(LLVM.Int8Type(), number, Constants.Falsy));
        }

        public static Value Int16(short number)
        {
            unchecked
            {
                return new Value(LLVM.ConstInt(LLVM.Int16Type(), (ulong)number, Constants.True));
            }
        }

        public static Value UInt16(ushort number)
        {
            return new Value(LLVM.ConstInt(LLVM.Int16Type(), number, Constants.Falsy));
        }

        public static Value Int32(int number)
        {
            unchecked
            {
                return new Value(LLVM.ConstInt(LLVM.Int32Type(), (ulong)number, Constants.True));
            }
        }

        public static Value UInt32(uint number)
        {
            return new Value(LLVM.ConstInt(LLVM.Int32Type(), number, Constants.Falsy));
        }

        public static Value Int64(long number)
        {
            unchecked
            {
                return new Value(LLVM.ConstInt(LLVM.Int64Type(), (ulong)number, Constants.True));
            }
        }

        public static Value UInt64(ulong number)
        {
            return new Value(LLVM.ConstInt(LLVM.Int64Type(), number, Constants.Falsy));
        }
    }
}