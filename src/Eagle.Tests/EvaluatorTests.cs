using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.CodeAnalysis;
using Eagle.CodeAnalysis.Syntax;
using Xunit;

namespace Eagle.Tests
{
    public class EvaluatorTests
    {
        [Fact]
        public void Literal_LastValue()
        {
            var source = "100";
            var result = Evaluate(source);
            Assert.Equal(100L, result);
        }

        [Fact]
        public void UnaryExpression_LastValue()
        {
            var source = "-100";
            var result = Evaluate(source);
            Assert.Equal(-100L, result);
        }

        [Fact]
        public void BinaryExpression_LastValue()
        {
            var source = "49 + 51";
            var result = Evaluate(source);
            Assert.Equal(100L, result);
        }

        [Fact]
        public void MethodCall_LastValue()
        {
            var source = @"
object A {
    GetValue() {
        100
    }
}
A().GetValue()
";
            var result = Evaluate(source);
            Assert.Equal(100, (long)result);
        }

        [Fact]
        public void FieldInitializer_Value()
        {
            var source = @"
object A {
    b: int = 100
}
A().b
";
            var result = Evaluate(source);
            Assert.Equal(100, (long)result);
        }

        [Fact]
        public void Ctor_FieldValue()
        {
            var source = @"
object A {
    b: int
    A() {
        b = 100
    }
}
A().b
";
            var result = Evaluate(source);
            Assert.Equal(100L, (long)result);
        }

        private static object Evaluate(string source)
        {
            var tree = SyntaxTree.Parse(source);
            var compilation = Compilation.CreateScript(null, tree);
            var evaluationResult = compilation.Evaluate(new Dictionary<VariableSymbol, object>());
            if (evaluationResult.Diagnostics.Any())
            {
                throw new Exception(evaluationResult.Diagnostics.First().Message);
            }
            var result = evaluationResult.Value;
            return result;
        }
    }
}
