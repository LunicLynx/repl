; ModuleID = '.\main.cpp'
source_filename = ".\\main.cpp"
target datalayout = "e-m:w-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc19.26.28804"

%class.Point = type { i64, i64, i64, i64 }
%struct.i64_2 = type { i64, i64 }
%class.Single = type { i64 }
%class.Empty = type { i8 }

$"??0Single@@QEAA@XZ" = comdat any

$"??0Empty@@QEAA@XZ" = comdat any

$"?Act@Single@@QEAAXXZ" = comdat any

$"?StaticAct@Single@@SAXXZ" = comdat any

@__const.main.pa = private unnamed_addr constant %class.Point { i64 1, i64 5, i64 8, i64 3 }, align 8
@__const.main.pb = private unnamed_addr constant %class.Point { i64 6, i64 3, i64 2, i64 4 }, align 8

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?print@@YAXXZ"() #0 {
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?print1@@YAX_J@Z"(i64 %0) #0 {
  %2 = alloca i64, align 8
  store i64 %0, i64* %2, align 8
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?print2@@YAXAEA_J@Z"(i64* dereferenceable(8) %0) #0 {
  %2 = alloca i64*, align 8
  store i64* %0, i64** %2, align 8
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?print3@@YAXPEA_J@Z"(i64* %0) #0 {
  %2 = alloca i64*, align 8
  store i64* %0, i64** %2, align 8
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?print4@@YAXUi64_2@@@Z"(%struct.i64_2* %0) #0 {
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?print5@@YAXAEAUi64_2@@@Z"(%struct.i64_2* dereferenceable(16) %0) #0 {
  %2 = alloca %struct.i64_2*, align 8
  store %struct.i64_2* %0, %struct.i64_2** %2, align 8
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?print6@@YAXPEAUi64_2@@@Z"(%struct.i64_2* %0) #0 {
  %2 = alloca %struct.i64_2*, align 8
  store %struct.i64_2* %0, %struct.i64_2** %2, align 8
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @"?print7@@YA_JXZ"() #0 {
  ret i64 0
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64* @"?print8@@YAPEA_JXZ"() #0 {
  %1 = alloca i64, align 8
  store i64 0, i64* %1, align 8
  ret i64* %1
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dereferenceable(8) i64* @"?print9@@YAAEA_JXZ"() #0 {
  %1 = alloca i64, align 8
  store i64 0, i64* %1, align 8
  ret i64* %1
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?print10@@YA?AUi64_2@@XZ"(%struct.i64_2* noalias sret %0) #0 {
  %2 = alloca i8*, align 8
  %3 = bitcast %struct.i64_2* %0 to i8*
  store i8* %3, i8** %2, align 8
  %4 = bitcast %struct.i64_2* %0 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %4, i8 0, i64 16, i1 false)
  ret void
}

; Function Attrs: argmemonly nounwind willreturn
declare void @llvm.memset.p0i8.i64(i8* nocapture writeonly, i8, i64, i1 immarg) #1

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dereferenceable(16) %struct.i64_2* @"?print11@@YAAEAUi64_2@@XZ"() #0 {
  %1 = alloca %struct.i64_2, align 8
  %2 = bitcast %struct.i64_2* %1 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %2, i8 0, i64 16, i1 false)
  ret %struct.i64_2* %1
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local %struct.i64_2* @"?print12@@YAPEAUi64_2@@XZ"() #0 {
  %1 = alloca %struct.i64_2, align 8
  %2 = bitcast %struct.i64_2* %1 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %2, i8 0, i64 16, i1 false)
  ret %struct.i64_2* %1
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @"?add1@@YA_J_J0@Z"(i64 %0, i64 %1) #0 {
  %3 = alloca i64, align 8
  %4 = alloca i64, align 8
  store i64 %1, i64* %3, align 8
  store i64 %0, i64* %4, align 8
  %5 = load i64, i64* %4, align 8
  %6 = load i64, i64* %3, align 8
  %7 = add nsw i64 %5, %6
  ret i64 %7
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @"?add2@@YA_JAEA_J0@Z"(i64* dereferenceable(8) %0, i64* dereferenceable(8) %1) #0 {
  %3 = alloca i64*, align 8
  %4 = alloca i64*, align 8
  %5 = alloca i64, align 8
  store i64* %1, i64** %3, align 8
  store i64* %0, i64** %4, align 8
  %6 = load i64*, i64** %4, align 8
  %7 = load i64, i64* %6, align 8
  %8 = load i64*, i64** %3, align 8
  %9 = load i64, i64* %8, align 8
  %10 = add nsw i64 %7, %9
  store i64 %10, i64* %5, align 8
  %11 = load i64, i64* %5, align 8
  ret i64 %11
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i64 @"?add3@@YA_JPEA_J0@Z"(i64* %0, i64* %1) #0 {
  %3 = alloca i64*, align 8
  %4 = alloca i64*, align 8
  store i64* %1, i64** %3, align 8
  store i64* %0, i64** %4, align 8
  %5 = load i64*, i64** %4, align 8
  %6 = load i64, i64* %5, align 8
  %7 = load i64*, i64** %3, align 8
  %8 = load i64, i64* %7, align 8
  %9 = add nsw i64 %6, %8
  ret i64 %9
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?padd1@@YA?AVPoint@@V1@0@Z"(%class.Point* noalias sret %0, %class.Point* %1, %class.Point* %2) #0 {
  %4 = alloca i8*, align 8
  %5 = bitcast %class.Point* %0 to i8*
  store i8* %5, i8** %4, align 8
  %6 = bitcast %class.Point* %0 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %6, i8 0, i64 32, i1 false)
  %7 = getelementptr inbounds %class.Point, %class.Point* %1, i32 0, i32 0
  %8 = load i64, i64* %7, align 8
  %9 = getelementptr inbounds %class.Point, %class.Point* %2, i32 0, i32 0
  %10 = load i64, i64* %9, align 8
  %11 = add nsw i64 %8, %10
  %12 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 0
  store i64 %11, i64* %12, align 8
  %13 = getelementptr inbounds %class.Point, %class.Point* %1, i32 0, i32 1
  %14 = load i64, i64* %13, align 8
  %15 = getelementptr inbounds %class.Point, %class.Point* %2, i32 0, i32 1
  %16 = load i64, i64* %15, align 8
  %17 = add nsw i64 %14, %16
  %18 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 1
  store i64 %17, i64* %18, align 8
  %19 = getelementptr inbounds %class.Point, %class.Point* %1, i32 0, i32 2
  %20 = load i64, i64* %19, align 8
  %21 = getelementptr inbounds %class.Point, %class.Point* %2, i32 0, i32 2
  %22 = load i64, i64* %21, align 8
  %23 = add nsw i64 %20, %22
  %24 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 2
  store i64 %23, i64* %24, align 8
  %25 = getelementptr inbounds %class.Point, %class.Point* %1, i32 0, i32 3
  %26 = load i64, i64* %25, align 8
  %27 = getelementptr inbounds %class.Point, %class.Point* %2, i32 0, i32 3
  %28 = load i64, i64* %27, align 8
  %29 = add nsw i64 %26, %28
  %30 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 3
  store i64 %29, i64* %30, align 8
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?padd2@@YA?AVPoint@@AEAV1@0@Z"(%class.Point* noalias sret %0, %class.Point* dereferenceable(32) %1, %class.Point* dereferenceable(32) %2) #0 {
  %4 = alloca i8*, align 8
  %5 = alloca %class.Point*, align 8
  %6 = alloca %class.Point*, align 8
  %7 = bitcast %class.Point* %0 to i8*
  store i8* %7, i8** %4, align 8
  store %class.Point* %2, %class.Point** %5, align 8
  store %class.Point* %1, %class.Point** %6, align 8
  %8 = bitcast %class.Point* %0 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %8, i8 0, i64 32, i1 false)
  %9 = load %class.Point*, %class.Point** %6, align 8
  %10 = getelementptr inbounds %class.Point, %class.Point* %9, i32 0, i32 0
  %11 = load i64, i64* %10, align 8
  %12 = load %class.Point*, %class.Point** %5, align 8
  %13 = getelementptr inbounds %class.Point, %class.Point* %12, i32 0, i32 0
  %14 = load i64, i64* %13, align 8
  %15 = add nsw i64 %11, %14
  %16 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 0
  store i64 %15, i64* %16, align 8
  %17 = load %class.Point*, %class.Point** %6, align 8
  %18 = getelementptr inbounds %class.Point, %class.Point* %17, i32 0, i32 1
  %19 = load i64, i64* %18, align 8
  %20 = load %class.Point*, %class.Point** %5, align 8
  %21 = getelementptr inbounds %class.Point, %class.Point* %20, i32 0, i32 1
  %22 = load i64, i64* %21, align 8
  %23 = add nsw i64 %19, %22
  %24 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 1
  store i64 %23, i64* %24, align 8
  %25 = load %class.Point*, %class.Point** %6, align 8
  %26 = getelementptr inbounds %class.Point, %class.Point* %25, i32 0, i32 2
  %27 = load i64, i64* %26, align 8
  %28 = load %class.Point*, %class.Point** %5, align 8
  %29 = getelementptr inbounds %class.Point, %class.Point* %28, i32 0, i32 2
  %30 = load i64, i64* %29, align 8
  %31 = add nsw i64 %27, %30
  %32 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 2
  store i64 %31, i64* %32, align 8
  %33 = load %class.Point*, %class.Point** %6, align 8
  %34 = getelementptr inbounds %class.Point, %class.Point* %33, i32 0, i32 3
  %35 = load i64, i64* %34, align 8
  %36 = load %class.Point*, %class.Point** %5, align 8
  %37 = getelementptr inbounds %class.Point, %class.Point* %36, i32 0, i32 3
  %38 = load i64, i64* %37, align 8
  %39 = add nsw i64 %35, %38
  %40 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 3
  store i64 %39, i64* %40, align 8
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
  call void @llvm.memset.p0i8.i64(i8* align 8 %8, i8 0, i64 32, i1 false)
  %9 = load %class.Point*, %class.Point** %6, align 8
  %10 = getelementptr inbounds %class.Point, %class.Point* %9, i32 0, i32 0
  %11 = load i64, i64* %10, align 8
  %12 = load %class.Point*, %class.Point** %5, align 8
  %13 = getelementptr inbounds %class.Point, %class.Point* %12, i32 0, i32 0
  %14 = load i64, i64* %13, align 8
  %15 = add nsw i64 %11, %14
  %16 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 0
  store i64 %15, i64* %16, align 8
  %17 = load %class.Point*, %class.Point** %6, align 8
  %18 = getelementptr inbounds %class.Point, %class.Point* %17, i32 0, i32 1
  %19 = load i64, i64* %18, align 8
  %20 = load %class.Point*, %class.Point** %5, align 8
  %21 = getelementptr inbounds %class.Point, %class.Point* %20, i32 0, i32 1
  %22 = load i64, i64* %21, align 8
  %23 = add nsw i64 %19, %22
  %24 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 1
  store i64 %23, i64* %24, align 8
  %25 = load %class.Point*, %class.Point** %6, align 8
  %26 = getelementptr inbounds %class.Point, %class.Point* %25, i32 0, i32 2
  %27 = load i64, i64* %26, align 8
  %28 = load %class.Point*, %class.Point** %5, align 8
  %29 = getelementptr inbounds %class.Point, %class.Point* %28, i32 0, i32 2
  %30 = load i64, i64* %29, align 8
  %31 = add nsw i64 %27, %30
  %32 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 2
  store i64 %31, i64* %32, align 8
  %33 = load %class.Point*, %class.Point** %6, align 8
  %34 = getelementptr inbounds %class.Point, %class.Point* %33, i32 0, i32 3
  %35 = load i64, i64* %34, align 8
  %36 = load %class.Point*, %class.Point** %5, align 8
  %37 = getelementptr inbounds %class.Point, %class.Point* %36, i32 0, i32 3
  %38 = load i64, i64* %37, align 8
  %39 = add nsw i64 %35, %38
  %40 = getelementptr inbounds %class.Point, %class.Point* %0, i32 0, i32 3
  store i64 %39, i64* %40, align 8
  ret void
}

; Function Attrs: noinline norecurse optnone uwtable
define dso_local i32 @main() #2 {
  %1 = alloca i64, align 8
  %2 = alloca i64, align 8
  %3 = alloca i64, align 8
  %4 = alloca i64, align 8
  %5 = alloca i64, align 8
  %6 = alloca %class.Single, align 8
  %7 = alloca %class.Point, align 8
  %8 = alloca %class.Point, align 8
  %9 = alloca %class.Point, align 8
  %10 = alloca %class.Point, align 8
  %11 = alloca %class.Point, align 8
  %12 = alloca %class.Point, align 8
  %13 = alloca %class.Point, align 8
  %14 = alloca %class.Point, align 8
  %15 = alloca %class.Empty, align 1
  %16 = alloca %struct.i64_2, align 8
  %17 = alloca %struct.i64_2, align 8
  %18 = alloca %struct.i64_2, align 8
  %19 = alloca %struct.i64_2, align 8
  store i64 4, i64* %1, align 8
  store i64 7, i64* %2, align 8
  %20 = call i64 @"?add1@@YA_J_J0@Z"(i64 5, i64 6)
  %21 = load i64, i64* %2, align 8
  %22 = load i64, i64* %1, align 8
  %23 = call i64 @"?add1@@YA_J_J0@Z"(i64 %22, i64 %21)
  store i64 %23, i64* %3, align 8
  %24 = call i64 @"?add2@@YA_JAEA_J0@Z"(i64* dereferenceable(8) %1, i64* dereferenceable(8) %2)
  store i64 %24, i64* %4, align 8
  %25 = call i64 @"?add3@@YA_JPEA_J0@Z"(i64* %1, i64* %2)
  store i64 %25, i64* %5, align 8
  %26 = call %class.Single* @"??0Single@@QEAA@XZ"(%class.Single* %6)
  %27 = bitcast %class.Point* %7 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %27, i8 0, i64 32, i1 false)
  %28 = bitcast %class.Point* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %28, i8* align 8 bitcast (%class.Point* @__const.main.pa to i8*), i64 32, i1 false)
  %29 = bitcast %class.Point* %9 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %29, i8* align 8 bitcast (%class.Point* @__const.main.pb to i8*), i64 32, i1 false)
  %30 = bitcast %class.Point* %11 to i8*
  %31 = bitcast %class.Point* %9 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %30, i8* align 8 %31, i64 32, i1 false)
  %32 = bitcast %class.Point* %12 to i8*
  %33 = bitcast %class.Point* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %32, i8* align 8 %33, i64 32, i1 false)
  call void @"?padd1@@YA?AVPoint@@V1@0@Z"(%class.Point* sret %10, %class.Point* %12, %class.Point* %11)
  call void @"?padd2@@YA?AVPoint@@AEAV1@0@Z"(%class.Point* sret %13, %class.Point* dereferenceable(32) %8, %class.Point* dereferenceable(32) %9)
  call void @"?padd3@@YA?AVPoint@@PEAV1@0@Z"(%class.Point* sret %14, %class.Point* %8, %class.Point* %9)
  %34 = call %class.Empty* @"??0Empty@@QEAA@XZ"(%class.Empty* %15)
  %35 = bitcast %struct.i64_2* %16 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %35, i8 0, i64 16, i1 false)
  call void @"?print@@YAXXZ"()
  call void @"?print1@@YAX_J@Z"(i64 0)
  call void @"?print2@@YAXAEA_J@Z"(i64* dereferenceable(8) %1)
  call void @"?print3@@YAXPEA_J@Z"(i64* %1)
  %36 = bitcast %struct.i64_2* %17 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %36, i8 0, i64 16, i1 false)
  call void @"?print4@@YAXUi64_2@@@Z"(%struct.i64_2* %17)
  %37 = bitcast %struct.i64_2* %18 to i8*
  %38 = bitcast %struct.i64_2* %16 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %37, i8* align 8 %38, i64 16, i1 false)
  call void @"?print4@@YAXUi64_2@@@Z"(%struct.i64_2* %18)
  call void @"?print5@@YAXAEAUi64_2@@@Z"(%struct.i64_2* dereferenceable(16) %16)
  call void @"?print6@@YAXPEAUi64_2@@@Z"(%struct.i64_2* %16)
  %39 = call i64 @"?print7@@YA_JXZ"()
  %40 = call i64* @"?print8@@YAPEA_JXZ"()
  %41 = call dereferenceable(8) i64* @"?print9@@YAAEA_JXZ"()
  call void @"?print10@@YA?AUi64_2@@XZ"(%struct.i64_2* sret %19)
  %42 = call %struct.i64_2* @"?print12@@YAPEAUi64_2@@XZ"()
  call void @"?Act@Single@@QEAAXXZ"(%class.Single* %6)
  call void @"?StaticAct@Single@@SAXXZ"()
  ret i32 0
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %class.Single* @"??0Single@@QEAA@XZ"(%class.Single* returned %0) unnamed_addr #0 comdat align 2 {
  %2 = alloca %class.Single*, align 8
  store %class.Single* %0, %class.Single** %2, align 8
  %3 = load %class.Single*, %class.Single** %2, align 8
  ret %class.Single* %3
}

; Function Attrs: argmemonly nounwind willreturn
declare void @llvm.memcpy.p0i8.p0i8.i64(i8* noalias nocapture writeonly, i8* noalias nocapture readonly, i64, i1 immarg) #1

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %class.Empty* @"??0Empty@@QEAA@XZ"(%class.Empty* returned %0) unnamed_addr #0 comdat align 2 {
  %2 = alloca %class.Empty*, align 8
  store %class.Empty* %0, %class.Empty** %2, align 8
  %3 = load %class.Empty*, %class.Empty** %2, align 8
  ret %class.Empty* %3
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local void @"?Act@Single@@QEAAXXZ"(%class.Single* %0) #3 comdat align 2 {
  %2 = alloca %class.Single*, align 8
  store %class.Single* %0, %class.Single** %2, align 8
  %3 = load %class.Single*, %class.Single** %2, align 8
  call void @"?Act@Single@@QEAAXXZ"(%class.Single* %3)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"?StaticAct@Single@@SAXXZ"() #0 comdat align 2 {
  ret void
}

attributes #0 = { noinline nounwind optnone uwtable "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #1 = { argmemonly nounwind willreturn }
attributes #2 = { noinline norecurse optnone uwtable "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #3 = { noinline optnone uwtable "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }

!llvm.module.flags = !{!0, !1}
!llvm.ident = !{!2}

!0 = !{i32 1, !"wchar_size", i32 2}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{!"clang version 10.0.0 "}
