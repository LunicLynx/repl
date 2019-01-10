using System;
using System.Collections.Generic;

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

        public override bool Equals(object obj)
        {
            var span = obj as TextSpan;
            return span != null &&
                   Start == span.Start &&
                   Length == span.Length;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, Length);
        }

        public static bool operator ==(TextSpan span1, TextSpan span2)
        {
            return EqualityComparer<TextSpan>.Default.Equals(span1, span2);
        }

        public static bool operator !=(TextSpan span1, TextSpan span2)
        {
            return !(span1 == span2);
        }
    }
}