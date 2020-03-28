using System.Collections.Generic;
using XLang.Codegen.Llvm;

namespace Repl.CodeAnalysis.CodeGen
{
    public class CodeGeneratorContext
    {
        public CodeGeneratorContext(XModule module)
        {
            Module = module;
        }

        public Dictionary<Symbol, Value> Symbols { get; } = new Dictionary<Symbol, Value>();
        public Dictionary<TypeSymbol, XType> Types { get; } = new Dictionary<TypeSymbol, XType>();
        public Dictionary<FieldSymbol, int> FieldIndicies { get; } = new Dictionary<FieldSymbol, int>();
        public XModule Module { get; }
    }
}