using System.Collections.Generic;

namespace Repl.CodeAnalysis
{
    public class LocalContext : Context
    {
        public object This { get; }
        public Dictionary<Symbol, object> Store { get; } = new Dictionary<Symbol, object>();

        public LocalContext(object @this)
        {
            This = @this;
        }
    }
}