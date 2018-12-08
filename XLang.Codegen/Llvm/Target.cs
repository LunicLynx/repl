using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class Target
    {
        internal LLVMTargetRef TargetRef { get; }

        internal Target(LLVMTargetRef targetRef)
        {
            TargetRef = targetRef;
        }
    }
}