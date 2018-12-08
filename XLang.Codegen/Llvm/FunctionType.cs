using System.Linq;
using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class FunctionType
    {
        internal LLVMTypeRef TypeRef { get; }

        public FunctionType(XType returnType, params XType[] paramTypes)
            : this(returnType, paramTypes, false)
        {

        }

        public FunctionType(XType returnType, XType[] paramTypes = null, bool isVarArg = false)
        {
            if (paramTypes == null)
                paramTypes = new XType[0];
            TypeRef = LLVM.FunctionType(returnType.TypeRef, paramTypes.Select(pt => pt.TypeRef).ToArray(),
                isVarArg);
        }
    }
}