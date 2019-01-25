using Repl.CodeAnalysis.Binding;
using Repl.CodeAnalysis.Syntax;
using Repl.CodeAnalysis.Text;
using Xunit;

namespace Repl.Tests
{
    public class BinderTests
    {
        [Fact]
        public void BindMemberWithoutThisKeyword()
        {
            var source = "struct A { B(){} C(){ B() } }";

            var text = SourceText.From(source);

            var parser = new Parser(text);
            var compilationUnit = parser.ParseCompilationUnit();
            var scope = Binder.BindGlobalScope(null, compilationUnit);
        }
    }
}