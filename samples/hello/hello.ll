; ModuleID = 'MyMod'
source_filename = "MyMod"

@0 = private unnamed_addr constant [7 x i8] c"Hello \00", align 1
@1 = private unnamed_addr constant [2 x i8] c"!\00", align 1
@2 = private unnamed_addr constant [18 x i8] c"What's your name?\00", align 1

define i8* @Concat(i8*, i8*) {
entry:
  %2 = alloca i8*
  store i8* %0, i8** %2
  %3 = alloca i8*
  store i8* %1, i8** %3
  %4 = call i64 @"<>Get_Length"(i8** %2)
  %5 = call i64 @"<>Get_Length"(i8** %3)
  %6 = add i64 %4, %5
  %length = alloca i64
  store i64 %6, i64* %length
  %7 = add i64* %length, i64 1
  %8 = call i8* @malloc(i64* %7)
  %result = alloca i8*
  store i8* %8, i8** %result
  %i = alloca i64
  store i64 0, i64* %i
  %9 = call i64 @"<>Get_Length"(i8** %2)
  %upperBound = alloca i64
  store i64 %9, i64* %upperBound
  br label %Label1

Label2:                                           ; preds = %Label1
  %10 = getelementptr i8*, i8** %result, i64* %i
  %11 = call i8 @"<>Get_Item"(i8** %2, i64* %i)
  store i8 %11, i8** %10
  br label %continue1

continue1:                                        ; preds = %Label2
  %12 = add i64* %i, i64 1
  store i64* %12, i64* %i
  br label %Label1

Label1:                                           ; preds = %continue1, %entry
  %13 = icmp sle i64* %i, %upperBound
  br i1 %13, label %Label2, label %14

14:                                               ; preds = %Label1
  br label %break1

break1:                                           ; preds = %14
  %i1 = alloca i64
  store i64 0, i64* %i1
  %15 = call i64 @"<>Get_Length"(i8** %3)
  %upperBound2 = alloca i64
  store i64 %15, i64* %upperBound2
  br label %Label3

Label4:                                           ; preds = %Label3
  %16 = call i64 @"<>Get_Length"(i8** %2)
  %17 = add i64 %16, i64* %i1
  %18 = getelementptr i8*, i8** %result, i64 %17
  %19 = call i8 @"<>Get_Item"(i8** %3, i64* %i1)
  store i8 %19, i8** %18
  br label %continue2

continue2:                                        ; preds = %Label4
  %20 = add i64* %i1, i64 1
  store i64* %20, i64* %i1
  br label %Label3

Label3:                                           ; preds = %continue2, %break1
  %21 = icmp sle i64* %i1, %upperBound2
  br i1 %21, label %Label4, label %22

22:                                               ; preds = %Label3
  br label %break2

break2:                                           ; preds = %22
  %23 = getelementptr i8*, i8** %result, i64* %length
  store i8 0, i8** %23
  %24 = bitcast i8** %result to void*
  %25 = bitcast void* %24 to i8*
  ret i8* %25
}

define i8* @GetName() {
entry:
  call void @Print([18 x i8]* @2)
  %0 = call i8* @Input()
  %name = alloca i8*
  store i8* %0, i8** %name
  ret i8** %name
}

declare void @Print(i8*)

declare i8* @Input()

declare i8* @malloc(i64)

declare void @free(i8*)

define i8 @"<>Get_Item"(i8**, i64) {
entry:
  %2 = alloca i8**
  store i8** %0, i8*** %2
  %3 = alloca i64
  store i64 %1, i64* %3

  %4 = load i8**, i8*** %2
  %5 = bitcast i8** %4 to i8*
  
  %6 = getelementptr i8, i8* %5, i64* %3
  ret i8* %6
}

define i64 @"<>Get_Length"(i8**) {
entry:
  %1 = alloca i8**
  store i8** %0, i8*** %1
  %l = alloca i64
  store i64 0, i64* %l
  %2 = load i8**, i8*** %1
  %3 = bitcast i8** %2 to i8*
  %self = alloca i8*
  store i8* %3, i8** %self
  br label %continue1

Label5:                                           ; preds = %continue1
  %4 = add i64* %l, i64 1
  store i64* %4, i64* %l
  br label %continue1

continue1:                                        ; preds = %Label5, %entry
  %5 = getelementptr i8*, i8** %self, i64* %l
  %6 = icmp ne i8** %5, i8 0
  br i1 %6, label %Label5, label %7

7:                                                ; preds = %continue1
  br label %break1

break1:                                           ; preds = %7
  ret i64* %l
}

declare i8* @String(i8**)

define void @Main() {
entry:
  %0 = call i8* @GetName()
  %name = alloca i8*
  store i8* %0, i8** %name
  %1 = call i8* @Concat([7 x i8]* @0, i8** %name)
  %2 = call i8* @Concat(i8* %1, [2 x i8]* @1)
  call void @Print(i8* %2)
}
Both operands to a binary operator are not of the same type!
  %7 = add i64* %length, i64 1
Call parameter type does not match function signature!
  %7 = add i64* %length, i64 1
 i64  %8 = call i8* @malloc(i64* %7)
GEP indexes must be integers
  %10 = getelementptr i8*, i8** %result, i64* %i
Call parameter type does not match function signature!
  %i = alloca i64
 i64  %11 = call i8 @"<>Get_Item"(i8** %2, i64* %i)
Stored value type does not match pointer operand type!
  store i8 %11, i8** %10
 i8*Both operands to a binary operator are not of the same type!
  %12 = add i64* %i, i64 1
Stored value type does not match pointer operand type!
  store i64* %12, i64* %i
 i64Both operands to a binary operator are not of the same type!
  %17 = add i64 %16, i64* %i1
Call parameter type does not match function signature!
  %i1 = alloca i64
 i64  %19 = call i8 @"<>Get_Item"(i8** %3, i64* %i1)
Stored value type does not match pointer operand type!
  store i8 %19, i8** %18
 i8*Both operands to a binary operator are not of the same type!
  %20 = add i64* %i1, i64 1
Stored value type does not match pointer operand type!
  store i64* %20, i64* %i1
 i64GEP indexes must be integers
  %23 = getelementptr i8*, i8** %result, i64* %length
Stored value type does not match pointer operand type!
  store i8 0, i8** %23
 i8*Call parameter type does not match function signature!
[18 x i8]* @2
 i8*  call void @Print([18 x i8]* @2)
Function return type does not match operand type of return inst!
  ret i8** %name
 i8*GEP indexes must be integers
  %6 = getelementptr i8, i8* %5, i64* %3
Function return type does not match operand type of return inst!
  ret i8* %6
 i8Both operands to a binary operator are not of the same type!
  %4 = add i64* %l, i64 1
Stored value type does not match pointer operand type!
  store i64* %4, i64* %l
 i64GEP indexes must be integers
  %5 = getelementptr i8*, i8** %self, i64* %l
Both operands to ICmp instruction are not of the same type!
  %6 = icmp ne i8** %5, i8 0
Function return type does not match operand type of return inst!
  ret i64* %l
 i64Basic Block in function 'Main' does not have terminator!
label %entry
Issues:Both operands to a binary operator are not of the same type!
  %7 = add i64* %length, i64 1
Call parameter type does not match function signature!
  %7 = add i64* %length, i64 1
 i64  %8 = call i8* @malloc(i64* %7)
GEP indexes must be integers
  %10 = getelementptr i8*, i8** %result, i64* %i
Call parameter type does not match function signature!
  %i = alloca i64
 i64  %11 = call i8 @"<>Get_Item"(i8** %2, i64* %i)
Stored value type does not match pointer operand type!
  store i8 %11, i8** %10
 i8*Both operands to a binary operator are not of the same type!
  %12 = add i64* %i, i64 1
Stored value type does not match pointer operand type!
  store i64* %12, i64* %i
 i64Both operands to a binary operator are not of the same type!
  %17 = add i64 %16, i64* %i1
Call parameter type does not match function signature!
  %i1 = alloca i64
 i64  %19 = call i8 @"<>Get_Item"(i8** %3, i64* %i1)
Stored value type does not match pointer operand type!
  store i8 %19, i8** %18
 i8*Both operands to a binary operator are not of the same type!
  %20 = add i64* %i1, i64 1
Stored value type does not match pointer operand type!
  store i64* %20, i64* %i1
 i64GEP indexes must be integers
  %23 = getelementptr i8*, i8** %result, i64* %length
Stored value type does not match pointer operand type!
  store i8 0, i8** %23
 i8*Call parameter type does not match function signature!
[18 x i8]* @2
 i8*  call void @Print([18 x i8]* @2)
Function return type does not match operand type of return inst!
  ret i8** %name
 i8*GEP indexes must be integers
  %6 = getelementptr i8, i8* %5, i64* %3
Function return type does not match operand type of return inst!
  ret i8* %6
 i8Both operands to a binary operator are not of the same type!
  %4 = add i64* %l, i64 1
Stored value type does not match pointer operand type!
  store i64* %4, i64* %l
 i64GEP indexes must be integers
  %5 = getelementptr i8*, i8** %self, i64* %l
Both operands to ICmp instruction are not of the same type!
  %6 = icmp ne i8** %5, i8 0
Function return type does not match operand type of return inst!
  ret i64* %l
 i64Basic Block in function 'Main' does not have terminator!
label %entry

