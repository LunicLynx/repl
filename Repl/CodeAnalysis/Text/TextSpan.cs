namespace Repl.CodeAnalysis.Text
{
    public class TextSpan
    {
        public int Start { get; }
        public int Length { get; }

        public int End => Start + Length;

        public TextSpan(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public static TextSpan FromBounds(int start, int end)
        {
            return new TextSpan(start, end - start);
        }
    }
}