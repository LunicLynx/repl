using System.IO;
using System.Runtime.InteropServices;
using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class Value
    {
        internal LLVMValueRef ValueRef { get; }

        internal Value(LLVMValueRef valueRef)
        {
            ValueRef = valueRef;
        }

        public string GetName()
        {
            return LLVM.GetValueName(ValueRef);
        }

        public void SetName(string name)
        {
            LLVM.SetValueName(ValueRef, name);
        }

        public void Print(TextWriter writer)
        {
            var s = Marshal.PtrToStringAnsi(LLVM.PrintValueToString(ValueRef));
            writer.Write(s);
        }

        public void EraseFromParent()
        {
            // TODO should only be on Instr ?
            LLVM.InstructionEraseFromParent(ValueRef);
        }

        public void RemoveFromParent()
        {
            // TODO should only be on Instr ?
            LLVM.InstructionEraseFromParent(ValueRef);
        }

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

        public Value IsABasicBlock()
        {
            return new Value(LLVM.IsABasicBlock(ValueRef));
        }

        public bool IsBasicBlock()
        {
            return LLVM.ValueIsBasicBlock(ValueRef);
        }

        public BasicBlock AsBasicBlock()
        {
            return new BasicBlock(LLVM.ValueAsBasicBlock(ValueRef));
        }

        public Function AsFunction()
        {
            return new Function(ValueRef);
        }

        public Value IsAFunction()
        {
            return new Value(LLVM.IsAFunction(ValueRef));
        }

        public BasicBlock GetParent()
        {
            return new BasicBlock(LLVM.GetInstructionParent(ValueRef));
        }
        public Phi AsPhi()
        {
            return new Phi(ValueRef);
        }
    }
}