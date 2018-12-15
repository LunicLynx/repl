using System.IO;
using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class BasicBlock
    {
        internal LLVMBasicBlockRef BasicBlockRef { get; }

        internal BasicBlock(LLVMBasicBlockRef basicBlockRef)
        {
            BasicBlockRef = basicBlockRef;
        }

        public Builder Open()
        {
            var builder = new Builder();
            builder.PositionAtEnd(this);
            return builder;
        }

        public static BasicBlock Append(Function function, string name = "")
        {
            var basicBlockRef = LLVM.AppendBasicBlock(function.ValueRef, name);
            return new BasicBlock(basicBlockRef);
        }

        public void Print(TextWriter writer)
        {
            var value = new Value(LLVM.BasicBlockAsValue(BasicBlockRef));
            value.Print(writer);
        }

        public Function GetParent()
        {
            return new Function(LLVM.GetBasicBlockParent(BasicBlockRef));
        }

        public void RemoveFromParent()
        {
            LLVM.RemoveBasicBlockFromParent(BasicBlockRef);
        }

        public void MoveAfter(BasicBlock pos)
        {
            LLVM.MoveBasicBlockAfter(BasicBlockRef, pos.BasicBlockRef);
        }

        public void MoveBefore(BasicBlock pos)
        {
            LLVM.MoveBasicBlockBefore(BasicBlockRef, pos.BasicBlockRef);
        }
    }
}