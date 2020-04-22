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
  %length1 = load i64, i64* %length
  %7 = add i64 %length1, 1
  %8 = call i8* @malloc(i64 %7)
  %result = alloca i8*
  store i8* %8, i8** %result
  %i = alloca i64
  store i64 0, i64* %i
  %9 = call i64 @"<>Get_Length"(i8** %2)
  %upperBound = alloca i64
  store i64 %9, i64* %upperBound
  br label %Label1

Label2:                                           ; preds = %Label1
  %result2 = load i8*, i8** %result
  %i3 = load i64, i64* %i
  %10 = getelementptr i8, i8* %result2, i64 %i3
  %i4 = load i64, i64* %i
  %11 = call i8 @"<>Get_Item"(i8** %2, i64 %i4)
  store i8 %11, i8* %10
  br label %continue1

continue1:                                        ; preds = %Label2
  %i5 = load i64, i64* %i
  %12 = add i64 %i5, 1
  store i64 %12, i64* %i
  br label %Label1

Label1:                                           ; preds = %continue1, %entry
  %i6 = load i64, i64* %i
  %upperBound7 = load i64, i64* %upperBound
  %13 = icmp sle i64 %i6, %upperBound7
  br i1 %13, label %Label2, label %14

14:                                               ; preds = %Label1
  br label %break1

break1:                                           ; preds = %14
  %i8 = alloca i64
  store i64 0, i64* %i8
  %15 = call i64 @"<>Get_Length"(i8** %3)
  %upperBound9 = alloca i64
  store i64 %15, i64* %upperBound9
  br label %Label3

Label4:                                           ; preds = %Label3
  %result10 = load i8*, i8** %result
  %16 = call i64 @"<>Get_Length"(i8** %2)
  %i11 = load i64, i64* %i8
  %17 = add i64 %16, %i11
  %18 = getelementptr i8, i8* %result10, i64 %17
  %i12 = load i64, i64* %i8
  %19 = call i8 @"<>Get_Item"(i8** %3, i64 %i12)
  store i8 %19, i8* %18
  br label %continue2

continue2:                                        ; preds = %Label4
  %i13 = load i64, i64* %i8
  %20 = add i64 %i13, 1
  store i64 %20, i64* %i8
  br label %Label3

Label3:                                           ; preds = %continue2, %break1
  %i14 = load i64, i64* %i8
  %upperBound15 = load i64, i64* %upperBound9
  %21 = icmp sle i64 %i14, %upperBound15
  br i1 %21, label %Label4, label %22

22:                                               ; preds = %Label3
  br label %break2

break2:                                           ; preds = %22
  %result16 = load i8*, i8** %result
  %length17 = load i64, i64* %length
  %23 = getelementptr i8, i8* %result16, i64 %length17
  store i8 0, i8* %23
  %result18 = load i8*, i8** %result
  %24 = bitcast i8* %result18 to void*
  %25 = bitcast void* %24 to i8*
  ret i8* %25
}

define i8* @GetName() {
entry:
  call void @Print(i8* getelementptr inbounds ([18 x i8], [18 x i8]* @2, i32 0, i32 0))
  %0 = call i8* @Input()
  %name = alloca i8*
  store i8* %0, i8** %name
  %name1 = load i8*, i8** %name
  ret i8* %name1
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
  %index = load i64, i64* %3
  %5 = call i8 @"<>Get_Item"(i8** %4, i64 %index)
  ret i8 %5
}

define i64 @"<>Get_Length"(i8**) {
entry:
  %1 = alloca i8**
  store i8** %0, i8*** %1
  %l = alloca i64
  store i64 0, i64* %l
  %2 = load i8**, i8*** %1
  %self = alloca i8*
  store i8** %2, i8** %self
  br label %continue1

Label5:                                           ; preds = %continue1
  %l1 = load i64, i64* %l
  %3 = add i64 %l1, 1
  store i64 %3, i64* %l
  br label %continue1

continue1:                                        ; preds = %Label5, %entry
  %l2 = load i64, i64* %l
  %4 = call i8 @"<>Get_Item"(i8** %self, i64 %l2)
  %5 = icmp ne i8 %4, 0
  br i1 %5, label %Label5, label %6

6:                                                ; preds = %continue1
  br label %break1

break1:                                           ; preds = %6
  %l3 = load i64, i64* %l
  ret i64 %l3
}

declare i8* @String(i8**)

define void @Main() {
entry:
  %0 = call i8* @GetName()
  %name = alloca i8*
  store i8* %0, i8** %name
  %name1 = load i8*, i8** %name
  %1 = call i8* @Concat(i8* getelementptr inbounds ([7 x i8], [7 x i8]* @0, i32 0, i32 0), i8* %name1)
  %2 = call i8* @Concat(i8* %1, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @1, i32 0, i32 0))
  call void @Print(i8* %2)
  ret void
}
Stored value type does not match pointer operand type!
  store i8** %2, i8** %self
 i8*
C:\Users\Florian\source\repos\repl\src\ec\bin\Debug\netcoreapp3.1\win-x64\ec.exe (process 2932) exited with code 0.
To automatically close the console when debugging stops, enable Tools->Options->Debugging->Automatically close the console when debugging stops.
Press any key to close this window . . .
 