using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class TargetData
    {
        internal LLVMTargetDataRef TargetDataRef { get; }

        public TargetData(TargetMachine targetMachine)
        {
            TargetDataRef = LLVM.CreateTargetDataLayout(targetMachine.TargetMachineRef);
        }
    }
}