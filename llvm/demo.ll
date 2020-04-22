; ModuleID = '.\demo.cpp'
source_filename = ".\\demo.cpp"
target datalayout = "e-m:w-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc19.26.28720"

$"??_C@_02LEGMHFOO@?5?$CB?$AA@" = comdat any

$"??_C@_03MDGMGCCO@Flo?$AA@" = comdat any

$"??_C@_06LGHDPPDO@Hello?5?$AA@" = comdat any

@"??_C@_02LEGMHFOO@?5?$CB?$AA@" = linkonce_odr dso_local unnamed_addr constant [3 x i8] c" !\00", comdat, align 1
@"??_C@_03MDGMGCCO@Flo?$AA@" = linkonce_odr dso_local unnamed_addr constant [4 x i8] c"Flo\00", comdat, align 1
@"??_C@_06LGHDPPDO@Hello?5?$AA@" = linkonce_odr dso_local unnamed_addr constant [7 x i8] c"Hello \00", comdat, align 1

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @"?length@@YAHPEAPEBD@Z"(i8** %0) #0 {
  %2 = alloca i8**, align 8
  %3 = alloca i32, align 4
  store i8** %0, i8*** %2, align 8
  store i32 0, i32* %3, align 4
  br label %4

4:                                                ; preds = %13, %1
  %5 = load i8**, i8*** %2, align 8
  %6 = load i8*, i8** %5, align 8
  %7 = load i32, i32* %3, align 4
  %8 = sext i32 %7 to i64
  %9 = getelementptr inbounds i8, i8* %6, i64 %8
  %10 = load i8, i8* %9, align 1
  %11 = sext i8 %10 to i32
  %12 = icmp ne i32 %11, 0
  br i1 %12, label %13, label %16

13:                                               ; preds = %4
  %14 = load i32, i32* %3, align 4
  %15 = add nsw i32 %14, 1
  store i32 %15, i32* %3, align 4
  br label %4

16:                                               ; preds = %4
  %17 = load i32, i32* %3, align 4
  ret i32 %17
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i8 @"?item@@YADPEAPEBDH@Z"(i8** %0, i32 %1) #0 {
  %3 = alloca i32, align 4
  %4 = alloca i8**, align 8
  store i32 %1, i32* %3, align 4
  store i8** %0, i8*** %4, align 8
  %5 = load i8**, i8*** %4, align 8
  %6 = load i8*, i8** %5, align 8
  %7 = load i32, i32* %3, align 4
  %8 = sext i32 %7 to i64
  %9 = getelementptr inbounds i8, i8* %6, i64 %8
  %10 = load i8, i8* %9, align 1
  ret i8 %10
}

; Function Attrs: noinline optnone uwtable
define dso_local i8* @"?concat@@YAPEBDPEBD0@Z"(i8* %0, i8* %1) #1 {
  %3 = alloca i8*, align 8
  %4 = alloca i8*, align 8
  %5 = alloca i32, align 4
  %6 = alloca i8*, align 8
  %7 = alloca i32, align 4
  %8 = alloca i32, align 4
  store i8* %1, i8** %3, align 8
  store i8* %0, i8** %4, align 8
  %9 = call i32 @"?length@@YAHPEAPEBD@Z"(i8** %4)
  %10 = call i32 @"?length@@YAHPEAPEBD@Z"(i8** %3)
  %11 = add nsw i32 %9, %10
  store i32 %11, i32* %5, align 4
  %12 = load i32, i32* %5, align 4
  %13 = add nsw i32 %12, 1
  %14 = sext i32 %13 to i64
  %15 = call i8* @"??_U@YAPEAX_K@Z"(i64 %14) #5
  store i8* %15, i8** %6, align 8
  store i32 0, i32* %7, align 4
  br label %16

16:                                               ; preds = %27, %2
  %17 = load i32, i32* %7, align 4
  %18 = call i32 @"?length@@YAHPEAPEBD@Z"(i8** %4)
  %19 = icmp slt i32 %17, %18
  br i1 %19, label %20, label %30

20:                                               ; preds = %16
  %21 = load i32, i32* %7, align 4
  %22 = call i8 @"?item@@YADPEAPEBDH@Z"(i8** %4, i32 %21)
  %23 = load i8*, i8** %6, align 8
  %24 = load i32, i32* %7, align 4
  %25 = sext i32 %24 to i64
  %26 = getelementptr inbounds i8, i8* %23, i64 %25
  store i8 %22, i8* %26, align 1
  br label %27

27:                                               ; preds = %20
  %28 = load i32, i32* %7, align 4
  %29 = add nsw i32 %28, 1
  store i32 %29, i32* %7, align 4
  br label %16

30:                                               ; preds = %16
  store i32 0, i32* %8, align 4
  br label %31

31:                                               ; preds = %44, %30
  %32 = load i32, i32* %8, align 4
  %33 = call i32 @"?length@@YAHPEAPEBD@Z"(i8** %3)
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %47

35:                                               ; preds = %31
  %36 = load i32, i32* %8, align 4
  %37 = call i8 @"?item@@YADPEAPEBDH@Z"(i8** %3, i32 %36)
  %38 = load i8*, i8** %6, align 8
  %39 = call i32 @"?length@@YAHPEAPEBD@Z"(i8** %4)
  %40 = load i32, i32* %8, align 4
  %41 = add nsw i32 %39, %40
  %42 = sext i32 %41 to i64
  %43 = getelementptr inbounds i8, i8* %38, i64 %42
  store i8 %37, i8* %43, align 1
  br label %44

44:                                               ; preds = %35
  %45 = load i32, i32* %8, align 4
  %46 = add nsw i32 %45, 1
  store i32 %46, i32* %8, align 4
  br label %31

47:                                               ; preds = %31
  %48 = load i8*, i8** %6, align 8
  %49 = load i32, i32* %5, align 4
  %50 = sext i32 %49 to i64
  %51 = getelementptr inbounds i8, i8* %48, i64 %50
  store i8 0, i8* %51, align 1
  %52 = load i8*, i8** %6, align 8
  ret i8* %52
}

; Function Attrs: nobuiltin
declare dso_local noalias i8* @"??_U@YAPEAX_K@Z"(i64) #2

; Function Attrs: noinline norecurse optnone uwtable
define dso_local i32 @main() #3 {
  %1 = call i8* @"?concat@@YAPEBDPEBD0@Z"(i8* getelementptr inbounds ([4 x i8], [4 x i8]* @"??_C@_03MDGMGCCO@Flo?$AA@", i64 0, i64 0), i8* getelementptr inbounds ([3 x i8], [3 x i8]* @"??_C@_02LEGMHFOO@?5?$CB?$AA@", i64 0, i64 0))
  %2 = call i8* @"?concat@@YAPEBDPEBD0@Z"(i8* getelementptr inbounds ([7 x i8], [7 x i8]* @"??_C@_06LGHDPPDO@Hello?5?$AA@", i64 0, i64 0), i8* %1)
  call void @Print(i8* %2)
  ret i32 0
}

declare dso_local void @Print(i8*) #4

attributes #0 = { noinline nounwind optnone uwtable "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #1 = { noinline optnone uwtable "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #2 = { nobuiltin "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "no-infs-fp-math"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #3 = { noinline norecurse optnone uwtable "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #4 = { "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "no-infs-fp-math"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #5 = { builtin }

!llvm.module.flags = !{!0, !1}
!llvm.ident = !{!2}

!0 = !{i32 1, !"wchar_size", i32 2}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{!"clang version 10.0.0 "}
