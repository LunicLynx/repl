using System.Linq;
using LLVMSharp.Interop;

namespace Eagle.CodeAnalysis.CodeGen
{
    public static class ModuleExtensions
    {
        public static bool TryEmitObj(this LLVMModuleRef mod, string filename, out string error)
        {
            error = "";

            var targetTriple = Statics.GetDefaultTargetTriple();
            mod.Target = targetTriple;

            var target = LLVMTargetRef.Targets.SingleOrDefault(t => t.Name == "x86-64");
            if (target == null)
            {
                return false;
            }

            var targetMachine = target.CreateTargetMachine(targetTriple, "generic", "",
                LLVMCodeGenOptLevel.LLVMCodeGenLevelNone,
                LLVMRelocMode.LLVMRelocDefault,
                LLVMCodeModel.LLVMCodeModelDefault);

            var dataLayout = targetMachine.CreateTargetDataLayout();
            mod.DataLayout = dataLayout;

            if (!targetMachine.TryEmitToFile(mod, filename, LLVMCodeGenFileType.LLVMObjectFile, out error))
            {
                return false;
            }
            return true;
        }
    }
}