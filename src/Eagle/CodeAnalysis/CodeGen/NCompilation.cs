namespace Repl.CodeAnalysis.CodeGen
{
    public class NCompilation
    {
        public NCompilation Previous { get; }
        public Compilation Compilation { get; }

        public NCompilation(Compilation compilation)
            : this(null, compilation)
        {

        }

        private NCompilation(NCompilation previous, Compilation compilation)
        {
            Previous = previous;
            Compilation = compilation;
        }

        public NCompilation ContinueWith(Compilation compilation)
        {
            return new NCompilation(this, compilation);
        }
    }
}