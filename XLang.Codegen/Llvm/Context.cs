using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class Context
    {
        internal LLVMContextRef ContextRef { get; }

        internal Context(LLVMContextRef contextRef)
        {
            ContextRef = contextRef;
        }

        public static Context Global = new Context(LLVM.GetGlobalContext());

        public Builder Builder()
        {
            return new Builder(this);
        }

        public XModule CreateModule(string name)
        {
            return new XModule(name, this);
        }

        public XType Double => new XType(LLVM.DoubleTypeInContext(ContextRef));

    }
}