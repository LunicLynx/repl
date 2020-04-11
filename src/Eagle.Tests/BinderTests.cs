using System.Collections.Immutable;
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

            var syntaxTree = SyntaxTree.Parse(source);

            //var scope = Binder.BindGlobalScope(null, ImmutableArray.Create(syntaxTree));
        }
    }
}