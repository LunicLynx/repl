using Repl.CodeAnalysis.Text;

namespace Repl.CodeAnalysis
{
    public class Diagnostic
    {
        public string Message { get; }

        public Diagnostic(TextLocation location, string message)
        {
            Location = location;
            Message = message;
        }

        public TextLocation Location { get; set; }

        public override string ToString() => Message;
    }
}