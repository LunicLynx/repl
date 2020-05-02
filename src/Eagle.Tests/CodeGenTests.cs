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
        public void EmitCallFunctionWithoutParameters()
        {
            var source = @"
Print() {}

Main() { Print(); }
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
        public void EmitFunctionWithIntParameter()
        {
            var source = @"
Print(number: int) {}

Main() {}
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
        public void EmitCallFunctionWithIntParameter()
        {
            var source = @"
Print(number: int) {}

Main() { Print(1); }
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
  call void @Print(i64 1)
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithRefIntParameter()
        {
            var source = @"
Print(number: int&) {}

Main() {}
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
        public void EmitCallFunctionWithRefIntParameter()
        {
            var source = @"
Print(number: int&) {}

Main() { let x = 1; Print(x); }
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
  %0 = alloca i64
  store i64 1, i64* %0
  call void @Print(i64* %0)
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithPointerIntParameter()
        {
            var source = @"
Print(number: int*) {}

Main() {}
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
        public void EmitCallFunctionWithPointerIntParameter()
        {
            var source = @"
Print(number: int*) {}

Main() { let x = 1; Print(&x); }
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
  %0 = alloca i64
  store i64 1, i64* %0
  call void @Print(i64* %0)
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithIntReturn()
        {
            var source = @"
Print(): int { return 0; }

Main() {}
";

            var expected = @"
define i64 @Print() {
entry:
  ret i64 0
}

define void @Main() {
entry:
  ret void
}
";
            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitCallFunctionWithIntReturn()
        {
            var source = @"
Print(): int { return 0; }

Main() { Print(); }
";

            var expected = @"
define i64 @Print() {
entry:
  ret i64 0
}

define void @Main() {
entry:
  %0 = call i64 @Print()
  ret void
}
";
            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithRefIntReturn()
        {
            // TODO should emit a warning
            // because it references stack memory
            // these cases are only here test code generation
            var source = @"
Print(): int& { let x = 0; return x; }

Main() {}
";

            var expected = @"
define i64* @Print() {
entry:
  %0 = alloca i64
  store i64 0, i64* %0
  ret i64* %0
}

define void @Main() {
entry:
  ret void
}
";
            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitCallFunctionWithRefIntReturn()
        {
            // TODO should emit a warning
            // because it references stack memory
            // these cases are only here test code generation
            var source = @"
Print(): int& { let x = 0; return x; }

Main() { Print(); }
";

            var expected = @"
define i64* @Print() {
entry:
  %0 = alloca i64
  store i64 0, i64* %0
  ret i64* %0
}

define void @Main() {
entry:
  %0 = call i64* @Print()
  ret void
}
";
            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithPointerIntReturn()
        {
            // TODO should emit a warning
            // because it references stack memory
            // these cases are only here test code generation
            var source = @"
Print(): int* { let x = 0; return &x; }

Main() {}
";

            var expected = @"
define i64* @Print() {
entry:
  %0 = alloca i64
  store i64 0, i64* %0
  ret i64* %0
}

define void @Main() {
entry:
  ret void
}
";
            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitCallFunctionWithPointerIntReturn()
        {
            // TODO should emit a warning
            // because it references stack memory
            // these cases are only here test code generation
            var source = @"
Print(): int* { let x = 0; return &x; }

Main() { Print(); }
";

            var expected = @"
define i64* @Print() {
entry:
  %0 = alloca i64
  store i64 0, i64* %0
  ret i64* %0
}

define void @Main() {
entry:
  %0 = call i64* @Print()
  ret void
}
";
            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithObjReturn()
        {
            var source = @"
object MyObj { }

Print(): MyObj { return MyObj(); }

Main() {}
";

            var expected = @"
%MyObj = type {}

define void @Print(%MyObj*) {
entry:
  call void @MyObj(%MyObj* %0)
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
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";
            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithRefObjReturn()
        {
            var source = @"
object MyObj { }

Print(): MyObj& {
  let x = MyObj();
  return x;
}

Main() {}
";

            var expected = @"
%MyObj = type {}

define %MyObj* @Print() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  ret %MyObj* %0
}

define void @Main() {
entry:
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";
            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithPointerObjReturn()
        {
            var source = @"
object MyObj { }

Print(): MyObj* { 
  let x = MyObj();
  return &x;
}

Main() {}
";

            var expected = @"
%MyObj = type {}

define %MyObj* @Print() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  ret %MyObj* %0
}

define void @Main() {
entry:
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";
            AssertGeneration(source, expected);
        }


        [Fact]
        public void EmitObjectEmpty()
        {
            var source = @"
object MyObj { }

Main() {}
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
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitObjectWithInt()
        {
            var source = @"
object MyObj { a: int; }

Main() {}
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
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitReadField()
        {
            var source = @"
object MyObj { a: int; }

Main() {
    let x = MyObj()
    x.a;
}
";

            var expected = @"
%MyObj = type { i64 }

define void @Main() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  %1 = getelementptr inbounds %MyObj, %MyObj* %0, i32 0, i32 0
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        

        [Fact]
        public void EmitWriteField()
        {
            var source = @"
object MyObj { a: int; }

Main() {
    let x = MyObj();
    x.a = 10;
}
";

            var expected = @"
%MyObj = type { i64 }

define void @Main() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  %1 = getelementptr inbounds %MyObj, %MyObj* %0, i32 0, i32 0
  store i64 10, i64* %1
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitReadFieldOfReference()
        {
            var source = @"
object MyObj { a: int; }

Main() {
    let x = MyObj()
    let& y = x;
    y.a;
}
";

            var expected = @"
%MyObj = type { i64 }

define void @Main() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  %3 = getelementptr inbounds %MyObj, %MyObj* %2, i32 0, i32 0
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitWriteFieldOfReference()
        {
            var source = @"
object MyObj { a: int; }

Main() {
    let x = MyObj()
    let& y = x;
    y.a = 23;
}
";

            var expected = @"
%MyObj = type { i64 }

define void @Main() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  %3 = getelementptr inbounds %MyObj, %MyObj* %2, i32 0, i32 0
  store i64 23, i64* %3
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitReadFieldFromMethod()
        {
            var source = @"
object MyObj {
  a: int;
  Act() {
    a;
  }
}

Main() {}
";

            var expected = @"
%MyObj = type { i64 }

define void @Main() {
entry:
  ret void
}

define void @Act(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  %3 = getelementptr inbounds %MyObj, %MyObj* %2, i32 0, i32 0
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitWriteFieldFromMethod()
        {
            var source = @"
object MyObj { 
  a: int;
  Act() {
    a = 7;
  }
}

Main() {}
";

            var expected = @"
%MyObj = type { i64 }

define void @Main() {
entry:
  ret void
}

define void @Act(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  %3 = getelementptr inbounds %MyObj, %MyObj* %2, i32 0, i32 0
  store i64 7, i64* %3
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitCreateInstanceOfObject()
        {
            var source = @"
object MyObj { a: int; }

Main() { MyObj(); }
";

            var expected = @"
%MyObj = type { i64 }

define void @Main() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitCreateInstanceOfObjectAssignToVariable()
        {
            var source = @"
object MyObj { a: int; }

Main() { let x = MyObj(); }
";

            var expected = @"
%MyObj = type { i64 }

define void @Main() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
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

Main() {}
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
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitCallFunctionWithObjParameter()
        {
            var source = @"
object MyObj { a: int; b: int; }

Print(number: MyObj) {}

Main() { Print(MyObj()); }
";

            var expected = @"
%MyObj = type { i64, i64 }

define void @Print(%MyObj*) {
entry:
  ret void
}

define void @Main() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  call void @Print(%MyObj* %0)
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitCallFunctionWithObjParameterFromVariable()
        {
            var source = @"
object MyObj { a: int; b: int; }

Print(number: MyObj) {}

Main() { let x = MyObj(); Print(x); }
";

            var expected = @"
%MyObj = type { i64, i64 }

define void @Print(%MyObj*) {
entry:
  ret void
}

define void @Main() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  %1 = alloca %MyObj
  %2 = bitcast %MyObj* %1 to i8*
  %3 = bitcast %MyObj* %0 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* %2, i8* %3)
  call void @Print(%MyObj* %1)
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}

; Function Attrs: argmemonly nounwind
declare void @llvm.memcpy.p0i8.p0i8.i64(i8* nocapture writeonly, i8* nocapture readonly, i64, i1 immarg) #0

attributes #0 = { argmemonly nounwind }
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitFunctionWithRefObjParameter()
        {
            var source = @"
object MyObj { a: int; b: int; }

Print(number: MyObj&) {}

Main() {}
";

            var expected = @"
%MyObj = type { i64, i64 }

define void @Print(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
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
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitCallFunctionWithRefObjParameter()
        {
            var source = @"
object MyObj { a: int; b: int; }

Print(number: MyObj&) {}

Main() { let x = MyObj(); Print(x); }
";

            var expected = @"
%MyObj = type { i64, i64 }

define void @Print(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  ret void
}

define void @Main() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  call void @Print(%MyObj* %0)
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitCallFunctionWithRefObjParameterWithTempValue()
        {
            var source = @"
object MyObj { a: int; b: int; }

Act(number: MyObj&): MyObj {
  return MyObj();
}

Main() { 
  let x = MyObj(); 
  Act(Act(x));
}
";

            var expected = @"
%MyObj = type { i64, i64 }

define void @Act(%MyObj*, %MyObj*) {
entry:
  %2 = alloca %MyObj*
  store %MyObj* %1, %MyObj** %2
  call void @MyObj(%MyObj* %0)
  ret void
}

define void @Main() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  %1 = alloca %MyObj
  call void @Act(%MyObj* %1, %MyObj* %0)
  %2 = alloca %MyObj
  call void @Act(%MyObj* %2, %MyObj* %1)
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
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

Main() {}
";

            var expected = @"
%MyObj = type { i64, i64 }

define void @Print(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
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
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitCallFunctionWithPointerObjParameter()
        {
            var source = @"
object MyObj { a: int; b: int; }

Print(number: MyObj*) {}

Main() { let x = MyObj(); Print(&x); }
";

            var expected = @"
%MyObj = type { i64, i64 }

define void @Print(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  ret void
}

define void @Main() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  call void @Print(%MyObj* %0)
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitConstructor()
        {
            var source = @"
object MyObj { 
    ctor() {  }
}

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
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitMethod()
        {
            var source = @"
object MyObj { 
    Act() {  }
}

Main() { }
";

            var expected = @"
%MyObj = type {}

define void @Main() {
entry:
  ret void
}

define void @Act(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitCallMethod()
        {
            var source = @"
object MyObj { 
    Act() {  }
}

Main() { 
  let x = MyObj();
  x.Act();
}
";

            var expected = @"
%MyObj = type {}

define void @Main() {
entry:
  %0 = alloca %MyObj
  call void @MyObj(%MyObj* %0)
  call void @Act(%MyObj* %0)
  ret void
}

define void @Act(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitCallMethodFromMethod()
        {
            var source = @"
object MyObj { 
  Act() { 
    Act();  
  }
}

Main() { }
";

            var expected = @"
%MyObj = type {}

define void @Main() {
entry:
  ret void
}

define void @Act(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  call void @Act(%MyObj* %2)
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitStaticMethod()
        {
            var source = @"
object MyObj { 
    static Act() {  }
}

Main() { }
";

            var expected = @"
%MyObj = type {}

define void @Main() {
entry:
  ret void
}

define void @Act() {
entry:
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
  ret void
}
";

            AssertGeneration(source, expected);
        }

        [Fact]
        public void EmitCallStaticMethod()
        {
            var source = @"
object MyObj { 
    static Act() {  }
}

Main() {
    MyObj.Act();
}
";

            var expected = @"
%MyObj = type {}

define void @Main() {
entry:
  call void @Act()
  ret void
}

define void @Act() {
entry:
  ret void
}

define void @MyObj(%MyObj*) {
entry:
  %1 = alloca %MyObj*
  store %MyObj* %0, %MyObj** %1
  %2 = load %MyObj*, %MyObj** %1
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