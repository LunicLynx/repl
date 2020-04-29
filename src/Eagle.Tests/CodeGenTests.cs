using System.Collections.Immutable;
using System.Diagnostics;
using Eagle.CodeAnalysis.Binding;
using Eagle.CodeAnalysis.CodeGen;
using Eagle.CodeAnalysis.Syntax;
using LLVMSharp.Interop;
using Xunit;
using Xunit.Abstractions;

namespace Eagle.Tests
{
    public class CodeGenTests
    {
        private readonly ITestOutputHelper _output;

        public CodeGenTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void EmitEmptySourceShouldHaveMain()
        {
            var source = @"";
            var expected = @"
define void @Main() {
entry:
  ret void
}
";

            AssertGeneration(source, expected);
        }


        [Fact]
        public void EmitEmptyMainShouldHaveMain()
        {
            var source = @"Main(){}";

            var expected = @"
define void @Main() {
entry:
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithoutParameters()
        {
            var source = @"
Print() {}

Main() {}
";

            var expected = @"
define void @Print() {
entry:
  ret void
}

define void @Main() {
entry:
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithIntParameter()
        {
            var source = @"

Print(number: i64) {}

Main() {    }
";

            var expected = @"
define void @Print(i64) {
entry:
  %1 = alloca i64
  store i64 %0, i64* %1
  ret void
}

define void @Main() {
entry:
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithRefIntParameter()
        {
            var source = @"

Print(number: i64&) {}

Main() {    }
";

            var expected = @"
define void @Print(i64*) {
entry:
  %1 = alloca i64*
  store i64* %0, i64** %1
  ret void
}

define void @Main() {
entry:
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithPointerIntParameter()
        {
            var source = @"

Print(number: i64*) {}

Main() {    }
";

            var expected = @"
define void @Print(i64*) {
entry:
  %1 = alloca i64*
  store i64* %0, i64** %1
  ret void
}

define void @Main() {
entry:
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitStructEmpty()
        {
            var source = @"

object MyObj { }

Main() { }
";

            var expected = @"
%MyObj = type {}

define void @Main() {
entry:
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitStructWithInt()
        {
            var source = @"

object MyObj { a: int; }

Main() { }
";

            var expected = @"
%MyObj = type { i64 }

define void @Main() {
entry:
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }


        [Fact]
        public void EmitFunctionWithObjParameter()
        {
            var source = @"

object MyObj { a: int; b: int; }

Print(number: MyObj) {}

Main() {    }
";

            var expected = @"
%MyObj = type { i64, i64 }

define void @Print(%MyObj*) {
entry:
  ret void
}

define void @Main() {
entry:
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  // TODO initialize fields
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithRefObjParameter()
        {
            var source = @"

object MyObj { a: int; b: int; }

Print(number: MyObj&) {}

Main() {    }
";

            var expected = @"
define void @Print(i64*) {
entry:
  %1 = alloca i64*
  store i64* %0, i64** %1
  ret void
}

define void @Main() {
entry:
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithPointerObjParameter()
        {
            var source = @"

object MyObj { a: int; b: int; }

Print(number: MyObj*) {}

Main() {    }
";

            var expected = @"
define void @Print(i64*) {
entry:
  %1 = alloca i64*
  store i64* %0, i64** %1
  ret void
}

define void @Main() {
entry:
  ret void
}
";

            AssertGeneration(source, expected);
        }



        [Fact]
        public void EmitFunctionWithoutParametersAndCall()
        {
            var source = @"
Print() {}

Main() {
    Print();
}
";

            var expected = @"
define void @Print() {
entry:
  ret void
}

define void @Main() {
entry:
  call void @Print()
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionAddAndCall()
        {
            var source = @"

Add(a: int, b: int): int {
    return a + b;
}

Main() {
    Add(3, 4);
}
";

            var expected = @"

define i64 @Add(i64, i64) {
entry:
  %2 = alloca i64
  store i64 %0, i64* %2
  %3 = alloca i64
  store i64 %1, i64* %3
  %4 = load i64, i64* %2
  %5 = load i64, i64* %3
  %6 = add i64 %4, %5
  ret i64 %6
}

define void @Main() {
entry:
  %0 = call i64 @Add(i64 3, i64 4)
  ret void
}
";

            AssertGeneration(source, expected);
        }

        private void AssertGeneration(string source, string expected)
        {
            var syntaxTree = SyntaxTree.Parse(source);
            var globalScope = Binder.BindGlobalScope(false, null, ImmutableArray.Create(syntaxTree));
            var program = Binder.BindProgram(false, null, globalScope);
            var generator = new CodeGenerator(program, globalScope);

            using var context = LLVMContextRef.Create();
            using var mod = context.CreateModuleWithName("Test");
            generator.Generate(mod);

            var actual = mod.PrintToString().Replace("\n", "\r\n").Trim();

            expected = @"; ModuleID = 'Test'
source_filename = ""Test""

" + expected.Trim();


            _output.WriteLine("Expected");
            _output.WriteLine("========");
            _output.WriteLine(expected);

            _output.WriteLine("");
            _output.WriteLine("Actual");
            _output.WriteLine("======");
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}