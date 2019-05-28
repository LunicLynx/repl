using System;
using System.Collections.Generic;
using System.Linq;
using Repl.CodeAnalysis.Lowering;
using XLang.Codegen;
using XLang.Codegen.Llvm;

namespace Repl.CodeAnalysis.CodeGen
{
    class NCompilation
    {
        public NCompilation Previous { get; }
        public Compilation Compilation { get; }

        public NCompilation(Compilation compilation)
        : this(null, compilation)
        {

        }

        private NCompilation(NCompilation previous, Compilation compilation)
        {
            Previous = previous;
            Compilation = compilation;
        }

        public NCompilation ContinueWith(Compilation compilation)
        {
            return new NCompilation(this, compilation);
        }
    }

    internal class Compiler
    {
        private BasicBlock _basicBlock;
        private readonly Function _function;
        private readonly CodeGeneratorContext _context;

        public Compiler()
        {
            Statics.InitializeX86Target();
            var module = new XModule("test");
            var functionType = new FunctionType(XType.Int32);
            _function = module.AddFunction(functionType, "__anon_expr");
            _basicBlock = _function.AppendBasicBlock();

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
            if (int64 != null) _context.Types[int64] = XType.Int64;
            if (int32 != null) _context.Types[int32] = XType.Int32;
            if (int16 != null) _context.Types[int16] = XType.Int16;
            if (int8 != null) _context.Types[int8] = XType.Int8;
            if (uint64 != null) _context.Types[uint64] = XType.Int64;
            if (uint32 != null) _context.Types[uint32] = XType.Int32;
            if (uint16 != null) _context.Types[uint16] = XType.Int16;
            if (uint8 != null) _context.Types[uint8] = XType.Int8;
            if (boolean != null) _context.Types[boolean] = XType.Int1;
            //if (@string != null) _types[@string] = XType.String;
            if (@void != null) _context.Types[@void] = XType.Int64;


            Value v;
            using (var builder = new Builder())
            {
                builder.PositionAtEnd(_basicBlock);

                var codeGenerator = new CodeGenerator(_context, builder);
                var value = codeGenerator.Generate(Lowerer.Lower(globalScope));
                _basicBlock = builder.GetInsertBlock();

                v = builder.Ret(value);

                //_function.ViewCfg();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                _context.Module.Print(Console.Out);
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

                v.RemoveFromParent();

            }
        }
    }
}