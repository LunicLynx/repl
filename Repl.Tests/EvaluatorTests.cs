using System;
using System.Collections.Generic;
using System.Linq;
using Repl.CodeAnalysis;
using Repl.CodeAnalysis.Syntax;
using Xunit;

namespace Repl.Tests
{
    public class EvaluatorTests
    {
        [Fact]
        public void Literal_LastValue()
        {
            var source = "100";
            var result = Evaluate(source);
            Assert.Equal(100, (int)result);
        }

        [Fact]
        public void UnaryExpression_LastValue()
        {
            var source = "-100";
            var result = Evaluate(source);
            Assert.Equal(-100, result);
        }

        [Fact]
        public void BinaryExpression_LastValue()
        {
            var source = "49 + 51";
            var result = Evaluate(source);
            Assert.Equal(100, result);
        }

        [Fact]
        public void MethodCall_LastValue()
        {
            var source = @"
struct A {
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
struct A {
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
struct A {
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
            var compilation = new Compilation(tree);
            var evaluationResult = compilation.Evaluate(new Dictionary<Symbol, object>());
            if (evaluationResult.Diagnostics.Any())
            {
                throw new Exception(evaluationResult.Diagnostics.First().Message);
            }
            var result = evaluationResult.Value;
            return result;
        }
    }
}
