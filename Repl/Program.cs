using System;

namespace Repl
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //PrintAllColors();
            //Console.SetWindowSize(22, 5);
            //Console.SetBufferSize(22, 5);
            var repl = new EagleRepl();
            //var repl = new RuleRepl();
            repl.Run();
        }

        private static void PrintAllColors()
        {
            var colors = Enum.GetValues(typeof(ConsoleColor));

            for (int i = 0; i < colors.Length; i++)
            {
                for (int j = 0; j < colors.Length; j++)
                {
                    var background = (ConsoleColor)colors.GetValue(i);
                    var foreground = (ConsoleColor)colors.GetValue(j);
                    Console.ForegroundColor = foreground;
                    Console.BackgroundColor = background;
                    Console.Write("#");
                }
                Console.WriteLine();
            }

            Console.ResetColor();
        }
    }
}
