using System.Collections.Generic;
using System.IO;

namespace Eagle.CodeAnalysis.Binding
{
    public abstract class BoundNode
    {
        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                this.WriteTo(writer);
                return writer.ToString();
            }
        }
    }
}