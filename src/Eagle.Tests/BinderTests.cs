using Eagle.CodeAnalysis.Syntax;
using Xunit;

namespace Eagle.Tests
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