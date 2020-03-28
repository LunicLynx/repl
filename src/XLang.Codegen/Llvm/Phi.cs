using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class Phi : Value
    {
        internal Phi(LLVMValueRef valueRef) : base(valueRef)
        {
            
        }

        public void AddIncoming(Value incomingValue, BasicBlock incomingBlock)
        {
            LLVM.AddIncoming(
                ValueRef,
                new[] { incomingValue.ValueRef },
                new[] { incomingBlock.BasicBlockRef },
                1);
        }
    }
}