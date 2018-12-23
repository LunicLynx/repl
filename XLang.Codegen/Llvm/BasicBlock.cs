using System;
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

        public static BasicBlock Insert(BasicBlock before, string name = "")
        {
            var basicBlockRef = LLVM.InsertBasicBlock(before.BasicBlockRef, name);
            return new BasicBlock(basicBlockRef);
        }

        public void Print(TextWriter writer)
        {
            var value = new Value(LLVM.BasicBlockAsValue(BasicBlockRef));
            value.Print(writer);
        }

        public Value GetParent()
        {
            return new Value(LLVM.GetBasicBlockParent(BasicBlockRef));
        }

        public Value AsValue()
        {
            return new Value(LLVM.BasicBlockAsValue(BasicBlockRef));
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

        public void Delete()
        {
            LLVM.DeleteBasicBlock(BasicBlockRef);
        }

        public string GetName()
        {
            return LLVM.GetBasicBlockName(BasicBlockRef);
        }

        public Value GetTerminator()
        {
            var terminator = LLVM.GetBasicBlockTerminator(BasicBlockRef);
            if (terminator.Pointer == IntPtr.Zero) return null;
            return new Value(terminator);
        }

        public bool IsTerminated()
        {
            var terminator = LLVM.GetBasicBlockTerminator(BasicBlockRef);
            return terminator.Pointer != IntPtr.Zero;
        }

        public BasicBlock GetNext()
        {
            return new BasicBlock(LLVM.GetNextBasicBlock(BasicBlockRef));
        }

        public BasicBlock GetPrevious()
        {
            return new BasicBlock(LLVM.GetPreviousBasicBlock(BasicBlockRef));
        }

        public Value GetFirstInstruction()
        {
            return new Value(LLVM.GetFirstInstruction(BasicBlockRef));
        }

        public Value GetLastInstruction()
        {
            return new Value(LLVM.GetLastInstruction(BasicBlockRef));
        }
    }
}