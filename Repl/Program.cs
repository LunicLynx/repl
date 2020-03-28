using System;
using System.Text;
using Repl.CodeAnalysis;

namespace Repl
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var stdin = Functions.GetStdHandle(-10);
            //byte[] buffer = new byte[1024];
            //Functions.ReadFile(stdin, buffer, (uint)buffer.Length, out var read, IntPtr.Zero);
            //var s = Encoding.ASCII.GetString(buffer, 0, (int)read).Trim();
            
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
