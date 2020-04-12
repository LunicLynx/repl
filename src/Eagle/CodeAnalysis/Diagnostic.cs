using Eagle.CodeAnalysis.Text;

namespace Eagle.CodeAnalysis
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