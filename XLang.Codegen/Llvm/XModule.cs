using System.IO;
using System.Runtime.InteropServices;
using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class XModule
    {
        internal LLVMModuleRef ModuleRef { get; }

        public XModule(string name)
        {
            ModuleRef = LLVM.ModuleCreateWithName(name);
        }

        public XModule(string name, Context context)
        {
            LLVM.ModuleCreateWithNameInContext(name, context.ContextRef);
        }

        public Function AddFunction(FunctionType type, string name)
        {
            return new Function(this, type, name);
        }

        public Function GetFunction(string name)
        {
            return new Function(LLVM.GetNamedFunction(ModuleRef, name));
        }

        public bool Verify(LLVMVerifierFailureAction action, out string outMessage)
        {
            return LLVM.VerifyModule(ModuleRef, action, out outMessage) == Constants.True;
        }

        public void Dump()
        {
            LLVM.DumpModule(ModuleRef);
        }

        public void SetTarget(string triple)
        {
            LLVM.SetTarget(ModuleRef, triple);
        }

        public void SetDataLayout(TargetData dataLayout)
        {
            LLVM.SetModuleDataLayout(ModuleRef, dataLayout.TargetDataRef);
        }

        public void Print(TextWriter writer)
        {
            var s = Marshal.PtrToStringAnsi(LLVM.PrintModuleToString(ModuleRef));
            writer.Write(s);
        }
    }
}