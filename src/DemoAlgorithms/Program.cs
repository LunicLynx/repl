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

    class GuessGame
    {
        public void Run()
        {
            var number = new Random().Next(0, 100);
            Console.WriteLine("Enter guess:");

            int guess;
            do
            {
                int.TryParse(Console.ReadLine(), out guess);
                if (guess > number)
                    Console.WriteLine("Your number was greater!");
                if (guess < number)
                    Console.WriteLine("Your number was less!");
            } while (guess != number);

            Console.WriteLine("You guessed the right number");
        }
    }

    class GuessGame2
    {
        public void Run()
        {
            var min = 0;
            var max = 100;
            Console.WriteLine($"Think of a number between {min} and {max}.");

            while (min != max)
            {
                var guess = (min + max) / 2;

                Console.WriteLine($"Is your number greater than {guess}?");

                bool greater = false;
                while (true)
                {
                    Console.WriteLine("Enter y, n:");
                    var a = Console.ReadLine();
                    if (a == "y")
                    {
                        greater = true;
                        break;
                    }
                    else if (a == "n")
                    {
                        break;
                    }
                }

                if (greater)
                {
                    min = guess + 1;
                }
                else
                {
                    max = guess;
                }
            }

            Console.WriteLine($"Your number is {min}");
        }
    }
}
