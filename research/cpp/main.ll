; ModuleID = '.\main.cpp'
source_filename = ".\\main.cpp"
target datalayout = "e-m:w-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc19.26.28803"

%class.Point = type { i32, i32, i32, i32 }

@__const.main.pa = private unnamed_addr constant %class.Point { i32 1, i32 5, i32 0, i32 0 }, align 4
@__const.main.pb = private unnamed_addr constant %class.Point { i32 6, i32 3, i32 0, i32 0 }, align 4

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @"?add1@@YAHHH@Z"(i32 %0, i32 %1) #0 {
  %3 = alloca i32, align 4
  %4 = alloca i32, align 4
  store i32 %1, i32* %3, align 4
  store i32 %0, i32* %4, align 4
  %5 = load i32, i32* %4, align 4
  %6 = load i32, i32* %3, align 4
  %7 = add nsw i32 %5, %6
  ret i32 %7
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @"?add2@@YAHAEAH0@Z"(i32* dereferenceable(4) %0, i32* dereferenceable(4) %1) #0 {
  %3 = alloca i32*, align 8
  %4 = alloca i32*, align 8
  %5 = alloca i32, align 4
  store i32* %1, i32** %3, align 8
  store i32* %0, i32** %4, align 8
  %6 = load i32*, i32** %4, align 8
  %7 = load i32, i32* %6, align 4
  %8 = load i32*, i32** %3, align 8
  %9 = load i32, i32* %8, align 4
  %10 = add nsw i32 %7, %9
  store i32 %10, i32* %5, align 4
  %11 = load i32, i32* %5, align 4
  ret i32 %11
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @"?add3@@YAHPEAH0@Z"(i32* %0, i32* %1) #0 {
  %3 = alloca i32*, align 8
  %4 = alloca i32*, align 8
  store i32* %1, i32** %3, align 8
  store i32* %0, i32** %4, align 8
  %5 = load i32*, i32** %4, align 8
  %6 = load i32, i32* %5, align 4
  %7 = load i32*, i32** %3, align 8
  %8 = load i32, i32* %7, align 4
  %9 = add nsw i32 %6, %8
  ret i32 %9
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?padd1@@YA?AVPoint@@V1@0@Z"(%class.Point* noalias sret %0, %class.Point* %1, %class.Point* %2) #0 {
  %4 = alloca i8*, align 8
  %5 = bitcast %class.Point* %0 to i8*
  store i8* %5, i8** %4, align 8
  %6 = bitcast %class.Point* %0 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 4 %6, i8 0, i64 16, i1 false)
  %7 = getelementptr inbounds %class.Point, %class.Point* %1, i32 0, i32 0
  %8 = load i32, i32* %7, align 4
  %9 = getelementptr inbounds %class.Point, %class.Point* %2, i32 0, i32 0
  %10 = load i32, i32* %9, align 4
  %11 = add nsw i32 %8, %10
  %12 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 0
  store i32 %11, i32* %12, align 4
  %13 = getelementptr inbounds %class.Point, %class.Point* %1, i32 0, i32 1
  %14 = load i32, i32* %13, align 4
  %15 = getelementptr inbounds %class.Point, %class.Point* %2, i32 0, i32 1
  %16 = load i32, i32* %15, align 4
  %17 = add nsw i32 %14, %16
  %18 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 1
  store i32 %17, i32* %18, align 4
  %19 = getelementptr inbounds %class.Point, %class.Point* %1, i32 0, i32 2
  %20 = load i32, i32* %19, align 4
  %21 = getelementptr inbounds %class.Point, %class.Point* %2, i32 0, i32 2
  %22 = load i32, i32* %21, align 4
  %23 = add nsw i32 %20, %22
  %24 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 2
  store i32 %23, i32* %24, align 4
  %25 = getelementptr inbounds %class.Point, %class.Point* %1, i32 0, i32 3
  %26 = load i32, i32* %25, align 4
  %27 = getelementptr inbounds %class.Point, %class.Point* %2, i32 0, i32 3
  %28 = load i32, i32* %27, align 4
  %29 = add nsw i32 %26, %28
  %30 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 3
  store i32 %29, i32* %30, align 4
  ret void
}

; Function Attrs: argmemonly nounwind willreturn
declare void @llvm.memset.p0i8.i64(i8* nocapture writeonly, i8, i64, i1 immarg) #1

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?padd2@@YA?AVPoint@@AEAV1@0@Z"(%class.Point* noalias sret %0, %class.Point* dereferenceable(16) %1, %class.Point* dereferenceable(16) %2) #0 {
  %4 = alloca i8*, align 8
  %5 = alloca %class.Point*, align 8
  %6 = alloca %class.Point*, align 8
  %7 = bitcast %class.Point* %0 to i8*
  store i8* %7, i8** %4, align 8
  store %class.Point* %2, %class.Point** %5, align 8
  store %class.Point* %1, %class.Point** %6, align 8
  %8 = bitcast %class.Point* %0 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 4 %8, i8 0, i64 16, i1 false)
  %9 = load %class.Point*, %class.Point** %6, align 8
  %10 = getelementptr inbounds %class.Point, %class.Point* %9, i32 0, i32 0
  %11 = load i32, i32* %10, align 4
  %12 = load %class.Point*, %class.Point** %5, align 8
  %13 = getelementptr inbounds %class.Point, %class.Point* %12, i32 0, i32 0
  %14 = load i32, i32* %13, align 4
  %15 = add nsw i32 %11, %14
  %16 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 0
  store i32 %15, i32* %16, align 4
  %17 = load %class.Point*, %class.Point** %6, align 8
  %18 = getelementptr inbounds %class.Point, %class.Point* %17, i32 0, i32 1
  %19 = load i32, i32* %18, align 4
  %20 = load %class.Point*, %class.Point** %5, align 8
  %21 = getelementptr inbounds %class.Point, %class.Point* %20, i32 0, i32 1
  %22 = load i32, i32* %21, align 4
  %23 = add nsw i32 %19, %22
  %24 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 1
  store i32 %23, i32* %24, align 4
  %25 = load %class.Point*, %class.Point** %6, align 8
  %26 = getelementptr inbounds %class.Point, %class.Point* %25, i32 0, i32 2
  %27 = load i32, i32* %26, align 4
  %28 = load %class.Point*, %class.Point** %5, align 8
  %29 = getelementptr inbounds %class.Point, %class.Point* %28, i32 0, i32 2
  %30 = load i32, i32* %29, align 4
  %31 = add nsw i32 %27, %30
  %32 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 2
  store i32 %31, i32* %32, align 4
  %33 = load %class.Point*, %class.Point** %6, align 8
  %34 = getelementptr inbounds %class.Point, %class.Point* %33, i32 0, i32 3
  %35 = load i32, i32* %34, align 4
  %36 = load %class.Point*, %class.Point** %5, align 8
  %37 = getelementptr inbounds %class.Point, %class.Point* %36, i32 0, i32 3
  %38 = load i32, i32* %37, align 4
  %39 = add nsw i32 %35, %38
  %40 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 3
  store i32 %39, i32* %40, align 4
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?padd3@@YA?AVPoint@@PEAV1@0@Z"(%class.Point* noalias sret %0, %class.Point* %1, %class.Point* %2) #0 {
  %4 = alloca i8*, align 8
  %5 = alloca %class.Point*, align 8
  %6 = alloca %class.Point*, align 8
  %7 = bitcast %class.Point* %0 to i8*
  store i8* %7, i8** %4, align 8
  store %class.Point* %2, %class.Point** %5, align 8
  store %class.Point* %1, %class.Point** %6, align 8
  %8 = bitcast %class.Point* %0 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 4 %8, i8 0, i64 16, i1 false)
  %9 = load %class.Point*, %class.Point** %6, align 8
  %10 = getelementptr inbounds %class.Point, %class.Point* %9, i32 0, i32 0
  %11 = load i32, i32* %10, align 4
  %12 = load %class.Point*, %class.Point** %5, align 8
  %13 = getelementptr inbounds %class.Point, %class.Point* %12, i32 0, i32 0
  %14 = load i32, i32* %13, align 4
  %15 = add nsw i32 %11, %14
  %16 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 0
  store i32 %15, i32* %16, align 4
  %17 = load %class.Point*, %class.Point** %6, align 8
  %18 = getelementptr inbounds %class.Point, %class.Point* %17, i32 0, i32 1
  %19 = load i32, i32* %18, align 4
  %20 = load %class.Point*, %class.Point** %5, align 8
  %21 = getelementptr inbounds %class.Point, %class.Point* %20, i32 0, i32 1
  %22 = load i32, i32* %21, align 4
  %23 = add nsw i32 %19, %22
  %24 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 1
  store i32 %23, i32* %24, align 4
  %25 = load %class.Point*, %class.Point** %6, align 8
  %26 = getelementptr inbounds %class.Point, %class.Point* %25, i32 0, i32 2
  %27 = load i32, i32* %26, align 4
  %28 = load %class.Point*, %class.Point** %5, align 8
  %29 = getelementptr inbounds %class.Point, %class.Point* %28, i32 0, i32 2
  %30 = load i32, i32* %29, align 4
  %31 = add nsw i32 %27, %30
  %32 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 2
  store i32 %31, i32* %32, align 4
  %33 = load %class.Point*, %class.Point** %6, align 8
  %34 = getelementptr inbounds %class.Point, %class.Point* %33, i32 0, i32 3
  %35 = load i32, i32* %34, align 4
  %36 = load %class.Point*, %class.Point** %5, align 8
  %37 = getelementptr inbounds %class.Point, %class.Point* %36, i32 0, i32 3
  %38 = load i32, i32* %37, align 4
  %39 = add nsw i32 %35, %38
  %40 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 3
  store i32 %39, i32* %40, align 4
  ret void
}

; Function Attrs: noinline norecurse nounwind optnone uwtable
define dso_local i32 @main() #2 {
  %1 = alloca i32, align 4
  %2 = alloca i32, align 4
  %3 = alloca i32, align 4
  %4 = alloca i32, align 4
  %5 = alloca i32, align 4
  %6 = alloca %class.Point, align 4
  %7 = alloca %class.Point, align 4
  %8 = alloca %class.Point, align 4
  %9 = alloca %class.Point, align 4
  %10 = alloca %class.Point, align 4
  %11 = alloca %class.Point, align 4
  %12 = alloca %class.Point, align 4
  store i32 4, i32* %1, align 4
  store i32 7, i32* %2, align 4
  %13 = load i32, i32* %2, align 4
  %14 = load i32, i32* %1, align 4
  %15 = call i32 @"?add1@@YAHHH@Z"(i32 %14, i32 %13)
  store i32 %15, i32* %3, align 4
  %16 = call i32 @"?add2@@YAHAEAH0@Z"(i32* dereferenceable(4) %1, i32* dereferenceable(4) %2)
  store i32 %16, i32* %4, align 4
  %17 = call i32 @"?add3@@YAHPEAH0@Z"(i32* %1, i32* %2)
  store i32 %17, i32* %5, align 4
  %18 = bitcast %class.Point* %6 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 4 %18, i8* align 4 bitcast (%class.Point* @__const.main.pa to i8*), i64 16, i1 false)
  %19 = bitcast %class.Point* %7 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 4 %19, i8* align 4 bitcast (%class.Point* @__const.main.pb to i8*), i64 16, i1 false)
  %20 = bitcast %class.Point* %9 to i8*
  %21 = bitcast %class.Point* %7 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 4 %20, i8* align 4 %21, i64 16, i1 false)
  %22 = bitcast %class.Point* %10 to i8*
  %23 = bitcast %class.Point* %6 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 4 %22, i8* align 4 %23, i64 16, i1 false)
  call void @"?padd1@@YA?AVPoint@@V1@0@Z"(%class.Point* sret %8, %class.Point* %10, %class.Point* %9)
  call void @"?padd2@@YA?AVPoint@@AEAV1@0@Z"(%class.Point* sret %11, %class.Point* dereferenceable(16) %6, %class.Point* dereferenceable(16) %7)
  call void @"?padd3@@YA?AVPoint@@PEAV1@0@Z"(%class.Point* sret %12, %class.Point* %6, %class.Point* %7)
  ret i32 0
}

; Function Attrs: argmemonly nounwind willreturn
declare void @llvm.memcpy.p0i8.p0i8.i64(i8* noalias nocapture writeonly, i8* noalias nocapture readonly, i64, i1 immarg) #1

attributes #0 = { noinline nounwind optnone uwtable "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #1 = { argmemonly nounwind willreturn }
attributes #2 = { noinline norecurse nounwind optnone uwtable "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }

!llvm.module.flags = !{!0, !1}
!llvm.ident = !{!2}

!0 = !{i32 1, !"wchar_size", i32 2}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{!"clang version 10.0.0 "}
