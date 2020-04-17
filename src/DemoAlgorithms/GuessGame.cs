using System;

namespace DemoAlgorithms
{
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
}