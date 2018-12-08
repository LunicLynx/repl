using System.Linq;
using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class Function : Value
    {
        internal Function(LLVMValueRef valueRef)
            : base(valueRef)
        {
        }

        public Function(XModule module, FunctionType type, string name)
            : base(LLVM.AddFunction(module.ModuleRef, name, type.TypeRef))
        {
        }

        public Builder OpenBody()
        {
            var basicBlock = AppendBasicBlock("entry");
            return basicBlock.Open();
        }

        public BasicBlock AppendBasicBlock(string name = "")
        {
            return BasicBlock.Append(this, name);
        }

        public Value GetParam(int index)
        {
            return new Value(LLVM.GetParam(ValueRef, (uint) index));
        }

        public int ArgSize()
        {
            return (int) LLVM.CountParams(ValueRef);
        }

        public Value[] Args()
        {
            return LLVM.GetParams(ValueRef).Select(p => new Value(p)).ToArray();
        }

        public bool Verify()
        {
            return LLVM.VerifyFunction(ValueRef, LLVMVerifierFailureAction.LLVMPrintMessageAction) ==
                   Constants.True;
        }

        public void Delete()
        {
            LLVM.DeleteFunction(ValueRef);
        }
    }
}