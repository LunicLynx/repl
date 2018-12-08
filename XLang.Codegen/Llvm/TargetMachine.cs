using System.Runtime.InteropServices;
using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class TargetMachine
    {
        internal LLVMTargetMachineRef TargetMachineRef { get; }

        public TargetMachine(Target target, string triple, string cpu, string features)
        {
            TargetMachineRef = LLVM.CreateTargetMachine(target.TargetRef, triple, cpu, features,
                LLVMCodeGenOptLevel.LLVMCodeGenLevelDefault,
                LLVMRelocMode.LLVMRelocDefault, LLVMCodeModel.LLVMCodeModelDefault);
        }

        public bool EmitToFile(XModule module, string fileName, out string errorMessage)
        {
            var filename = Marshal.StringToHGlobalAnsi(fileName);
            return LLVM.TargetMachineEmitToFile(TargetMachineRef, module.ModuleRef, filename,
                       LLVMCodeGenFileType.LLVMObjectFile, out errorMessage) == Constants.True;
        }
    }
}