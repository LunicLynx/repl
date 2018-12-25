using System;
using System.Linq;
using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public class Builder : IDisposable
    {
        private bool _disposed;
        internal LLVMBuilderRef BuilderRef { get; }

        public Builder()
        {
            BuilderRef = LLVM.CreateBuilder();
        }

        public Builder(Context context)
        {
            BuilderRef = LLVM.CreateBuilderInContext(context.ContextRef);
        }

        public void PositionAtEnd(BasicBlock basicBlock)
        {
            ThrowIfDisposed();
            LLVM.PositionBuilderAtEnd(BuilderRef, basicBlock.BasicBlockRef);
        }

        public void Dispose()
        {
            ThrowIfDisposed();
            LLVM.DisposeBuilder(BuilderRef);
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Builder));
        }

        public Value Add(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildAdd(BuilderRef, left.ValueRef, right.ValueRef, name));
        }

        public Value Mul(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildMul(BuilderRef, left.ValueRef, right.ValueRef, name));
        }

        public Value Ret(Value value)
        {
            return new Value(LLVM.BuildRet(BuilderRef, value.ValueRef));
        }

        public Value GlobalString(string str, string name = "")
        {
            return new Value(LLVM.BuildGlobalString(BuilderRef, str, name));
        }

        public Value GEP(Value pointer, Value[] indices, string name = "")
        {
            return new Value(LLVM.BuildGEP(BuilderRef, pointer.ValueRef,
                indices.Select(i => i.ValueRef).ToArray(), name));
        }

        public Value Call(Value func, Value[] args, string name = "")
        {
            return new Value(LLVM.BuildCall(BuilderRef, func.ValueRef, args.Select(a => a.ValueRef).ToArray(),
                ""));
        }

        public Value RetVoid()
        {
            return new Value(LLVM.BuildRetVoid(BuilderRef));
        }

        public Value FAdd(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildFAdd(BuilderRef, left.ValueRef, right.ValueRef, name));
        }

        public Value FSub(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildFSub(BuilderRef, left.ValueRef, right.ValueRef, name));
        }

        public Value FMul(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildFMul(BuilderRef, left.ValueRef, right.ValueRef, name));
        }

        public Value FCmpUlt(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildFCmp(BuilderRef, LLVMRealPredicate.LLVMRealULT, left.ValueRef,
                right.ValueRef, name));
        }

        public Value UiToFp(Value val, XType destTy, string name = "")
        {
            return new Value(LLVM.BuildUIToFP(BuilderRef, val.ValueRef, destTy.TypeRef, name));
        }

        public Value Alloca(XType type, string name = "")
        {
            return new Value(LLVM.BuildAlloca(BuilderRef, type.TypeRef, name));
        }

        public Value Store(Value val, Value ptr)
        {

            return new Value(LLVM.BuildStore(BuilderRef, val.ValueRef, ptr.ValueRef));
        }

        public Value Load(Value ptr, string name = "")
        {
            return new Value(LLVM.BuildLoad(BuilderRef, ptr.ValueRef, name));
        }

        public Value Sub(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildSub(BuilderRef, left.ValueRef, right.ValueRef, name));
        }
        public Value SDiv(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildSDiv(BuilderRef, left.ValueRef, right.ValueRef, name));
        }

        public Value SRem(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildSRem(BuilderRef, left.ValueRef, right.ValueRef, name));
        }

        public Value URem(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildURem(BuilderRef, left.ValueRef, right.ValueRef, name));
        }

        public Value UDiv(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildUDiv(BuilderRef, left.ValueRef, right.ValueRef, name));
        }

        public Value ICmpEq(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildICmp(BuilderRef, LLVMIntPredicate.LLVMIntEQ, left.ValueRef, right.ValueRef, name));
        }

        public Value ICmpNe(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildICmp(BuilderRef, LLVMIntPredicate.LLVMIntNE, left.ValueRef, right.ValueRef, name));
        }

        public Value And(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildAnd(BuilderRef, left.ValueRef, right.ValueRef, name));
        }

        public Value Or(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildOr(BuilderRef, left.ValueRef, right.ValueRef, name));
        }

        public Value Neg(Value value, string name = "")
        {
            return new Value(LLVM.BuildNeg(BuilderRef, value.ValueRef, name));
        }

        public Value Not(Value value, string name = "")
        {
            return new Value(LLVM.BuildNot(BuilderRef, value.ValueRef, name));
        }

        public Value ICmpSlt(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildICmp(BuilderRef, LLVMIntPredicate.LLVMIntSLT, left.ValueRef, right.ValueRef,
                name));
        }

        public Value ICmpSle(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildICmp(BuilderRef, LLVMIntPredicate.LLVMIntSLE, left.ValueRef, right.ValueRef,
                name));
        }

        public Value ICmpSgt(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildICmp(BuilderRef, LLVMIntPredicate.LLVMIntSGT, left.ValueRef, right.ValueRef,
                name));
        }

        public Value ICmpSge(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildICmp(BuilderRef, LLVMIntPredicate.LLVMIntSGE, left.ValueRef, right.ValueRef,
                name));
        }

        public Value CondBr(Value cond, BasicBlock then, BasicBlock @else)
        {
            return new Value(LLVM.BuildCondBr(BuilderRef, cond.ValueRef, then.BasicBlockRef, @else.BasicBlockRef));
        }

        public BasicBlock GetInsertBlock()
        {
            var basicBlockRef = LLVM.GetInsertBlock(BuilderRef);
            return new BasicBlock(basicBlockRef);
        }

        public Value Br(BasicBlock dest)
        {
            return new Value(LLVM.BuildBr(BuilderRef, dest.BasicBlockRef));
        }

        public Phi Phi(XType type, string name = "")
        {
            return new Phi(LLVM.BuildPhi(BuilderRef, type.TypeRef, name));
        }

        public Value Xor(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildXor(BuilderRef, left.ValueRef, right.ValueRef, name));
        }

        
    }
}