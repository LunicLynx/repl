using System;

namespace Eagle
{
    class Program
    {
        static void Main(string[] args)
        {
            var repl = new EagleRepl();
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
