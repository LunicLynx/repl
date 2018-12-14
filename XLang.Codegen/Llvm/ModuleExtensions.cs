namespace XLang.Codegen.Llvm
{
    public static class ModuleExtensions
    {
        public static bool TryEmitObj(this XModule mod, string filename, out string error)
        {
            var targetTriple = Statics.GetDefaultTargetTriple();
            mod.SetTarget(targetTriple);

            if (!Statics.GetTargetFromTriple(targetTriple, out var target, out error))
            {
                return false;
            }

            var targetMachine = new TargetMachine(target, targetTriple, "generic", "");
            var dataLayout = new TargetData(targetMachine);

            mod.SetDataLayout(dataLayout);

            if (!targetMachine.EmitToFile(mod, filename, out error))
            {
                return false;
            }
            return true;
        }
    }
}