using System;

namespace DemoAlgorithms
{
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