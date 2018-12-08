using System;

namespace XLang.Codegen.Llvm
{
    public static class ModuleExtensions
    {
        public static void EmitObj(this XModule mod, string filename)
        {
            var targetTriple = Statics.GetDefaultTargetTriple();
            mod.SetTarget(targetTriple);

            if (!Statics.GetTargetFromTriple(targetTriple, out var target, out var error))
            {
                Console.WriteLine($"Error getting target: {error}");
            }

            var targetMachine = new TargetMachine(target, targetTriple, "generic", "");
            var dataLayout = new TargetData(targetMachine);

            mod.SetDataLayout(dataLayout);

            if (!targetMachine.EmitToFile(mod, filename, out error))
            {
                Console.WriteLine($"Error emitting: {error}");
            }
        }
    }
}