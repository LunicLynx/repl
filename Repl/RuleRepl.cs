using System.IO;

namespace Repl
{
    public class RuleRepl : Repl
    {
        public override void Run()
        {
            var path = "demo.r";
            if (File.Exists(path))
                Load(path);
            base.Run();
        }

        protected override bool IsCompleteSubmission(string text)
        {
            return true;
        }

        protected override void EvaluateSubmission(string text)
        {

        }
    }
}