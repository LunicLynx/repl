﻿using System.Runtime.InteropServices;
using LLVMSharp;

namespace XLang.Codegen.Llvm
{
    public static class Statics
    {
        public static string GetDefaultTargetTriple()
        {
            return Marshal.PtrToStringAnsi(LLVM.GetDefaultTargetTriple());
        }

        public static bool GetTargetFromTriple(string triple, out Target target, out string errorMessage)
        {
            target = null;
            var success = LLVM.GetTargetFromTriple(triple, out var t, out errorMessage) == Constants.True;
            if (!success) return false;
            target = new Target(t);
            return true;
        }
    }
}