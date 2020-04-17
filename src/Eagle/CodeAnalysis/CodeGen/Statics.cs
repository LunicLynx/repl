using System.Collections.Generic;
using System.Collections.Immutable;
using LLVMSharp.Interop;

namespace Eagle.CodeAnalysis.CodeGen
{
    public static class Statics
    {
        public static string GetDefaultTargetTriple()
        {
            return LLVMTargetRef.DefaultTriple;
        }

        public static void InitializeX86Target()
        {
            LLVM.InitializeX86TargetMC();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86AsmParser();
            LLVM.InitializeX86AsmPrinter();
        }
    }

    //public class Compiler
    //{
    //    private LLVMBasicBlockRef _basicBlock;
    //    private readonly LLVMValueRef _function;
    //    private readonly CodeGeneratorContext _context;

    //    public Compiler()
    //    {

    //        Statics.InitializeX86Target();
    //        var module = LLVMModuleRef.CreateWithName("test");

    //        var functionType = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, new LLVMTypeRef[0]);
    //        _function = module.AddFunction("__anon_expr", functionType);
    //        _basicBlock = _function.AppendBasicBlock("entry");

    //        _context = new CodeGeneratorContext(module);
    //    }

    //    public void CompileAndRun(Compilation compilation, Dictionary<Symbol, object> variables)
    //    {
    //        var globalScope = compilation.GlobalScope;

    //        var ts = globalScope.Symbols.OfType<TypeSymbol>().ToList();
    //        var int64 = ts.FirstOrDefault(t => t.Name == "Int64");
    //        var int32 = ts.FirstOrDefault(t => t.Name == "Int32");
    //        var int16 = ts.FirstOrDefault(t => t.Name == "Int16");
    //        var int8 = ts.FirstOrDefault(t => t.Name == "Int8");
    //        var uint64 = ts.FirstOrDefault(t => t.Name == "UInt64");
    //        var uint32 = ts.FirstOrDefault(t => t.Name == "UInt32");
    //        var uint16 = ts.FirstOrDefault(t => t.Name == "UInt16");
    //        var uint8 = ts.FirstOrDefault(t => t.Name == "UInt8");
    //        var boolean = ts.FirstOrDefault(t => t.Name == "Boolean");
    //        //var @string = ts.FirstOrDefault(t => t.Name == "String");
    //        var @void = ts.FirstOrDefault(t => t.Name == "Void");
    //        if (int64 != null) _context.Types[int64] = LLVMTypeRef.Int64;
    //        if (int32 != null) _context.Types[int32] = LLVMTypeRef.Int32;
    //        if (int16 != null) _context.Types[int16] = LLVMTypeRef.Int16;
    //        if (int8 != null) _context.Types[int8] = LLVMTypeRef.Int8;
    //        if (uint64 != null) _context.Types[uint64] = LLVMTypeRef.Int64;
    //        if (uint32 != null) _context.Types[uint32] = LLVMTypeRef.Int32;
    //        if (uint16 != null) _context.Types[uint16] = LLVMTypeRef.Int16;
    //        if (uint8 != null) _context.Types[uint8] = LLVMTypeRef.Int8;
    //        if (boolean != null) _context.Types[boolean] = LLVMTypeRef.Int1;
    //        //if (@string != null) _types[@string] = XType.String;
    //        if (@void != null) _context.Types[@void] = LLVMTypeRef.Int64;


    //        LLVMValueRef v;
    //        using (var builder = _context.Module.Context.CreateBuilder())
    //        {
    //            builder.PositionAtEnd(_basicBlock);

    //            var codeGenerator = new CodeGenerator(_context, builder);
    //            LLVMValueRef value = null;//codeGenerator.Generate(Lowerer.Lower(globalScope.Statements[0]));
    //            _basicBlock = builder.InsertBlock;

    //            v = builder.BuildRet(value);

    //            _function.ViewFunctionCFG();

    //            Console.ForegroundColor = ConsoleColor.DarkYellow;
    //            var s = _context.Module.PrintToString();
    //            Console.WriteLine(s);
    //            Console.WriteLine();
    //            Console.ResetColor();

    //            var objFilename = "entry.obj";
    //            if (_context.Module.TryEmitObj(objFilename, out var error))
    //            {
    //                Console.ForegroundColor = ConsoleColor.DarkCyan;
    //                Cl.InvokeCl(objFilename);
    //                Console.ForegroundColor = ConsoleColor.Cyan;
    //                Cl.InvokeMain();
    //                Console.ResetColor();
    //            }
    //            else
    //            {
    //                Console.ForegroundColor = ConsoleColor.Red;
    //                Console.WriteLine(error);
    //                Console.ResetColor();
    //            }

    //            _basicBlock.RemoveFromParent();
    //        }
    //    }
    //}
}