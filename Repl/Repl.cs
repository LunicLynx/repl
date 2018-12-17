using System;
using System.Text;

namespace Repl
{
    public abstract class Repl
    {

        public void Run()
        {
            while (true)
            {
                var text = EditSubmission();
                if (text == null)
                    return;

                EvaluateSubmission(text);
            }
        }

        private string EditSubmission()
        {
            var textBuilder = new StringBuilder();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(textBuilder.Length == 0 ? "» " : "· ");
                Console.ResetColor();

                var input = Console.ReadLine();
                var isBlank = string.IsNullOrWhiteSpace(input);

                if (textBuilder.Length == 0)
                {
                    if (isBlank)
                        return null;

                    if (input.StartsWith("#"))
                    {
                        EvaluateMetaCommand(input);
                        continue;
                    }
                }

                textBuilder.AppendLine(input);
                var text = textBuilder.ToString();

                if (!IsCompleteSubmission(text))
                    continue;

                return text;
            }
        }

        protected virtual void EvaluateMetaCommand(string input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid command {input}");
            Console.ResetColor();
        }

        protected abstract bool IsCompleteSubmission(string text);

        protected abstract void EvaluateSubmission(string text);
    }
}