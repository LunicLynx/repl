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
    }
}