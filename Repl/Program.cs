namespace Repl
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Console.SetWindowSize(22, 5);
            //Console.SetBufferSize(22, 5);
            var repl = new EagleRepl();
            //var repl = new RuleRepl();
            repl.Run();
        }
    }
}
