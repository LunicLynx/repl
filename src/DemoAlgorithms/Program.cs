using System;
using System.Linq.Expressions;

namespace DemoAlgorithms
{
    class Program
    {
        static void Main(string[] args)
        {

            var s1 = "asd";
            var s2 = "def";
            var s3 = s1 + s2;
            var s4 = string.Concat(s1, s2);

            Expression<Func<string, string, string>> expr = (a, b) => a + b;
            Expression<Func<string, int, string>> expr2 = (a, b) => a + b;
            Expression<Func<int, string, string>> expr3 = (a, b) => a + b;

            
            new GuessGame2().Run();
        }
    }
}
