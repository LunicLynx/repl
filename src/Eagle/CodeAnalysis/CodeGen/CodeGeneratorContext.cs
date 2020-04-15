using System.Collections.Generic;
using LLVMSharp.Interop;

namespace Eagle.CodeAnalysis.CodeGen
{
    public class CodeGeneratorContext
    {
        public CodeGeneratorContext(LLVMModuleRef module)
        {
            Module = module;
        }

        public Dictionary<Symbol, LLVMValueRef> Symbols { get; } = new Dictionary<Symbol, LLVMValueRef>();
        public Dictionary<TypeSymbol, LLVMTypeRef> Types { get; } = new Dictionary<TypeSymbol, LLVMTypeRef>();
        public Dictionary<FieldSymbol, int> FieldIndicies { get; } = new Dictionary<FieldSymbol, int>();
        public LLVMModuleRef Module { get; }
    }
}