﻿using System;
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

        public Value UDiv(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildUDiv(BuilderRef, left.ValueRef, right.ValueRef, name));
        }

        public Value ICmpEq(Value left, Value right, string name = "")
        {
            return new Value(LLVM.BuildICmp(BuilderRef, LLVMIntPredicate.LLVMIntEQ, left.ValueRef, right.ValueRef, name));
        }
    }
}