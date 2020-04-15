using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

    public static class Cl
    {
        public static void InvokeCl(string filename)
        {
            Console.WriteLine("Compiling...");

            var toolset = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\VC\Tools\MSVC\14.21.27619";
            var windowskitsInclude = @"C:\Program Files (x86)\Windows Kits\10\include\10.0.17763.0";
            var windowskitsLibs = @"C:\Program Files (x86)\Windows Kits\10\lib\10.0.17763.0";
            var clExe = Path.Combine(toolset, @"bin\Hostx64\x64\cl.exe");

            var includes = new[]
            {
                Path.Combine(toolset, "include"),
                Path.Combine(windowskitsInclude, "ucrt"),
                //Path.Combine(windowskits, "shared"),
                //Path.Combine(windowskits, "um"),
                //Path.Combine(windowskits, "winrt"),
                //Path.Combine(windowskits, "cppwinrt")
            };
            var include = string.Join(";", includes);

            var libs = new[]
            {
                Path.Combine(toolset, @"lib\x64"),
                Path.Combine(windowskitsLibs, @"ucrt\x64"),
                Path.Combine(windowskitsLibs, @"um\x64"),
            };

            var lib = string.Join(";", libs);

            var startInfo = new ProcessStartInfo();
            startInfo.EnvironmentVariables.Add("INCLUDE", include);
            startInfo.EnvironmentVariables.Add("LIB", lib);
            startInfo.FileName = clExe;
            startInfo.Arguments = "/nologo /EHsc main.cpp " + filename;
            var process = Process.Start(startInfo);
            process.WaitForExit();
        }

        public static void InvokeMain()
        {
            Console.WriteLine("Executing...");
            var fileName = "main.exe";
            if (File.Exists(fileName))
            {
                var process = Process.Start(fileName);
                process.WaitForExit();
                Console.WriteLine("Exit Code: " + process.ExitCode);
            }
        }
    }

    public static class ModuleExtensions
    {
        public static bool TryEmitObj(this LLVMModuleRef mod, string filename, out string error)
        {
            error = "";

            var targetTriple = Statics.GetDefaultTargetTriple();
            mod.Target = targetTriple;

            var target = LLVMTargetRef.Targets.SingleOrDefault(t => t.Name == targetTriple);
            if (target == null)
            {
                return false;
            }

            var targetMachine = target.CreateTargetMachine(targetTriple, "generic", "",
                LLVMCodeGenOptLevel.LLVMCodeGenLevelNone,
                LLVMRelocMode.LLVMRelocDefault,
                LLVMCodeModel.LLVMCodeModelDefault);

            var dataLayout = targetMachine.CreateTargetDataLayout();
            mod.DataLayout = dataLayout;

            if (!targetMachine.TryEmitToFile(mod, filename, LLVMCodeGenFileType.LLVMAssemblyFile, out error))
            {
                return false;
            }
            return true;
        }
    }

    public class Compiler
    {
        private LLVMBasicBlockRef _basicBlock;
        private readonly LLVMValueRef _function;
        private readonly CodeGeneratorContext _context;

        public Compiler()
        {

            Statics.InitializeX86Target();
            var module = LLVMModuleRef.CreateWithName("test");

            var functionType = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, new LLVMTypeRef[0]);
            _function = module.AddFunction("__anon_expr", functionType);
            _basicBlock = _function.AppendBasicBlock("entry");

            _context = new CodeGeneratorContext(module);
        }

        public void CompileAndRun(Compilation compilation, Dictionary<Symbol, object> variables)
        {
            var globalScope = compilation.GlobalScope;

            var ts = globalScope.Symbols.OfType<TypeSymbol>().ToList();
            var int64 = ts.FirstOrDefault(t => t.Name == "Int64");
            var int32 = ts.FirstOrDefault(t => t.Name == "Int32");
            var int16 = ts.FirstOrDefault(t => t.Name == "Int16");
            var int8 = ts.FirstOrDefault(t => t.Name == "Int8");
            var uint64 = ts.FirstOrDefault(t => t.Name == "UInt64");
            var uint32 = ts.FirstOrDefault(t => t.Name == "UInt32");
            var uint16 = ts.FirstOrDefault(t => t.Name == "UInt16");
            var uint8 = ts.FirstOrDefault(t => t.Name == "UInt8");
            var boolean = ts.FirstOrDefault(t => t.Name == "Boolean");
            //var @string = ts.FirstOrDefault(t => t.Name == "String");
            var @void = ts.FirstOrDefault(t => t.Name == "Void");
            if (int64 != null) _context.Types[int64] = LLVMTypeRef.Int64;
            if (int32 != null) _context.Types[int32] = LLVMTypeRef.Int32;
            if (int16 != null) _context.Types[int16] = LLVMTypeRef.Int16;
            if (int8 != null) _context.Types[int8] = LLVMTypeRef.Int8;
            if (uint64 != null) _context.Types[uint64] = LLVMTypeRef.Int64;
            if (uint32 != null) _context.Types[uint32] = LLVMTypeRef.Int32;
            if (uint16 != null) _context.Types[uint16] = LLVMTypeRef.Int16;
            if (uint8 != null) _context.Types[uint8] = LLVMTypeRef.Int8;
            if (boolean != null) _context.Types[boolean] = LLVMTypeRef.Int1;
            //if (@string != null) _types[@string] = XType.String;
            if (@void != null) _context.Types[@void] = LLVMTypeRef.Int64;


            LLVMValueRef v;
            using (var builder = _context.Module.Context.CreateBuilder())
            {
                builder.PositionAtEnd(_basicBlock);

                var codeGenerator = new CodeGenerator(_context, builder);
                LLVMValueRef value = null;//codeGenerator.Generate(Lowerer.Lower(globalScope.Statements[0]));
                _basicBlock = builder.InsertBlock;

                v = builder.BuildRet(value);

                _function.ViewFunctionCFG();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                var s = _context.Module.PrintToString();
                Console.WriteLine(s);
                Console.WriteLine();
                Console.ResetColor();

                var objFilename = "entry.obj";
                if (_context.Module.TryEmitObj(objFilename, out var error))
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Cl.InvokeCl(objFilename);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Cl.InvokeMain();
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(error);
                    Console.ResetColor();
                }

                _basicBlock.RemoveFromParent();
            }
        }
    }
}