using System;
using System.Runtime.InteropServices;

namespace Repl.CodeAnalysis
{
    public static class Functions
    {
        public static void putc()
        {
            Console.WriteLine("Hello world");
        }

        [DllImport("kernel32.dll")]
        public static extern int GetStdHandle(int x);

        [DllImport("kernel32.dll")]
        public static extern bool WriteFile(long handle, string text, int len, long p, long p2);
    }
}