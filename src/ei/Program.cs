using System;
using Repl;

namespace Eagle
{
    class Program
    {
        static void Main(string[] args)
        {
            var repl = new EagleRepl();
            repl.Run();
        }
    }
}
