; ModuleID = '.\main.cpp'
source_filename = ".\\main.cpp"
target datalayout = "e-m:w-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc19.26.28805"

%class.Point = type { i64, i64, i64, i64 }
%rtti.TypeDescriptor30 = type { i8**, i8*, [31 x i8] }
%eh.CatchableType = type { i32, i32, i32, i32, i32, i32, i32 }
%rtti.TypeDescriptor19 = type { i8**, i8*, [20 x i8] }
%eh.CatchableTypeArray.3 = type { i32, [3 x i32] }
%eh.ThrowInfo = type { i32, i32, i32, i32 }
%rtti.CompleteObjectLocator = type { i32, i32, i32, i32, i32, i32 }
%rtti.ClassHierarchyDescriptor = type { i32, i32, i32, i32 }
%rtti.BaseClassDescriptor = type { i32, i32, i32, i32, i32, i32, i32 }
%struct.i64_2 = type { i64, i64 }
%class.Single = type { i64 }
%class.Empty = type { i8 }
%"class.std::basic_string" = type { %"class.std::_Compressed_pair" }
%"class.std::_Compressed_pair" = type { %"class.std::_String_val" }
%"class.std::_String_val" = type { %"union.std::_String_val<std::_Simple_types<char> >::_Bxty", i64, i64 }
%"union.std::_String_val<std::_Simple_types<char> >::_Bxty" = type { i8*, [8 x i8] }
%"struct.std::_Zero_then_variadic_args_t" = type { i8 }
%"struct.std::_Fake_allocator" = type { i8 }
%"struct.std::_Fake_proxy_ptr_impl" = type { i8 }
%"struct.std::_Container_base0" = type { i8 }
%"struct.std::_String_constructor_concat_tag" = type { i8 }
%"class.std::allocator" = type { i8 }
%class.anon = type { i8 }
%"class.std::bad_array_new_length" = type { %"class.std::bad_alloc" }
%"class.std::bad_alloc" = type { %"class.std::exception" }
%"class.std::exception" = type { i32 (...)**, %struct.__std_exception_data }
%struct.__std_exception_data = type { i8*, i8 }
%"struct.std::_One_then_variadic_args_t" = type { i8 }
%"struct.std::integral_constant" = type { i8 }
%class.anon.0 = type { i8 }

$"??0Single@@QEAA@XZ" = comdat any

$"??0Empty@@QEAA@XZ" = comdat any

$"?Act@Single@@QEAAXXZ" = comdat any

$"?StaticAct@Single@@SAXXZ" = comdat any

$"??0?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@QEBD@Z" = comdat any

$"??$?HDU?$char_traits@D@std@@V?$allocator@D@1@@std@@YA?AV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@0@$$QEAV10@AEBV10@@Z" = comdat any

$"??$?HDU?$char_traits@D@std@@V?$allocator@D@1@@std@@YA?AV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@0@AEBV10@QEBD@Z" = comdat any

$"??1?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@XZ" = comdat any

$"?Create@Single@@QEAA?AV1@AEBV1@@Z" = comdat any

$"?_Tidy_deallocate@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAXXZ" = comdat any

$"??1?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAA@XZ" = comdat any

$"?_Orphan_all@_Container_base0@std@@QEAAXXZ" = comdat any

$"?_Large_string_engaged@?$_String_val@U?$_Simple_types@D@std@@@std@@QEBA_NXZ" = comdat any

$"?_Getal@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAAEAV?$allocator@D@2@XZ" = comdat any

$"??$_Destroy_in_place@PEAD@std@@YAXAEAPEAD@Z" = comdat any

$"?deallocate@?$allocator@D@std@@QEAAXQEAD_K@Z" = comdat any

$"?assign@?$_Narrow_char_traits@DH@std@@SAXAEADAEBD@Z" = comdat any

$"?_Get_first@?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAAAEAV?$allocator@D@2@XZ" = comdat any

$"??$_Deallocate@$0BA@$0A@@std@@YAXPEAX_K@Z" = comdat any

$"?_Adjust_manually_vector_aligned@std@@YAXAEAPEAXAEA_K@Z" = comdat any

$"??1?$_String_val@U?$_Simple_types@D@std@@@std@@QEAA@XZ" = comdat any

$"??1_Bxty@?$_String_val@U?$_Simple_types@D@std@@@std@@QEAA@XZ" = comdat any

$"??$?0$$V@?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAA@U_Zero_then_variadic_args_t@1@@Z" = comdat any

$"??0_Fake_proxy_ptr_impl@std@@QEAA@AEBU_Fake_allocator@1@AEBU_Container_base0@1@@Z" = comdat any

$"?_Tidy_init@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAXXZ" = comdat any

$"?assign@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV12@QEBD@Z" = comdat any

$"?_Release@_Fake_proxy_ptr_impl@std@@QEAAXXZ" = comdat any

$"??0?$allocator@D@std@@QEAA@XZ" = comdat any

$"??0?$_String_val@U?$_Simple_types@D@std@@@std@@QEAA@XZ" = comdat any

$"??0_Bxty@?$_String_val@U?$_Simple_types@D@std@@@std@@QEAA@XZ" = comdat any

$"?assign@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV12@QEBD_K@Z" = comdat any

$"??$_Convert_size@_K@std@@YA_K_K@Z" = comdat any

$"?length@?$_Narrow_char_traits@DH@std@@SA_KQEBD@Z" = comdat any

$"?_Myptr@?$_String_val@U?$_Simple_types@D@std@@@std@@QEAAPEADXZ" = comdat any

$"?move@?$_Narrow_char_traits@DH@std@@SAPEADQEADQEBD_K@Z" = comdat any

$"??$_Reallocate_for@V<lambda_1>@?0??assign@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV34@QEBD_K@Z@PEBD@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAAEAV01@_KV<lambda_1>@?0??assign@01@QEAAAEAV01@QEBD0@Z@PEBD@Z" = comdat any

$"??$_Unfancy@D@std@@YAPEADPEAD@Z" = comdat any

$"?max_size@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEBA_KXZ" = comdat any

$"?_Xlen_string@std@@YAXXZ" = comdat any

$"?_Calculate_growth@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEBA_K_K@Z" = comdat any

$"?allocate@?$allocator@D@std@@QEAAPEAD_K@Z" = comdat any

$"??R<lambda_1>@?0??assign@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV23@QEBD_K@Z@QEBA?A?<auto>@@QEAD10@Z" = comdat any

$"??$_Construct_in_place@PEADAEBQEAD@std@@YAXAEAPEADAEBQEAD@Z" = comdat any

$"?max_size@?$_Default_allocator_traits@V?$allocator@D@std@@@std@@SA_KAEBV?$allocator@D@2@@Z" = comdat any

$"?_Getal@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEBAAEBV?$allocator@D@2@XZ" = comdat any

$"??$_Max_value@_K@std@@YAAEB_KAEB_K0@Z" = comdat any

$"??$_Min_value@_K@std@@YAAEB_KAEB_K0@Z" = comdat any

$"?max@?$numeric_limits@_J@std@@SA_JXZ" = comdat any

$"?_Get_first@?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEBAAEBV?$allocator@D@2@XZ" = comdat any

$"?_Calculate_growth@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@CA_K_K00@Z" = comdat any

$"??$_Allocate@$0BA@U_Default_allocate_traits@std@@$0A@@std@@YAPEAX_K@Z" = comdat any

$"??$_Get_size_of_n@$00@std@@YA_K_K@Z" = comdat any

$"??$_Allocate_manually_vector_aligned@U_Default_allocate_traits@std@@@std@@YAPEAX_K@Z" = comdat any

$"?_Allocate@_Default_allocate_traits@std@@SAPEAX_K@Z" = comdat any

$"?_Throw_bad_array_new_length@std@@YAXXZ" = comdat any

$"??0bad_array_new_length@std@@QEAA@XZ" = comdat any

$"??0bad_array_new_length@std@@QEAA@AEBV01@@Z" = comdat any

$"??0bad_alloc@std@@QEAA@AEBV01@@Z" = comdat any

$"??0exception@std@@QEAA@AEBV01@@Z" = comdat any

$"??1bad_array_new_length@std@@UEAA@XZ" = comdat any

$"??0bad_alloc@std@@AEAA@QEBD@Z" = comdat any

$"??_Gbad_array_new_length@std@@UEAAPEAXI@Z" = comdat any

$"?what@exception@std@@UEBAPEBDXZ" = comdat any

$"??0exception@std@@QEAA@QEBDH@Z" = comdat any

$"??_Gbad_alloc@std@@UEAAPEAXI@Z" = comdat any

$"??_Gexception@std@@UEAAPEAXI@Z" = comdat any

$"??1exception@std@@UEAA@XZ" = comdat any

$"??1bad_alloc@std@@UEAA@XZ" = comdat any

$"?copy@?$_Narrow_char_traits@DH@std@@SAPEADQEADQEBD_K@Z" = comdat any

$"??$addressof@PEAD@std@@YAPEAPEADAEAPEAD@Z" = comdat any

$"??$forward@AEBQEAD@std@@YAAEBQEADAEBQEAD@Z" = comdat any

$"?size@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEBA_KXZ" = comdat any

$"?c_str@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEBAPEBDXZ" = comdat any

$"??0?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@U_String_constructor_concat_tag@1@AEBV01@QEBD_K23@Z" = comdat any

$"?_Myptr@?$_String_val@U?$_Simple_types@D@std@@@std@@QEBAPEBDXZ" = comdat any

$"?select_on_container_copy_construction@?$_Default_allocator_traits@V?$allocator@D@std@@@std@@SA?AV?$allocator@D@2@AEBV32@@Z" = comdat any

$"??$?0V?$allocator@D@std@@$$V@?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAA@U_One_then_variadic_args_t@1@$$QEAV?$allocator@D@1@@Z" = comdat any

$"??$forward@V?$allocator@D@std@@@std@@YA$$QEAV?$allocator@D@0@AEAV10@@Z" = comdat any

$"??$move@AEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@std@@YA$$QEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@0@AEAV10@@Z" = comdat any

$"?append@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV12@AEBV12@@Z" = comdat any

$"??0?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@$$QEAV01@@Z" = comdat any

$"?append@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV12@QEBD_K@Z" = comdat any

$"??$_Reallocate_grow_by@V<lambda_1>@?0??append@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV34@QEBD_K@Z@PEBD_K@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAAEAV01@_KV<lambda_1>@?0??append@01@QEAAAEAV01@QEBD0@Z@PEBD_K@Z" = comdat any

$"??R<lambda_1>@?0??append@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV23@QEBD_K@Z@QEBA?A?<auto>@@QEAD0101@Z" = comdat any

$"??$move@AEAV?$allocator@D@std@@@std@@YA$$QEAV?$allocator@D@0@AEAV10@@Z" = comdat any

$"?_Alloc_proxy@_Container_base0@std@@QEAAXAEBU_Fake_allocator@2@@Z" = comdat any

$"?_Take_contents@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAXAEAV12@U?$integral_constant@_N$00@2@@Z" = comdat any

$"?_Memcpy_val_from@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAXAEBV12@@Z" = comdat any

$"??$addressof@V?$_String_val@U?$_Simple_types@D@std@@@std@@@std@@YAPEAV?$_String_val@U?$_Simple_types@D@std@@@0@AEAV10@@Z" = comdat any

$"??$addressof@$$CBV?$_String_val@U?$_Simple_types@D@std@@@std@@@std@@YAPEBV?$_String_val@U?$_Simple_types@D@std@@@0@AEBV10@@Z" = comdat any

$"??_C@_05NLDMIFDH@Hallo?$AA@" = comdat any

$"??_C@_04LFIIJCLO@Welt?$AA@" = comdat any

$"??_C@_01CLKCMJKC@?5?$AA@" = comdat any

$"??_C@_0BA@JFNIOLAK@string?5too?5long?$AA@" = comdat any

$"??_R0?AVbad_array_new_length@std@@@8" = comdat any

$"_CT??_R0?AVbad_array_new_length@std@@@8??0bad_array_new_length@std@@QEAA@AEBV01@@Z24" = comdat any

$"??_R0?AVbad_alloc@std@@@8" = comdat any

$"_CT??_R0?AVbad_alloc@std@@@8??0bad_alloc@std@@QEAA@AEBV01@@Z24" = comdat any

$"??_R0?AVexception@std@@@8" = comdat any

$"_CT??_R0?AVexception@std@@@8??0exception@std@@QEAA@AEBV01@@Z24" = comdat any

$"_CTA3?AVbad_array_new_length@std@@" = comdat any

$"_TI3?AVbad_array_new_length@std@@" = comdat any

$"??_C@_0BF@KINCDENJ@bad?5array?5new?5length?$AA@" = comdat any

$"??_7bad_array_new_length@std@@6B@" = comdat largest

$"??_R4bad_array_new_length@std@@6B@" = comdat any

$"??_R3bad_array_new_length@std@@8" = comdat any

$"??_R2bad_array_new_length@std@@8" = comdat any

$"??_R1A@?0A@EA@bad_array_new_length@std@@8" = comdat any

$"??_R1A@?0A@EA@bad_alloc@std@@8" = comdat any

$"??_R3bad_alloc@std@@8" = comdat any

$"??_R2bad_alloc@std@@8" = comdat any

$"??_R1A@?0A@EA@exception@std@@8" = comdat any

$"??_R3exception@std@@8" = comdat any

$"??_R2exception@std@@8" = comdat any

$"??_7bad_alloc@std@@6B@" = comdat largest

$"??_R4bad_alloc@std@@6B@" = comdat any

$"??_7exception@std@@6B@" = comdat largest

$"??_R4exception@std@@6B@" = comdat any

$"??_C@_0BC@EOODALEL@Unknown?5exception?$AA@" = comdat any

@__const.main.pa = private unnamed_addr constant %class.Point { i64 1, i64 5, i64 8, i64 3 }, align 8
@__const.main.pb = private unnamed_addr constant %class.Point { i64 6, i64 3, i64 2, i64 4 }, align 8
@"??_C@_05NLDMIFDH@Hallo?$AA@" = linkonce_odr dso_local unnamed_addr constant [6 x i8] c"Hallo\00", comdat, align 1
@"??_C@_04LFIIJCLO@Welt?$AA@" = linkonce_odr dso_local unnamed_addr constant [5 x i8] c"Welt\00", comdat, align 1
@"??_C@_01CLKCMJKC@?5?$AA@" = linkonce_odr dso_local unnamed_addr constant [2 x i8] c" \00", comdat, align 1
@"??_C@_0BA@JFNIOLAK@string?5too?5long?$AA@" = linkonce_odr dso_local unnamed_addr constant [16 x i8] c"string too long\00", comdat, align 1
@"??_7type_info@@6B@" = external constant i8*
@"??_R0?AVbad_array_new_length@std@@@8" = linkonce_odr global %rtti.TypeDescriptor30 { i8** @"??_7type_info@@6B@", i8* null, [31 x i8] c".?AVbad_array_new_length@std@@\00" }, comdat
@__ImageBase = external dso_local constant i8
@"_CT??_R0?AVbad_array_new_length@std@@@8??0bad_array_new_length@std@@QEAA@AEBV01@@Z24" = linkonce_odr unnamed_addr constant %eh.CatchableType { i32 0, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.TypeDescriptor30* @"??_R0?AVbad_array_new_length@std@@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 0, i32 -1, i32 0, i32 24, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%"class.std::bad_array_new_length"* (%"class.std::bad_array_new_length"*, %"class.std::bad_array_new_length"*)* @"??0bad_array_new_length@std@@QEAA@AEBV01@@Z" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32) }, section ".xdata", comdat
@"??_R0?AVbad_alloc@std@@@8" = linkonce_odr global %rtti.TypeDescriptor19 { i8** @"??_7type_info@@6B@", i8* null, [20 x i8] c".?AVbad_alloc@std@@\00" }, comdat
@"_CT??_R0?AVbad_alloc@std@@@8??0bad_alloc@std@@QEAA@AEBV01@@Z24" = linkonce_odr unnamed_addr constant %eh.CatchableType { i32 16, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.TypeDescriptor19* @"??_R0?AVbad_alloc@std@@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 0, i32 -1, i32 0, i32 24, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%"class.std::bad_alloc"* (%"class.std::bad_alloc"*, %"class.std::bad_alloc"*)* @"??0bad_alloc@std@@QEAA@AEBV01@@Z" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32) }, section ".xdata", comdat
@"??_R0?AVexception@std@@@8" = linkonce_odr global %rtti.TypeDescriptor19 { i8** @"??_7type_info@@6B@", i8* null, [20 x i8] c".?AVexception@std@@\00" }, comdat
@"_CT??_R0?AVexception@std@@@8??0exception@std@@QEAA@AEBV01@@Z24" = linkonce_odr unnamed_addr constant %eh.CatchableType { i32 0, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.TypeDescriptor19* @"??_R0?AVexception@std@@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 0, i32 -1, i32 0, i32 24, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%"class.std::exception"* (%"class.std::exception"*, %"class.std::exception"*)* @"??0exception@std@@QEAA@AEBV01@@Z" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32) }, section ".xdata", comdat
@"_CTA3?AVbad_array_new_length@std@@" = linkonce_odr unnamed_addr constant %eh.CatchableTypeArray.3 { i32 3, [3 x i32] [i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%eh.CatchableType* @"_CT??_R0?AVbad_array_new_length@std@@@8??0bad_array_new_length@std@@QEAA@AEBV01@@Z24" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%eh.CatchableType* @"_CT??_R0?AVbad_alloc@std@@@8??0bad_alloc@std@@QEAA@AEBV01@@Z24" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%eh.CatchableType* @"_CT??_R0?AVexception@std@@@8??0exception@std@@QEAA@AEBV01@@Z24" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32)] }, section ".xdata", comdat
@"_TI3?AVbad_array_new_length@std@@" = linkonce_odr unnamed_addr constant %eh.ThrowInfo { i32 0, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (void (%"class.std::bad_array_new_length"*)* @"??1bad_array_new_length@std@@UEAA@XZ" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 0, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%eh.CatchableTypeArray.3* @"_CTA3?AVbad_array_new_length@std@@" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32) }, section ".xdata", comdat
@"??_C@_0BF@KINCDENJ@bad?5array?5new?5length?$AA@" = linkonce_odr dso_local unnamed_addr constant [21 x i8] c"bad array new length\00", comdat, align 1
@0 = private unnamed_addr constant { [3 x i8*] } { [3 x i8*] [i8* bitcast (%rtti.CompleteObjectLocator* @"??_R4bad_array_new_length@std@@6B@" to i8*), i8* bitcast (i8* (%"class.std::bad_array_new_length"*, i32)* @"??_Gbad_array_new_length@std@@UEAAPEAXI@Z" to i8*), i8* bitcast (i8* (%"class.std::exception"*)* @"?what@exception@std@@UEBAPEBDXZ" to i8*)] }, comdat($"??_7bad_array_new_length@std@@6B@")
@"??_R4bad_array_new_length@std@@6B@" = linkonce_odr constant %rtti.CompleteObjectLocator { i32 1, i32 0, i32 0, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.TypeDescriptor30* @"??_R0?AVbad_array_new_length@std@@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.ClassHierarchyDescriptor* @"??_R3bad_array_new_length@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.CompleteObjectLocator* @"??_R4bad_array_new_length@std@@6B@" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32) }, comdat
@"??_R3bad_array_new_length@std@@8" = linkonce_odr constant %rtti.ClassHierarchyDescriptor { i32 0, i32 0, i32 3, i32 trunc (i64 sub nuw nsw (i64 ptrtoint ([4 x i32]* @"??_R2bad_array_new_length@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32) }, comdat
@"??_R2bad_array_new_length@std@@8" = linkonce_odr constant [4 x i32] [i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.BaseClassDescriptor* @"??_R1A@?0A@EA@bad_array_new_length@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.BaseClassDescriptor* @"??_R1A@?0A@EA@bad_alloc@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.BaseClassDescriptor* @"??_R1A@?0A@EA@exception@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 0], comdat
@"??_R1A@?0A@EA@bad_array_new_length@std@@8" = linkonce_odr constant %rtti.BaseClassDescriptor { i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.TypeDescriptor30* @"??_R0?AVbad_array_new_length@std@@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 2, i32 0, i32 -1, i32 0, i32 64, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.ClassHierarchyDescriptor* @"??_R3bad_array_new_length@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32) }, comdat
@"??_R1A@?0A@EA@bad_alloc@std@@8" = linkonce_odr constant %rtti.BaseClassDescriptor { i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.TypeDescriptor19* @"??_R0?AVbad_alloc@std@@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 1, i32 0, i32 -1, i32 0, i32 64, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.ClassHierarchyDescriptor* @"??_R3bad_alloc@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32) }, comdat
@"??_R3bad_alloc@std@@8" = linkonce_odr constant %rtti.ClassHierarchyDescriptor { i32 0, i32 0, i32 2, i32 trunc (i64 sub nuw nsw (i64 ptrtoint ([3 x i32]* @"??_R2bad_alloc@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32) }, comdat
@"??_R2bad_alloc@std@@8" = linkonce_odr constant [3 x i32] [i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.BaseClassDescriptor* @"??_R1A@?0A@EA@bad_alloc@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.BaseClassDescriptor* @"??_R1A@?0A@EA@exception@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 0], comdat
@"??_R1A@?0A@EA@exception@std@@8" = linkonce_odr constant %rtti.BaseClassDescriptor { i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.TypeDescriptor19* @"??_R0?AVexception@std@@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 0, i32 0, i32 -1, i32 0, i32 64, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.ClassHierarchyDescriptor* @"??_R3exception@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32) }, comdat
@"??_R3exception@std@@8" = linkonce_odr constant %rtti.ClassHierarchyDescriptor { i32 0, i32 0, i32 1, i32 trunc (i64 sub nuw nsw (i64 ptrtoint ([2 x i32]* @"??_R2exception@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32) }, comdat
@"??_R2exception@std@@8" = linkonce_odr constant [2 x i32] [i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.BaseClassDescriptor* @"??_R1A@?0A@EA@exception@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 0], comdat
@1 = private unnamed_addr constant { [3 x i8*] } { [3 x i8*] [i8* bitcast (%rtti.CompleteObjectLocator* @"??_R4bad_alloc@std@@6B@" to i8*), i8* bitcast (i8* (%"class.std::bad_alloc"*, i32)* @"??_Gbad_alloc@std@@UEAAPEAXI@Z" to i8*), i8* bitcast (i8* (%"class.std::exception"*)* @"?what@exception@std@@UEBAPEBDXZ" to i8*)] }, comdat($"??_7bad_alloc@std@@6B@")
@"??_R4bad_alloc@std@@6B@" = linkonce_odr constant %rtti.CompleteObjectLocator { i32 1, i32 0, i32 0, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.TypeDescriptor19* @"??_R0?AVbad_alloc@std@@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.ClassHierarchyDescriptor* @"??_R3bad_alloc@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.CompleteObjectLocator* @"??_R4bad_alloc@std@@6B@" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32) }, comdat
@2 = private unnamed_addr constant { [3 x i8*] } { [3 x i8*] [i8* bitcast (%rtti.CompleteObjectLocator* @"??_R4exception@std@@6B@" to i8*), i8* bitcast (i8* (%"class.std::exception"*, i32)* @"??_Gexception@std@@UEAAPEAXI@Z" to i8*), i8* bitcast (i8* (%"class.std::exception"*)* @"?what@exception@std@@UEBAPEBDXZ" to i8*)] }, comdat($"??_7exception@std@@6B@")
@"??_R4exception@std@@6B@" = linkonce_odr constant %rtti.CompleteObjectLocator { i32 1, i32 0, i32 0, i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.TypeDescriptor19* @"??_R0?AVexception@std@@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.ClassHierarchyDescriptor* @"??_R3exception@std@@8" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32), i32 trunc (i64 sub nuw nsw (i64 ptrtoint (%rtti.CompleteObjectLocator* @"??_R4exception@std@@6B@" to i64), i64 ptrtoint (i8* @__ImageBase to i64)) to i32) }, comdat
@"??_C@_0BC@EOODALEL@Unknown?5exception?$AA@" = linkonce_odr dso_local unnamed_addr constant [18 x i8] c"Unknown exception\00", comdat, align 1

@"??_7bad_array_new_length@std@@6B@" = unnamed_addr alias i8*, getelementptr inbounds ({ [3 x i8*] }, { [3 x i8*] }* @0, i32 0, i32 0, i32 1)
@"??_7bad_alloc@std@@6B@" = unnamed_addr alias i8*, getelementptr inbounds ({ [3 x i8*] }, { [3 x i8*] }* @1, i32 0, i32 0, i32 1)
@"??_7exception@std@@6B@" = unnamed_addr alias i8*, getelementptr inbounds ({ [3 x i8*] }, { [3 x i8*] }* @2, i32 0, i32 0, i32 1)

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
define dso_local void @"?print5_1@@YAXAEBUi64_2@@@Z"(%struct.i64_2* dereferenceable(16) %0) #0 {
  %2 = alloca %struct.i64_2*, align 8
  store %struct.i64_2* %0, %struct.i64_2** %2, align 8
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @"?print5_2@@YAX$$QEAUi64_2@@@Z"(%struct.i64_2* dereferenceable(16) %0) #0 {
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

; Function Attrs: noinline optnone uwtable
define dso_local void @"?print10_2@@YA?AVSingle@@XZ"(%class.Single* noalias sret %0) #2 {
  %2 = alloca i8*, align 8
  %3 = bitcast %class.Single* %0 to i8*
  store i8* %3, i8** %2, align 8
  %4 = call %class.Single* @"??0Single@@QEAA@XZ"(%class.Single* %0)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %class.Single* @"??0Single@@QEAA@XZ"(%class.Single* returned %0) unnamed_addr #0 comdat align 2 {
  %2 = alloca %class.Single*, align 8
  store %class.Single* %0, %class.Single** %2, align 8
  %3 = load %class.Single*, %class.Single** %2, align 8
  ret %class.Single* %3
}

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
define dso_local i32 @main() #3 personality i8* bitcast (i32 (...)* @__CxxFrameHandler3 to i8*) {
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
  %20 = alloca %struct.i64_2, align 8
  %21 = alloca %struct.i64_2, align 8
  %22 = alloca %class.Single, align 8
  %23 = alloca %struct.i64_2, align 8
  %24 = alloca %struct.i64_2, align 8
  %25 = alloca %struct.i64_2*, align 8
  %26 = alloca %"class.std::basic_string", align 8
  %27 = alloca %"class.std::basic_string", align 8
  %28 = alloca %"class.std::basic_string", align 8
  %29 = alloca %"class.std::basic_string", align 8
  %30 = alloca %class.Single, align 8
  %31 = alloca %class.Single, align 8
  %32 = alloca %class.Single, align 8
  %33 = alloca %class.Single, align 8
  store i64 4, i64* %1, align 8
  store i64 7, i64* %2, align 8
  %34 = call i64 @"?add1@@YA_J_J0@Z"(i64 5, i64 6)
  %35 = load i64, i64* %2, align 8
  %36 = load i64, i64* %1, align 8
  %37 = call i64 @"?add1@@YA_J_J0@Z"(i64 %36, i64 %35)
  store i64 %37, i64* %3, align 8
  %38 = call i64 @"?add2@@YA_JAEA_J0@Z"(i64* dereferenceable(8) %1, i64* dereferenceable(8) %2)
  store i64 %38, i64* %4, align 8
  %39 = call i64 @"?add3@@YA_JPEA_J0@Z"(i64* %1, i64* %2)
  store i64 %39, i64* %5, align 8
  %40 = call %class.Single* @"??0Single@@QEAA@XZ"(%class.Single* %6)
  %41 = bitcast %class.Point* %7 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %41, i8 0, i64 32, i1 false)
  %42 = bitcast %class.Point* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %42, i8* align 8 bitcast (%class.Point* @__const.main.pa to i8*), i64 32, i1 false)
  %43 = bitcast %class.Point* %9 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %43, i8* align 8 bitcast (%class.Point* @__const.main.pb to i8*), i64 32, i1 false)
  %44 = bitcast %class.Point* %11 to i8*
  %45 = bitcast %class.Point* %9 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %44, i8* align 8 %45, i64 32, i1 false)
  %46 = bitcast %class.Point* %12 to i8*
  %47 = bitcast %class.Point* %8 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %46, i8* align 8 %47, i64 32, i1 false)
  call void @"?padd1@@YA?AVPoint@@V1@0@Z"(%class.Point* sret %10, %class.Point* %12, %class.Point* %11)
  call void @"?padd2@@YA?AVPoint@@AEAV1@0@Z"(%class.Point* sret %13, %class.Point* dereferenceable(32) %8, %class.Point* dereferenceable(32) %9)
  call void @"?padd3@@YA?AVPoint@@PEAV1@0@Z"(%class.Point* sret %14, %class.Point* %8, %class.Point* %9)
  %48 = call %class.Empty* @"??0Empty@@QEAA@XZ"(%class.Empty* %15)
  %49 = bitcast %struct.i64_2* %16 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %49, i8 0, i64 16, i1 false)
  call void @"?print@@YAXXZ"()
  call void @"?print1@@YAX_J@Z"(i64 0)
  call void @"?print2@@YAXAEA_J@Z"(i64* dereferenceable(8) %1)
  call void @"?print3@@YAXPEA_J@Z"(i64* %1)
  %50 = bitcast %struct.i64_2* %17 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %50, i8 0, i64 16, i1 false)
  call void @"?print4@@YAXUi64_2@@@Z"(%struct.i64_2* %17)
  %51 = bitcast %struct.i64_2* %18 to i8*
  %52 = bitcast %struct.i64_2* %16 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %51, i8* align 8 %52, i64 16, i1 false)
  call void @"?print4@@YAXUi64_2@@@Z"(%struct.i64_2* %18)
  %53 = bitcast %struct.i64_2* %19 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %53, i8 0, i64 16, i1 false)
  call void @"?print5_1@@YAXAEBUi64_2@@@Z"(%struct.i64_2* dereferenceable(16) %19)
  call void @"?print5_1@@YAXAEBUi64_2@@@Z"(%struct.i64_2* dereferenceable(16) %16)
  %54 = bitcast %struct.i64_2* %20 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %54, i8 0, i64 16, i1 false)
  call void @"?print5_2@@YAX$$QEAUi64_2@@@Z"(%struct.i64_2* dereferenceable(16) %20)
  call void @"?print5@@YAXAEAUi64_2@@@Z"(%struct.i64_2* dereferenceable(16) %16)
  call void @"?print6@@YAXPEAUi64_2@@@Z"(%struct.i64_2* %16)
  %55 = call i64 @"?print7@@YA_JXZ"()
  %56 = call i64* @"?print8@@YAPEA_JXZ"()
  %57 = call dereferenceable(8) i64* @"?print9@@YAAEA_JXZ"()
  call void @"?print10@@YA?AUi64_2@@XZ"(%struct.i64_2* sret %21)
  call void @"?print10_2@@YA?AVSingle@@XZ"(%class.Single* sret %22)
  %58 = call dereferenceable(16) %struct.i64_2* @"?print11@@YAAEAUi64_2@@XZ"()
  %59 = call %struct.i64_2* @"?print12@@YAPEAUi64_2@@XZ"()
  call void @"?Act@Single@@QEAAXXZ"(%class.Single* %6)
  call void @"?StaticAct@Single@@SAXXZ"()
  %60 = bitcast %struct.i64_2* %23 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %60, i8 0, i64 16, i1 false)
  %61 = getelementptr inbounds %struct.i64_2, %struct.i64_2* %23, i32 0, i32 0
  %62 = bitcast %struct.i64_2* %24 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %62, i8 0, i64 16, i1 false)
  %63 = getelementptr inbounds %struct.i64_2, %struct.i64_2* %24, i32 0, i32 0
  store i64 10, i64* %63, align 8
  store %struct.i64_2* %24, %struct.i64_2** %25, align 8
  %64 = load %struct.i64_2*, %struct.i64_2** %25, align 8
  %65 = getelementptr inbounds %struct.i64_2, %struct.i64_2* %64, i32 0, i32 0
  store i64 10, i64* %65, align 8
  %66 = call %"class.std::basic_string"* @"??0?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@QEBD@Z"(%"class.std::basic_string"* %26, i8* getelementptr inbounds ([6 x i8], [6 x i8]* @"??_C@_05NLDMIFDH@Hallo?$AA@", i64 0, i64 0))
  %67 = invoke %"class.std::basic_string"* @"??0?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@QEBD@Z"(%"class.std::basic_string"* %27, i8* getelementptr inbounds ([5 x i8], [5 x i8]* @"??_C@_04LFIIJCLO@Welt?$AA@", i64 0, i64 0))
          to label %68 unwind label %84

68:                                               ; preds = %0
  invoke void @"??$?HDU?$char_traits@D@std@@V?$allocator@D@1@@std@@YA?AV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@0@AEBV10@QEBD@Z"(%"class.std::basic_string"* sret %29, %"class.std::basic_string"* dereferenceable(32) %26, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @"??_C@_01CLKCMJKC@?5?$AA@", i64 0, i64 0))
          to label %69 unwind label %82

69:                                               ; preds = %68
  invoke void @"??$?HDU?$char_traits@D@std@@V?$allocator@D@1@@std@@YA?AV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@0@$$QEAV10@AEBV10@@Z"(%"class.std::basic_string"* sret %28, %"class.std::basic_string"* dereferenceable(32) %29, %"class.std::basic_string"* dereferenceable(32) %27)
          to label %70 unwind label %78

70:                                               ; preds = %69
  call void @"??1?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@XZ"(%"class.std::basic_string"* %29) #10
  %71 = invoke %class.Single* @"??0Single@@QEAA@XZ"(%class.Single* %30)
          to label %72 unwind label %80

72:                                               ; preds = %70
  %73 = invoke %class.Single* @"??0Single@@QEAA@XZ"(%class.Single* %32)
          to label %74 unwind label %80

74:                                               ; preds = %72
  invoke void @"?Create@Single@@QEAA?AV1@AEBV1@@Z"(%class.Single* %30, %class.Single* sret %31, %class.Single* dereferenceable(8) %32)
          to label %75 unwind label %80

75:                                               ; preds = %74
  %76 = bitcast %class.Single* %33 to i8*
  %77 = bitcast %class.Single* %30 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 8 %76, i8* align 8 %77, i64 8, i1 false)
  call void @"??1?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@XZ"(%"class.std::basic_string"* %28) #10
  call void @"??1?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@XZ"(%"class.std::basic_string"* %27) #10
  call void @"??1?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@XZ"(%"class.std::basic_string"* %26) #10
  ret i32 0

78:                                               ; preds = %69
  %79 = cleanuppad within none []
  call void @"??1?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@XZ"(%"class.std::basic_string"* %29) #10 [ "funclet"(token %79) ]
  cleanupret from %79 unwind label %82

80:                                               ; preds = %74, %72, %70
  %81 = cleanuppad within none []
  call void @"??1?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@XZ"(%"class.std::basic_string"* %28) #10 [ "funclet"(token %81) ]
  cleanupret from %81 unwind label %82

82:                                               ; preds = %80, %78, %68
  %83 = cleanuppad within none []
  call void @"??1?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@XZ"(%"class.std::basic_string"* %27) #10 [ "funclet"(token %83) ]
  cleanupret from %83 unwind label %84

84:                                               ; preds = %82, %0
  %85 = cleanuppad within none []
  call void @"??1?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@XZ"(%"class.std::basic_string"* %26) #10 [ "funclet"(token %85) ]
  cleanupret from %85 unwind to caller
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
define linkonce_odr dso_local void @"?Act@Single@@QEAAXXZ"(%class.Single* %0) #2 comdat align 2 {
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

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local %"class.std::basic_string"* @"??0?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@QEBD@Z"(%"class.std::basic_string"* returned %0, i8* %1) unnamed_addr #2 comdat align 2 personality i8* bitcast (i32 (...)* @__CxxFrameHandler3 to i8*) {
  %3 = alloca i8*, align 8
  %4 = alloca %"class.std::basic_string"*, align 8
  %5 = alloca %"struct.std::_Zero_then_variadic_args_t", align 1
  %6 = alloca %"struct.std::_Fake_allocator"*, align 8
  %7 = alloca %"struct.std::_Fake_allocator", align 1
  %8 = alloca %"struct.std::_Fake_proxy_ptr_impl", align 1
  store i8* %1, i8** %3, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %4, align 8
  %9 = load %"class.std::basic_string"*, %"class.std::basic_string"** %4, align 8
  %10 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %9, i32 0, i32 0
  %11 = getelementptr inbounds %"struct.std::_Zero_then_variadic_args_t", %"struct.std::_Zero_then_variadic_args_t"* %5, i32 0, i32 0
  %12 = load i8, i8* %11, align 1
  %13 = call %"class.std::_Compressed_pair"* @"??$?0$$V@?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAA@U_Zero_then_variadic_args_t@1@@Z"(%"class.std::_Compressed_pair"* %10, i8 %12)
  store %"struct.std::_Fake_allocator"* %7, %"struct.std::_Fake_allocator"** %6, align 8
  %14 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %9, i32 0, i32 0
  %15 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %14, i32 0, i32 0
  %16 = bitcast %"class.std::_String_val"* %15 to %"struct.std::_Container_base0"*
  %17 = load %"struct.std::_Fake_allocator"*, %"struct.std::_Fake_allocator"** %6, align 8
  %18 = call %"struct.std::_Fake_proxy_ptr_impl"* @"??0_Fake_proxy_ptr_impl@std@@QEAA@AEBU_Fake_allocator@1@AEBU_Container_base0@1@@Z"(%"struct.std::_Fake_proxy_ptr_impl"* %8, %"struct.std::_Fake_allocator"* dereferenceable(1) %17, %"struct.std::_Container_base0"* dereferenceable(1) %16) #10
  call void @"?_Tidy_init@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAXXZ"(%"class.std::basic_string"* %9) #10
  %19 = load i8*, i8** %3, align 8
  %20 = invoke dereferenceable(32) %"class.std::basic_string"* @"?assign@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV12@QEBD@Z"(%"class.std::basic_string"* %9, i8* %19)
          to label %21 unwind label %22

21:                                               ; preds = %2
  call void @"?_Release@_Fake_proxy_ptr_impl@std@@QEAAXXZ"(%"struct.std::_Fake_proxy_ptr_impl"* %8) #10
  ret %"class.std::basic_string"* %9

22:                                               ; preds = %2
  %23 = cleanuppad within none []
  call void @"??1?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAA@XZ"(%"class.std::_Compressed_pair"* %10) #10 [ "funclet"(token %23) ]
  cleanupret from %23 unwind to caller
}

declare dso_local i32 @__CxxFrameHandler3(...)

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local void @"??$?HDU?$char_traits@D@std@@V?$allocator@D@1@@std@@YA?AV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@0@$$QEAV10@AEBV10@@Z"(%"class.std::basic_string"* noalias sret %0, %"class.std::basic_string"* dereferenceable(32) %1, %"class.std::basic_string"* dereferenceable(32) %2) #2 comdat {
  %4 = alloca i8*, align 8
  %5 = alloca %"class.std::basic_string"*, align 8
  %6 = alloca %"class.std::basic_string"*, align 8
  %7 = bitcast %"class.std::basic_string"* %0 to i8*
  store i8* %7, i8** %4, align 8
  store %"class.std::basic_string"* %2, %"class.std::basic_string"** %5, align 8
  store %"class.std::basic_string"* %1, %"class.std::basic_string"** %6, align 8
  %8 = load %"class.std::basic_string"*, %"class.std::basic_string"** %6, align 8
  %9 = load %"class.std::basic_string"*, %"class.std::basic_string"** %5, align 8
  %10 = call dereferenceable(32) %"class.std::basic_string"* @"?append@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV12@AEBV12@@Z"(%"class.std::basic_string"* %8, %"class.std::basic_string"* dereferenceable(32) %9)
  %11 = call dereferenceable(32) %"class.std::basic_string"* @"??$move@AEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@std@@YA$$QEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@0@AEAV10@@Z"(%"class.std::basic_string"* dereferenceable(32) %10) #10
  %12 = call %"class.std::basic_string"* @"??0?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@$$QEAV01@@Z"(%"class.std::basic_string"* %0, %"class.std::basic_string"* dereferenceable(32) %11) #10
  ret void
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local void @"??$?HDU?$char_traits@D@std@@V?$allocator@D@1@@std@@YA?AV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@0@AEBV10@QEBD@Z"(%"class.std::basic_string"* noalias sret %0, %"class.std::basic_string"* dereferenceable(32) %1, i8* %2) #2 comdat {
  %4 = alloca i8*, align 8
  %5 = alloca i8*, align 8
  %6 = alloca %"class.std::basic_string"*, align 8
  %7 = alloca i64, align 8
  %8 = alloca i64, align 8
  %9 = alloca %"struct.std::_String_constructor_concat_tag", align 1
  %10 = bitcast %"class.std::basic_string"* %0 to i8*
  store i8* %10, i8** %4, align 8
  store i8* %2, i8** %5, align 8
  store %"class.std::basic_string"* %1, %"class.std::basic_string"** %6, align 8
  %11 = load %"class.std::basic_string"*, %"class.std::basic_string"** %6, align 8
  %12 = call i64 @"?size@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEBA_KXZ"(%"class.std::basic_string"* %11) #10
  store i64 %12, i64* %7, align 8
  %13 = load i8*, i8** %5, align 8
  %14 = call i64 @"?length@?$_Narrow_char_traits@DH@std@@SA_KQEBD@Z"(i8* %13) #10
  %15 = call i64 @"??$_Convert_size@_K@std@@YA_K_K@Z"(i64 %14) #10
  store i64 %15, i64* %8, align 8
  %16 = load %"class.std::basic_string"*, %"class.std::basic_string"** %6, align 8
  %17 = call i64 @"?max_size@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEBA_KXZ"(%"class.std::basic_string"* %16) #10
  %18 = load i64, i64* %7, align 8
  %19 = sub i64 %17, %18
  %20 = load i64, i64* %8, align 8
  %21 = icmp ult i64 %19, %20
  br i1 %21, label %22, label %23

22:                                               ; preds = %3
  call void @"?_Xlen_string@std@@YAXXZ"() #11
  unreachable

23:                                               ; preds = %3
  %24 = load %"class.std::basic_string"*, %"class.std::basic_string"** %6, align 8
  %25 = load %"class.std::basic_string"*, %"class.std::basic_string"** %6, align 8
  %26 = call i8* @"?c_str@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEBAPEBDXZ"(%"class.std::basic_string"* %25) #10
  %27 = load i64, i64* %7, align 8
  %28 = load i8*, i8** %5, align 8
  %29 = load i64, i64* %8, align 8
  %30 = getelementptr inbounds %"struct.std::_String_constructor_concat_tag", %"struct.std::_String_constructor_concat_tag"* %9, i32 0, i32 0
  %31 = load i8, i8* %30, align 1
  %32 = call %"class.std::basic_string"* @"??0?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@U_String_constructor_concat_tag@1@AEBV01@QEBD_K23@Z"(%"class.std::basic_string"* %0, i8 %31, %"class.std::basic_string"* dereferenceable(32) %24, i8* %26, i64 %27, i8* %28, i64 %29)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"??1?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@XZ"(%"class.std::basic_string"* %0) unnamed_addr #0 comdat align 2 {
  %2 = alloca %"class.std::basic_string"*, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %2, align 8
  %3 = load %"class.std::basic_string"*, %"class.std::basic_string"** %2, align 8
  call void @"?_Tidy_deallocate@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAXXZ"(%"class.std::basic_string"* %3) #10
  %4 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %3, i32 0, i32 0
  call void @"??1?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAA@XZ"(%"class.std::_Compressed_pair"* %4) #10
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"?Create@Single@@QEAA?AV1@AEBV1@@Z"(%class.Single* %0, %class.Single* noalias sret %1, %class.Single* dereferenceable(8) %2) #0 comdat align 2 {
  %4 = alloca i8*, align 8
  %5 = alloca %class.Single*, align 8
  %6 = alloca %class.Single*, align 8
  %7 = bitcast %class.Single* %1 to i8*
  store i8* %7, i8** %4, align 8
  store %class.Single* %2, %class.Single** %5, align 8
  store %class.Single* %0, %class.Single** %6, align 8
  %8 = load %class.Single*, %class.Single** %6, align 8
  %9 = call %class.Single* @"??0Single@@QEAA@XZ"(%class.Single* %1)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"?_Tidy_deallocate@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAXXZ"(%"class.std::basic_string"* %0) #0 comdat align 2 personality i8* bitcast (i32 (...)* @__CxxFrameHandler3 to i8*) {
  %2 = alloca %"class.std::basic_string"*, align 8
  %3 = alloca i8*, align 8
  %4 = alloca %"class.std::allocator"*, align 8
  %5 = alloca i8, align 1
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %2, align 8
  %6 = load %"class.std::basic_string"*, %"class.std::basic_string"** %2, align 8
  %7 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %6, i32 0, i32 0
  %8 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %7, i32 0, i32 0
  %9 = bitcast %"class.std::_String_val"* %8 to %"struct.std::_Container_base0"*
  call void @"?_Orphan_all@_Container_base0@std@@QEAAXXZ"(%"struct.std::_Container_base0"* %9) #10
  %10 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %6, i32 0, i32 0
  %11 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %10, i32 0, i32 0
  %12 = call zeroext i1 @"?_Large_string_engaged@?$_String_val@U?$_Simple_types@D@std@@@std@@QEBA_NXZ"(%"class.std::_String_val"* %11) #10
  br i1 %12, label %13, label %32

13:                                               ; preds = %1
  %14 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %6, i32 0, i32 0
  %15 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %14, i32 0, i32 0
  %16 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %15, i32 0, i32 0
  %17 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %16 to i8**
  %18 = load i8*, i8** %17, align 8
  store i8* %18, i8** %3, align 8
  %19 = call dereferenceable(1) %"class.std::allocator"* @"?_Getal@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAAEAV?$allocator@D@2@XZ"(%"class.std::basic_string"* %6) #10
  store %"class.std::allocator"* %19, %"class.std::allocator"** %4, align 8
  %20 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %6, i32 0, i32 0
  %21 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %20, i32 0, i32 0
  %22 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %21, i32 0, i32 0
  %23 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %22 to i8**
  call void @"??$_Destroy_in_place@PEAD@std@@YAXAEAPEAD@Z"(i8** dereferenceable(8) %23) #10
  %24 = load %"class.std::allocator"*, %"class.std::allocator"** %4, align 8
  %25 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %6, i32 0, i32 0
  %26 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %25, i32 0, i32 0
  %27 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %26, i32 0, i32 2
  %28 = load i64, i64* %27, align 8
  %29 = add i64 %28, 1
  %30 = load i8*, i8** %3, align 8
  invoke void @"?deallocate@?$allocator@D@std@@QEAAXQEAD_K@Z"(%"class.std::allocator"* %24, i8* %30, i64 %29)
          to label %31 unwind label %44

31:                                               ; preds = %13
  br label %32

32:                                               ; preds = %31, %1
  %33 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %6, i32 0, i32 0
  %34 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %33, i32 0, i32 0
  %35 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %34, i32 0, i32 1
  store i64 0, i64* %35, align 8
  %36 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %6, i32 0, i32 0
  %37 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %36, i32 0, i32 0
  %38 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %37, i32 0, i32 2
  store i64 15, i64* %38, align 8
  store i8 0, i8* %5, align 1
  %39 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %6, i32 0, i32 0
  %40 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %39, i32 0, i32 0
  %41 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %40, i32 0, i32 0
  %42 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %41 to [16 x i8]*
  %43 = getelementptr inbounds [16 x i8], [16 x i8]* %42, i64 0, i64 0
  call void @"?assign@?$_Narrow_char_traits@DH@std@@SAXAEADAEBD@Z"(i8* dereferenceable(1) %43, i8* dereferenceable(1) %5) #10
  ret void

44:                                               ; preds = %13
  %45 = cleanuppad within none []
  call void @__std_terminate() #12 [ "funclet"(token %45) ]
  unreachable
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"??1?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAA@XZ"(%"class.std::_Compressed_pair"* %0) unnamed_addr #0 comdat align 2 {
  %2 = alloca %"class.std::_Compressed_pair"*, align 8
  store %"class.std::_Compressed_pair"* %0, %"class.std::_Compressed_pair"** %2, align 8
  %3 = load %"class.std::_Compressed_pair"*, %"class.std::_Compressed_pair"** %2, align 8
  %4 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %3, i32 0, i32 0
  call void @"??1?$_String_val@U?$_Simple_types@D@std@@@std@@QEAA@XZ"(%"class.std::_String_val"* %4) #10
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"?_Orphan_all@_Container_base0@std@@QEAAXXZ"(%"struct.std::_Container_base0"* %0) #0 comdat align 2 {
  %2 = alloca %"struct.std::_Container_base0"*, align 8
  store %"struct.std::_Container_base0"* %0, %"struct.std::_Container_base0"** %2, align 8
  %3 = load %"struct.std::_Container_base0"*, %"struct.std::_Container_base0"** %2, align 8
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local zeroext i1 @"?_Large_string_engaged@?$_String_val@U?$_Simple_types@D@std@@@std@@QEBA_NXZ"(%"class.std::_String_val"* %0) #0 comdat align 2 {
  %2 = alloca %"class.std::_String_val"*, align 8
  store %"class.std::_String_val"* %0, %"class.std::_String_val"** %2, align 8
  %3 = load %"class.std::_String_val"*, %"class.std::_String_val"** %2, align 8
  %4 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %3, i32 0, i32 2
  %5 = load i64, i64* %4, align 8
  %6 = icmp ule i64 16, %5
  ret i1 %6
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local dereferenceable(1) %"class.std::allocator"* @"?_Getal@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAAEAV?$allocator@D@2@XZ"(%"class.std::basic_string"* %0) #0 comdat align 2 {
  %2 = alloca %"class.std::basic_string"*, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %2, align 8
  %3 = load %"class.std::basic_string"*, %"class.std::basic_string"** %2, align 8
  %4 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %3, i32 0, i32 0
  %5 = call dereferenceable(1) %"class.std::allocator"* @"?_Get_first@?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAAAEAV?$allocator@D@2@XZ"(%"class.std::_Compressed_pair"* %4) #10
  ret %"class.std::allocator"* %5
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"??$_Destroy_in_place@PEAD@std@@YAXAEAPEAD@Z"(i8** dereferenceable(8) %0) #0 comdat {
  %2 = alloca i8**, align 8
  store i8** %0, i8*** %2, align 8
  %3 = load i8**, i8*** %2, align 8
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"?deallocate@?$allocator@D@std@@QEAAXQEAD_K@Z"(%"class.std::allocator"* %0, i8* %1, i64 %2) #0 comdat align 2 {
  %4 = alloca i64, align 8
  %5 = alloca i8*, align 8
  %6 = alloca %"class.std::allocator"*, align 8
  store i64 %2, i64* %4, align 8
  store i8* %1, i8** %5, align 8
  store %"class.std::allocator"* %0, %"class.std::allocator"** %6, align 8
  %7 = load %"class.std::allocator"*, %"class.std::allocator"** %6, align 8
  %8 = load i64, i64* %4, align 8
  %9 = mul i64 1, %8
  %10 = load i8*, i8** %5, align 8
  call void @"??$_Deallocate@$0BA@$0A@@std@@YAXPEAX_K@Z"(i8* %10, i64 %9) #10
  ret void
}

declare dso_local void @__std_terminate()

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"?assign@?$_Narrow_char_traits@DH@std@@SAXAEADAEBD@Z"(i8* dereferenceable(1) %0, i8* dereferenceable(1) %1) #0 comdat align 2 {
  %3 = alloca i8*, align 8
  %4 = alloca i8*, align 8
  store i8* %1, i8** %3, align 8
  store i8* %0, i8** %4, align 8
  %5 = load i8*, i8** %3, align 8
  %6 = load i8, i8* %5, align 1
  %7 = load i8*, i8** %4, align 8
  store i8 %6, i8* %7, align 1
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local dereferenceable(1) %"class.std::allocator"* @"?_Get_first@?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAAAEAV?$allocator@D@2@XZ"(%"class.std::_Compressed_pair"* %0) #0 comdat align 2 {
  %2 = alloca %"class.std::_Compressed_pair"*, align 8
  store %"class.std::_Compressed_pair"* %0, %"class.std::_Compressed_pair"** %2, align 8
  %3 = load %"class.std::_Compressed_pair"*, %"class.std::_Compressed_pair"** %2, align 8
  %4 = bitcast %"class.std::_Compressed_pair"* %3 to %"class.std::allocator"*
  ret %"class.std::allocator"* %4
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"??$_Deallocate@$0BA@$0A@@std@@YAXPEAX_K@Z"(i8* %0, i64 %1) #0 comdat personality i8* bitcast (i32 (...)* @__CxxFrameHandler3 to i8*) {
  %3 = alloca i64, align 8
  %4 = alloca i8*, align 8
  store i64 %1, i64* %3, align 8
  store i8* %0, i8** %4, align 8
  %5 = load i64, i64* %3, align 8
  %6 = icmp uge i64 %5, 4096
  br i1 %6, label %7, label %9

7:                                                ; preds = %2
  invoke void @"?_Adjust_manually_vector_aligned@std@@YAXAEAPEAXAEA_K@Z"(i8** dereferenceable(8) %4, i64* dereferenceable(8) %3)
          to label %8 unwind label %12

8:                                                ; preds = %7
  br label %9

9:                                                ; preds = %8, %2
  %10 = load i64, i64* %3, align 8
  %11 = load i8*, i8** %4, align 8
  call void @"??3@YAXPEAX_K@Z"(i8* %11, i64 %10) #10
  ret void

12:                                               ; preds = %7
  %13 = cleanuppad within none []
  call void @__std_terminate() #12 [ "funclet"(token %13) ]
  unreachable
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local void @"?_Adjust_manually_vector_aligned@std@@YAXAEAPEAXAEA_K@Z"(i8** dereferenceable(8) %0, i64* dereferenceable(8) %1) #2 comdat {
  %3 = alloca i64*, align 8
  %4 = alloca i8**, align 8
  %5 = alloca i64*, align 8
  %6 = alloca i64, align 8
  %7 = alloca i64, align 8
  %8 = alloca i64, align 8
  store i64* %1, i64** %3, align 8
  store i8** %0, i8*** %4, align 8
  %9 = load i64*, i64** %3, align 8
  %10 = load i64, i64* %9, align 8
  %11 = add i64 %10, 39
  store i64 %11, i64* %9, align 8
  %12 = load i8**, i8*** %4, align 8
  %13 = load i8*, i8** %12, align 8
  %14 = bitcast i8* %13 to i64*
  store i64* %14, i64** %5, align 8
  %15 = load i64*, i64** %5, align 8
  %16 = getelementptr inbounds i64, i64* %15, i64 -1
  %17 = load i64, i64* %16, align 8
  store i64 %17, i64* %6, align 8
  store i64 8, i64* %7, align 8
  %18 = load i8**, i8*** %4, align 8
  %19 = load i8*, i8** %18, align 8
  %20 = ptrtoint i8* %19 to i64
  %21 = load i64, i64* %6, align 8
  %22 = sub i64 %20, %21
  store i64 %22, i64* %8, align 8
  br label %23

23:                                               ; preds = %2
  %24 = load i64, i64* %8, align 8
  %25 = icmp uge i64 %24, 8
  br i1 %25, label %26, label %30

26:                                               ; preds = %23
  %27 = load i64, i64* %8, align 8
  %28 = icmp ule i64 %27, 39
  br i1 %28, label %29, label %30

29:                                               ; preds = %26
  br label %33

30:                                               ; preds = %26, %23
  br label %31

31:                                               ; preds = %30
  call void @_invalid_parameter_noinfo_noreturn() #11
  unreachable

32:                                               ; No predecessors!
  br label %33

33:                                               ; preds = %32, %29
  br label %34

34:                                               ; preds = %33
  %35 = load i64, i64* %6, align 8
  %36 = inttoptr i64 %35 to i8*
  %37 = load i8**, i8*** %4, align 8
  store i8* %36, i8** %37, align 8
  ret void
}

; Function Attrs: nounwind
declare dso_local void @"??3@YAXPEAX_K@Z"(i8*, i64) #4

; Function Attrs: noreturn
declare dso_local void @_invalid_parameter_noinfo_noreturn() #5

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"??1?$_String_val@U?$_Simple_types@D@std@@@std@@QEAA@XZ"(%"class.std::_String_val"* %0) unnamed_addr #0 comdat align 2 {
  %2 = alloca %"class.std::_String_val"*, align 8
  store %"class.std::_String_val"* %0, %"class.std::_String_val"** %2, align 8
  %3 = load %"class.std::_String_val"*, %"class.std::_String_val"** %2, align 8
  %4 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %3, i32 0, i32 0
  call void @"??1_Bxty@?$_String_val@U?$_Simple_types@D@std@@@std@@QEAA@XZ"(%"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %4) #10
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"??1_Bxty@?$_String_val@U?$_Simple_types@D@std@@@std@@QEAA@XZ"(%"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %0) unnamed_addr #0 comdat align 2 {
  %2 = alloca %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"*, align 8
  store %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %0, %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"** %2, align 8
  %3 = load %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"*, %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"** %2, align 8
  ret void
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local %"class.std::_Compressed_pair"* @"??$?0$$V@?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAA@U_Zero_then_variadic_args_t@1@@Z"(%"class.std::_Compressed_pair"* returned %0, i8 %1) unnamed_addr #2 comdat align 2 {
  %3 = alloca %"struct.std::_Zero_then_variadic_args_t", align 1
  %4 = alloca %"class.std::_Compressed_pair"*, align 8
  %5 = getelementptr inbounds %"struct.std::_Zero_then_variadic_args_t", %"struct.std::_Zero_then_variadic_args_t"* %3, i32 0, i32 0
  store i8 %1, i8* %5, align 1
  store %"class.std::_Compressed_pair"* %0, %"class.std::_Compressed_pair"** %4, align 8
  %6 = load %"class.std::_Compressed_pair"*, %"class.std::_Compressed_pair"** %4, align 8
  %7 = bitcast %"class.std::_Compressed_pair"* %6 to %"class.std::allocator"*
  %8 = call %"class.std::allocator"* @"??0?$allocator@D@std@@QEAA@XZ"(%"class.std::allocator"* %7) #10
  %9 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %6, i32 0, i32 0
  %10 = call %"class.std::_String_val"* @"??0?$_String_val@U?$_Simple_types@D@std@@@std@@QEAA@XZ"(%"class.std::_String_val"* %9)
  ret %"class.std::_Compressed_pair"* %6
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %"struct.std::_Fake_proxy_ptr_impl"* @"??0_Fake_proxy_ptr_impl@std@@QEAA@AEBU_Fake_allocator@1@AEBU_Container_base0@1@@Z"(%"struct.std::_Fake_proxy_ptr_impl"* returned %0, %"struct.std::_Fake_allocator"* dereferenceable(1) %1, %"struct.std::_Container_base0"* dereferenceable(1) %2) unnamed_addr #0 comdat align 2 {
  %4 = alloca %"struct.std::_Container_base0"*, align 8
  %5 = alloca %"struct.std::_Fake_allocator"*, align 8
  %6 = alloca %"struct.std::_Fake_proxy_ptr_impl"*, align 8
  store %"struct.std::_Container_base0"* %2, %"struct.std::_Container_base0"** %4, align 8
  store %"struct.std::_Fake_allocator"* %1, %"struct.std::_Fake_allocator"** %5, align 8
  store %"struct.std::_Fake_proxy_ptr_impl"* %0, %"struct.std::_Fake_proxy_ptr_impl"** %6, align 8
  %7 = load %"struct.std::_Fake_proxy_ptr_impl"*, %"struct.std::_Fake_proxy_ptr_impl"** %6, align 8
  ret %"struct.std::_Fake_proxy_ptr_impl"* %7
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"?_Tidy_init@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAXXZ"(%"class.std::basic_string"* %0) #0 comdat align 2 {
  %2 = alloca %"class.std::basic_string"*, align 8
  %3 = alloca i8, align 1
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %2, align 8
  %4 = load %"class.std::basic_string"*, %"class.std::basic_string"** %2, align 8
  %5 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %4, i32 0, i32 0
  %6 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %5, i32 0, i32 0
  %7 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %6, i32 0, i32 1
  store i64 0, i64* %7, align 8
  %8 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %4, i32 0, i32 0
  %9 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %8, i32 0, i32 0
  %10 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %9, i32 0, i32 2
  store i64 15, i64* %10, align 8
  store i8 0, i8* %3, align 1
  %11 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %4, i32 0, i32 0
  %12 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %11, i32 0, i32 0
  %13 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %12, i32 0, i32 0
  %14 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %13 to [16 x i8]*
  %15 = getelementptr inbounds [16 x i8], [16 x i8]* %14, i64 0, i64 0
  call void @"?assign@?$_Narrow_char_traits@DH@std@@SAXAEADAEBD@Z"(i8* dereferenceable(1) %15, i8* dereferenceable(1) %3) #10
  ret void
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local dereferenceable(32) %"class.std::basic_string"* @"?assign@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV12@QEBD@Z"(%"class.std::basic_string"* %0, i8* %1) #2 comdat align 2 {
  %3 = alloca i8*, align 8
  %4 = alloca %"class.std::basic_string"*, align 8
  store i8* %1, i8** %3, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %4, align 8
  %5 = load %"class.std::basic_string"*, %"class.std::basic_string"** %4, align 8
  %6 = load i8*, i8** %3, align 8
  %7 = call i64 @"?length@?$_Narrow_char_traits@DH@std@@SA_KQEBD@Z"(i8* %6) #10
  %8 = call i64 @"??$_Convert_size@_K@std@@YA_K_K@Z"(i64 %7) #10
  %9 = load i8*, i8** %3, align 8
  %10 = call dereferenceable(32) %"class.std::basic_string"* @"?assign@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV12@QEBD_K@Z"(%"class.std::basic_string"* %5, i8* %9, i64 %8)
  ret %"class.std::basic_string"* %10
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"?_Release@_Fake_proxy_ptr_impl@std@@QEAAXXZ"(%"struct.std::_Fake_proxy_ptr_impl"* %0) #0 comdat align 2 {
  %2 = alloca %"struct.std::_Fake_proxy_ptr_impl"*, align 8
  store %"struct.std::_Fake_proxy_ptr_impl"* %0, %"struct.std::_Fake_proxy_ptr_impl"** %2, align 8
  %3 = load %"struct.std::_Fake_proxy_ptr_impl"*, %"struct.std::_Fake_proxy_ptr_impl"** %2, align 8
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %"class.std::allocator"* @"??0?$allocator@D@std@@QEAA@XZ"(%"class.std::allocator"* returned %0) unnamed_addr #0 comdat align 2 {
  %2 = alloca %"class.std::allocator"*, align 8
  store %"class.std::allocator"* %0, %"class.std::allocator"** %2, align 8
  %3 = load %"class.std::allocator"*, %"class.std::allocator"** %2, align 8
  ret %"class.std::allocator"* %3
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local %"class.std::_String_val"* @"??0?$_String_val@U?$_Simple_types@D@std@@@std@@QEAA@XZ"(%"class.std::_String_val"* returned %0) unnamed_addr #2 comdat align 2 {
  %2 = alloca %"class.std::_String_val"*, align 8
  store %"class.std::_String_val"* %0, %"class.std::_String_val"** %2, align 8
  %3 = load %"class.std::_String_val"*, %"class.std::_String_val"** %2, align 8
  %4 = bitcast %"class.std::_String_val"* %3 to %"struct.std::_Container_base0"*
  %5 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %3, i32 0, i32 0
  %6 = call %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* @"??0_Bxty@?$_String_val@U?$_Simple_types@D@std@@@std@@QEAA@XZ"(%"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %5)
  %7 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %3, i32 0, i32 1
  store i64 0, i64* %7, align 8
  %8 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %3, i32 0, i32 2
  store i64 0, i64* %8, align 8
  ret %"class.std::_String_val"* %3
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* @"??0_Bxty@?$_String_val@U?$_Simple_types@D@std@@@std@@QEAA@XZ"(%"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* returned %0) unnamed_addr #0 comdat align 2 {
  %2 = alloca %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"*, align 8
  store %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %0, %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"** %2, align 8
  %3 = load %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"*, %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"** %2, align 8
  ret %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %3
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local dereferenceable(32) %"class.std::basic_string"* @"?assign@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV12@QEBD_K@Z"(%"class.std::basic_string"* %0, i8* %1, i64 %2) #2 comdat align 2 {
  %4 = alloca %"class.std::basic_string"*, align 8
  %5 = alloca i64, align 8
  %6 = alloca i8*, align 8
  %7 = alloca %"class.std::basic_string"*, align 8
  %8 = alloca i8*, align 8
  %9 = alloca i8, align 1
  %10 = alloca %class.anon, align 1
  store i64 %2, i64* %5, align 8
  store i8* %1, i8** %6, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %7, align 8
  %11 = load %"class.std::basic_string"*, %"class.std::basic_string"** %7, align 8
  %12 = load i64, i64* %5, align 8
  %13 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %11, i32 0, i32 0
  %14 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %13, i32 0, i32 0
  %15 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %14, i32 0, i32 2
  %16 = load i64, i64* %15, align 8
  %17 = icmp ule i64 %12, %16
  br i1 %17, label %18, label %33

18:                                               ; preds = %3
  %19 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %11, i32 0, i32 0
  %20 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %19, i32 0, i32 0
  %21 = call i8* @"?_Myptr@?$_String_val@U?$_Simple_types@D@std@@@std@@QEAAPEADXZ"(%"class.std::_String_val"* %20) #10
  store i8* %21, i8** %8, align 8
  %22 = load i64, i64* %5, align 8
  %23 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %11, i32 0, i32 0
  %24 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %23, i32 0, i32 0
  %25 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %24, i32 0, i32 1
  store i64 %22, i64* %25, align 8
  %26 = load i64, i64* %5, align 8
  %27 = load i8*, i8** %6, align 8
  %28 = load i8*, i8** %8, align 8
  %29 = call i8* @"?move@?$_Narrow_char_traits@DH@std@@SAPEADQEADQEBD_K@Z"(i8* %28, i8* %27, i64 %26) #10
  store i8 0, i8* %9, align 1
  %30 = load i8*, i8** %8, align 8
  %31 = load i64, i64* %5, align 8
  %32 = getelementptr inbounds i8, i8* %30, i64 %31
  call void @"?assign@?$_Narrow_char_traits@DH@std@@SAXAEADAEBD@Z"(i8* dereferenceable(1) %32, i8* dereferenceable(1) %9) #10
  store %"class.std::basic_string"* %11, %"class.std::basic_string"** %4, align 8
  br label %39

33:                                               ; preds = %3
  %34 = load i8*, i8** %6, align 8
  %35 = load i64, i64* %5, align 8
  %36 = getelementptr inbounds %class.anon, %class.anon* %10, i32 0, i32 0
  %37 = load i8, i8* %36, align 1
  %38 = call dereferenceable(32) %"class.std::basic_string"* @"??$_Reallocate_for@V<lambda_1>@?0??assign@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV34@QEBD_K@Z@PEBD@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAAEAV01@_KV<lambda_1>@?0??assign@01@QEAAAEAV01@QEBD0@Z@PEBD@Z"(%"class.std::basic_string"* %11, i64 %35, i8 %37, i8* %34)
  store %"class.std::basic_string"* %38, %"class.std::basic_string"** %4, align 8
  br label %39

39:                                               ; preds = %33, %18
  %40 = load %"class.std::basic_string"*, %"class.std::basic_string"** %4, align 8
  ret %"class.std::basic_string"* %40
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i64 @"??$_Convert_size@_K@std@@YA_K_K@Z"(i64 %0) #0 comdat {
  %2 = alloca i64, align 8
  store i64 %0, i64* %2, align 8
  %3 = load i64, i64* %2, align 8
  ret i64 %3
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i64 @"?length@?$_Narrow_char_traits@DH@std@@SA_KQEBD@Z"(i8* %0) #0 comdat align 2 personality i8* bitcast (i32 (...)* @__CxxFrameHandler3 to i8*) {
  %2 = alloca i8*, align 8
  store i8* %0, i8** %2, align 8
  %3 = load i8*, i8** %2, align 8
  %4 = invoke i64 @strlen(i8* %3)
          to label %5 unwind label %6

5:                                                ; preds = %1
  ret i64 %4

6:                                                ; preds = %1
  %7 = cleanuppad within none []
  call void @__std_terminate() #12 [ "funclet"(token %7) ]
  unreachable
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i8* @"?_Myptr@?$_String_val@U?$_Simple_types@D@std@@@std@@QEAAPEADXZ"(%"class.std::_String_val"* %0) #0 comdat align 2 {
  %2 = alloca %"class.std::_String_val"*, align 8
  %3 = alloca i8*, align 8
  store %"class.std::_String_val"* %0, %"class.std::_String_val"** %2, align 8
  %4 = load %"class.std::_String_val"*, %"class.std::_String_val"** %2, align 8
  %5 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %4, i32 0, i32 0
  %6 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %5 to [16 x i8]*
  %7 = getelementptr inbounds [16 x i8], [16 x i8]* %6, i64 0, i64 0
  store i8* %7, i8** %3, align 8
  %8 = call zeroext i1 @"?_Large_string_engaged@?$_String_val@U?$_Simple_types@D@std@@@std@@QEBA_NXZ"(%"class.std::_String_val"* %4) #10
  br i1 %8, label %9, label %14

9:                                                ; preds = %1
  %10 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %4, i32 0, i32 0
  %11 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %10 to i8**
  %12 = load i8*, i8** %11, align 8
  %13 = call i8* @"??$_Unfancy@D@std@@YAPEADPEAD@Z"(i8* %12) #10
  store i8* %13, i8** %3, align 8
  br label %14

14:                                               ; preds = %9, %1
  %15 = load i8*, i8** %3, align 8
  ret i8* %15
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i8* @"?move@?$_Narrow_char_traits@DH@std@@SAPEADQEADQEBD_K@Z"(i8* %0, i8* %1, i64 %2) #0 comdat align 2 {
  %4 = alloca i64, align 8
  %5 = alloca i8*, align 8
  %6 = alloca i8*, align 8
  store i64 %2, i64* %4, align 8
  store i8* %1, i8** %5, align 8
  store i8* %0, i8** %6, align 8
  %7 = load i8*, i8** %6, align 8
  %8 = load i8*, i8** %5, align 8
  %9 = load i64, i64* %4, align 8
  call void @llvm.memmove.p0i8.p0i8.i64(i8* align 1 %7, i8* align 1 %8, i64 %9, i1 false)
  ret i8* %7
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local dereferenceable(32) %"class.std::basic_string"* @"??$_Reallocate_for@V<lambda_1>@?0??assign@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV34@QEBD_K@Z@PEBD@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAAEAV01@_KV<lambda_1>@?0??assign@01@QEAAAEAV01@QEBD0@Z@PEBD@Z"(%"class.std::basic_string"* %0, i64 %1, i8 %2, i8* %3) #2 comdat align 2 {
  %5 = alloca %class.anon, align 1
  %6 = alloca i8*, align 8
  %7 = alloca i64, align 8
  %8 = alloca %"class.std::basic_string"*, align 8
  %9 = alloca i64, align 8
  %10 = alloca i64, align 8
  %11 = alloca %"class.std::allocator"*, align 8
  %12 = alloca i8*, align 8
  %13 = getelementptr inbounds %class.anon, %class.anon* %5, i32 0, i32 0
  store i8 %2, i8* %13, align 1
  store i8* %3, i8** %6, align 8
  store i64 %1, i64* %7, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %8, align 8
  %14 = load %"class.std::basic_string"*, %"class.std::basic_string"** %8, align 8
  %15 = load i64, i64* %7, align 8
  %16 = call i64 @"?max_size@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEBA_KXZ"(%"class.std::basic_string"* %14) #10
  %17 = icmp ugt i64 %15, %16
  br i1 %17, label %18, label %19

18:                                               ; preds = %4
  call void @"?_Xlen_string@std@@YAXXZ"() #11
  unreachable

19:                                               ; preds = %4
  %20 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %14, i32 0, i32 0
  %21 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %20, i32 0, i32 0
  %22 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %21, i32 0, i32 2
  %23 = load i64, i64* %22, align 8
  store i64 %23, i64* %9, align 8
  %24 = load i64, i64* %7, align 8
  %25 = call i64 @"?_Calculate_growth@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEBA_K_K@Z"(%"class.std::basic_string"* %14, i64 %24) #10
  store i64 %25, i64* %10, align 8
  %26 = call dereferenceable(1) %"class.std::allocator"* @"?_Getal@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAAEAV?$allocator@D@2@XZ"(%"class.std::basic_string"* %14) #10
  store %"class.std::allocator"* %26, %"class.std::allocator"** %11, align 8
  %27 = load %"class.std::allocator"*, %"class.std::allocator"** %11, align 8
  %28 = load i64, i64* %10, align 8
  %29 = add i64 %28, 1
  %30 = call i8* @"?allocate@?$allocator@D@std@@QEAAPEAD_K@Z"(%"class.std::allocator"* %27, i64 %29)
  store i8* %30, i8** %12, align 8
  %31 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %14, i32 0, i32 0
  %32 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %31, i32 0, i32 0
  %33 = bitcast %"class.std::_String_val"* %32 to %"struct.std::_Container_base0"*
  call void @"?_Orphan_all@_Container_base0@std@@QEAAXXZ"(%"struct.std::_Container_base0"* %33) #10
  %34 = load i64, i64* %7, align 8
  %35 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %14, i32 0, i32 0
  %36 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %35, i32 0, i32 0
  %37 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %36, i32 0, i32 1
  store i64 %34, i64* %37, align 8
  %38 = load i64, i64* %10, align 8
  %39 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %14, i32 0, i32 0
  %40 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %39, i32 0, i32 0
  %41 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %40, i32 0, i32 2
  store i64 %38, i64* %41, align 8
  %42 = load i8*, i8** %6, align 8
  %43 = load i64, i64* %7, align 8
  %44 = load i8*, i8** %12, align 8
  %45 = call i8* @"??$_Unfancy@D@std@@YAPEADPEAD@Z"(i8* %44) #10
  call void @"??R<lambda_1>@?0??assign@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV23@QEBD_K@Z@QEBA?A?<auto>@@QEAD10@Z"(%class.anon* %5, i8* %45, i64 %43, i8* %42)
  %46 = load i64, i64* %9, align 8
  %47 = icmp ule i64 16, %46
  br i1 %47, label %48, label %62

48:                                               ; preds = %19
  %49 = load %"class.std::allocator"*, %"class.std::allocator"** %11, align 8
  %50 = load i64, i64* %9, align 8
  %51 = add i64 %50, 1
  %52 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %14, i32 0, i32 0
  %53 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %52, i32 0, i32 0
  %54 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %53, i32 0, i32 0
  %55 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %54 to i8**
  %56 = load i8*, i8** %55, align 8
  call void @"?deallocate@?$allocator@D@std@@QEAAXQEAD_K@Z"(%"class.std::allocator"* %49, i8* %56, i64 %51)
  %57 = load i8*, i8** %12, align 8
  %58 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %14, i32 0, i32 0
  %59 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %58, i32 0, i32 0
  %60 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %59, i32 0, i32 0
  %61 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %60 to i8**
  store i8* %57, i8** %61, align 8
  br label %67

62:                                               ; preds = %19
  %63 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %14, i32 0, i32 0
  %64 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %63, i32 0, i32 0
  %65 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %64, i32 0, i32 0
  %66 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %65 to i8**
  call void @"??$_Construct_in_place@PEADAEBQEAD@std@@YAXAEAPEADAEBQEAD@Z"(i8** dereferenceable(8) %66, i8** dereferenceable(8) %12) #10
  br label %67

67:                                               ; preds = %62, %48
  ret %"class.std::basic_string"* %14
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i8* @"??$_Unfancy@D@std@@YAPEADPEAD@Z"(i8* %0) #0 comdat {
  %2 = alloca i8*, align 8
  store i8* %0, i8** %2, align 8
  %3 = load i8*, i8** %2, align 8
  ret i8* %3
}

; Function Attrs: argmemonly nounwind willreturn
declare void @llvm.memmove.p0i8.p0i8.i64(i8* nocapture, i8* nocapture readonly, i64, i1 immarg) #1

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i64 @"?max_size@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEBA_KXZ"(%"class.std::basic_string"* %0) #0 comdat align 2 {
  %2 = alloca %"class.std::basic_string"*, align 8
  %3 = alloca i64, align 8
  %4 = alloca i64, align 8
  %5 = alloca i64, align 8
  %6 = alloca i64, align 8
  %7 = alloca i64, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %2, align 8
  %8 = load %"class.std::basic_string"*, %"class.std::basic_string"** %2, align 8
  %9 = call dereferenceable(1) %"class.std::allocator"* @"?_Getal@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEBAAEBV?$allocator@D@2@XZ"(%"class.std::basic_string"* %8) #10
  %10 = call i64 @"?max_size@?$_Default_allocator_traits@V?$allocator@D@std@@@std@@SA_KAEBV?$allocator@D@2@@Z"(%"class.std::allocator"* dereferenceable(1) %9) #10
  store i64 %10, i64* %3, align 8
  store i64 16, i64* %5, align 8
  %11 = call dereferenceable(8) i64* @"??$_Max_value@_K@std@@YAAEB_KAEB_K0@Z"(i64* dereferenceable(8) %3, i64* dereferenceable(8) %5) #10
  %12 = load i64, i64* %11, align 8
  store i64 %12, i64* %4, align 8
  %13 = load i64, i64* %4, align 8
  %14 = sub i64 %13, 1
  store i64 %14, i64* %6, align 8
  %15 = call i64 @"?max@?$numeric_limits@_J@std@@SA_JXZ"() #10
  store i64 %15, i64* %7, align 8
  %16 = call dereferenceable(8) i64* @"??$_Min_value@_K@std@@YAAEB_KAEB_K0@Z"(i64* dereferenceable(8) %7, i64* dereferenceable(8) %6) #10
  %17 = load i64, i64* %16, align 8
  ret i64 %17
}

; Function Attrs: noinline noreturn optnone uwtable
define linkonce_odr dso_local void @"?_Xlen_string@std@@YAXXZ"() #6 comdat {
  call void @"?_Xlength_error@std@@YAXPEBD@Z"(i8* getelementptr inbounds ([16 x i8], [16 x i8]* @"??_C@_0BA@JFNIOLAK@string?5too?5long?$AA@", i64 0, i64 0)) #11
  unreachable
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i64 @"?_Calculate_growth@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEBA_K_K@Z"(%"class.std::basic_string"* %0, i64 %1) #0 comdat align 2 {
  %3 = alloca i64, align 8
  %4 = alloca %"class.std::basic_string"*, align 8
  store i64 %1, i64* %3, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %4, align 8
  %5 = load %"class.std::basic_string"*, %"class.std::basic_string"** %4, align 8
  %6 = call i64 @"?max_size@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEBA_KXZ"(%"class.std::basic_string"* %5) #10
  %7 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %5, i32 0, i32 0
  %8 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %7, i32 0, i32 0
  %9 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %8, i32 0, i32 2
  %10 = load i64, i64* %9, align 8
  %11 = load i64, i64* %3, align 8
  %12 = call i64 @"?_Calculate_growth@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@CA_K_K00@Z"(i64 %11, i64 %10, i64 %6) #10
  ret i64 %12
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local i8* @"?allocate@?$allocator@D@std@@QEAAPEAD_K@Z"(%"class.std::allocator"* %0, i64 %1) #2 comdat align 2 {
  %3 = alloca i64, align 8
  %4 = alloca %"class.std::allocator"*, align 8
  store i64 %1, i64* %3, align 8
  store %"class.std::allocator"* %0, %"class.std::allocator"** %4, align 8
  %5 = load %"class.std::allocator"*, %"class.std::allocator"** %4, align 8
  %6 = load i64, i64* %3, align 8
  %7 = call i64 @"??$_Get_size_of_n@$00@std@@YA_K_K@Z"(i64 %6)
  %8 = call i8* @"??$_Allocate@$0BA@U_Default_allocate_traits@std@@$0A@@std@@YAPEAX_K@Z"(i64 %7)
  ret i8* %8
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"??R<lambda_1>@?0??assign@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV23@QEBD_K@Z@QEBA?A?<auto>@@QEAD10@Z"(%class.anon* %0, i8* %1, i64 %2, i8* %3) #0 comdat align 2 {
  %5 = alloca i8*, align 8
  %6 = alloca i64, align 8
  %7 = alloca i8*, align 8
  %8 = alloca %class.anon*, align 8
  %9 = alloca i8, align 1
  store i8* %3, i8** %5, align 8
  store i64 %2, i64* %6, align 8
  store i8* %1, i8** %7, align 8
  store %class.anon* %0, %class.anon** %8, align 8
  %10 = load %class.anon*, %class.anon** %8, align 8
  %11 = load i64, i64* %6, align 8
  %12 = load i8*, i8** %5, align 8
  %13 = load i8*, i8** %7, align 8
  %14 = call i8* @"?copy@?$_Narrow_char_traits@DH@std@@SAPEADQEADQEBD_K@Z"(i8* %13, i8* %12, i64 %11) #10
  store i8 0, i8* %9, align 1
  %15 = load i8*, i8** %7, align 8
  %16 = load i64, i64* %6, align 8
  %17 = getelementptr inbounds i8, i8* %15, i64 %16
  call void @"?assign@?$_Narrow_char_traits@DH@std@@SAXAEADAEBD@Z"(i8* dereferenceable(1) %17, i8* dereferenceable(1) %9) #10
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"??$_Construct_in_place@PEADAEBQEAD@std@@YAXAEAPEADAEBQEAD@Z"(i8** dereferenceable(8) %0, i8** dereferenceable(8) %1) #0 comdat {
  %3 = alloca i8**, align 8
  %4 = alloca i8**, align 8
  store i8** %1, i8*** %3, align 8
  store i8** %0, i8*** %4, align 8
  %5 = load i8**, i8*** %4, align 8
  %6 = call i8** @"??$addressof@PEAD@std@@YAPEAPEADAEAPEAD@Z"(i8** dereferenceable(8) %5) #10
  %7 = bitcast i8** %6 to i8*
  %8 = bitcast i8* %7 to i8**
  %9 = load i8**, i8*** %3, align 8
  %10 = call dereferenceable(8) i8** @"??$forward@AEBQEAD@std@@YAAEBQEADAEBQEAD@Z"(i8** dereferenceable(8) %9) #10
  %11 = load i8*, i8** %10, align 8
  store i8* %11, i8** %8, align 8
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i64 @"?max_size@?$_Default_allocator_traits@V?$allocator@D@std@@@std@@SA_KAEBV?$allocator@D@2@@Z"(%"class.std::allocator"* dereferenceable(1) %0) #0 comdat align 2 {
  %2 = alloca %"class.std::allocator"*, align 8
  store %"class.std::allocator"* %0, %"class.std::allocator"** %2, align 8
  ret i64 -1
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local dereferenceable(1) %"class.std::allocator"* @"?_Getal@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEBAAEBV?$allocator@D@2@XZ"(%"class.std::basic_string"* %0) #0 comdat align 2 {
  %2 = alloca %"class.std::basic_string"*, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %2, align 8
  %3 = load %"class.std::basic_string"*, %"class.std::basic_string"** %2, align 8
  %4 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %3, i32 0, i32 0
  %5 = call dereferenceable(1) %"class.std::allocator"* @"?_Get_first@?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEBAAEBV?$allocator@D@2@XZ"(%"class.std::_Compressed_pair"* %4) #10
  ret %"class.std::allocator"* %5
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local dereferenceable(8) i64* @"??$_Max_value@_K@std@@YAAEB_KAEB_K0@Z"(i64* dereferenceable(8) %0, i64* dereferenceable(8) %1) #0 comdat {
  %3 = alloca i64*, align 8
  %4 = alloca i64*, align 8
  store i64* %1, i64** %3, align 8
  store i64* %0, i64** %4, align 8
  %5 = load i64*, i64** %4, align 8
  %6 = load i64, i64* %5, align 8
  %7 = load i64*, i64** %3, align 8
  %8 = load i64, i64* %7, align 8
  %9 = icmp ult i64 %6, %8
  br i1 %9, label %10, label %12

10:                                               ; preds = %2
  %11 = load i64*, i64** %3, align 8
  br label %14

12:                                               ; preds = %2
  %13 = load i64*, i64** %4, align 8
  br label %14

14:                                               ; preds = %12, %10
  %15 = phi i64* [ %11, %10 ], [ %13, %12 ]
  ret i64* %15
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local dereferenceable(8) i64* @"??$_Min_value@_K@std@@YAAEB_KAEB_K0@Z"(i64* dereferenceable(8) %0, i64* dereferenceable(8) %1) #0 comdat {
  %3 = alloca i64*, align 8
  %4 = alloca i64*, align 8
  store i64* %1, i64** %3, align 8
  store i64* %0, i64** %4, align 8
  %5 = load i64*, i64** %3, align 8
  %6 = load i64, i64* %5, align 8
  %7 = load i64*, i64** %4, align 8
  %8 = load i64, i64* %7, align 8
  %9 = icmp ult i64 %6, %8
  br i1 %9, label %10, label %12

10:                                               ; preds = %2
  %11 = load i64*, i64** %3, align 8
  br label %14

12:                                               ; preds = %2
  %13 = load i64*, i64** %4, align 8
  br label %14

14:                                               ; preds = %12, %10
  %15 = phi i64* [ %11, %10 ], [ %13, %12 ]
  ret i64* %15
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i64 @"?max@?$numeric_limits@_J@std@@SA_JXZ"() #0 comdat align 2 {
  ret i64 9223372036854775807
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local dereferenceable(1) %"class.std::allocator"* @"?_Get_first@?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEBAAEBV?$allocator@D@2@XZ"(%"class.std::_Compressed_pair"* %0) #0 comdat align 2 {
  %2 = alloca %"class.std::_Compressed_pair"*, align 8
  store %"class.std::_Compressed_pair"* %0, %"class.std::_Compressed_pair"** %2, align 8
  %3 = load %"class.std::_Compressed_pair"*, %"class.std::_Compressed_pair"** %2, align 8
  %4 = bitcast %"class.std::_Compressed_pair"* %3 to %"class.std::allocator"*
  ret %"class.std::allocator"* %4
}

; Function Attrs: noreturn
declare dso_local void @"?_Xlength_error@std@@YAXPEBD@Z"(i8*) #5

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i64 @"?_Calculate_growth@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@CA_K_K00@Z"(i64 %0, i64 %1, i64 %2) #0 comdat align 2 {
  %4 = alloca i64, align 8
  %5 = alloca i64, align 8
  %6 = alloca i64, align 8
  %7 = alloca i64, align 8
  %8 = alloca i64, align 8
  %9 = alloca i64, align 8
  store i64 %2, i64* %5, align 8
  store i64 %1, i64* %6, align 8
  store i64 %0, i64* %7, align 8
  %10 = load i64, i64* %7, align 8
  %11 = or i64 %10, 15
  store i64 %11, i64* %8, align 8
  %12 = load i64, i64* %8, align 8
  %13 = load i64, i64* %5, align 8
  %14 = icmp ugt i64 %12, %13
  br i1 %14, label %15, label %17

15:                                               ; preds = %3
  %16 = load i64, i64* %5, align 8
  store i64 %16, i64* %4, align 8
  br label %33

17:                                               ; preds = %3
  %18 = load i64, i64* %6, align 8
  %19 = load i64, i64* %5, align 8
  %20 = load i64, i64* %6, align 8
  %21 = udiv i64 %20, 2
  %22 = sub i64 %19, %21
  %23 = icmp ugt i64 %18, %22
  br i1 %23, label %24, label %26

24:                                               ; preds = %17
  %25 = load i64, i64* %5, align 8
  store i64 %25, i64* %4, align 8
  br label %33

26:                                               ; preds = %17
  %27 = load i64, i64* %6, align 8
  %28 = load i64, i64* %6, align 8
  %29 = udiv i64 %28, 2
  %30 = add i64 %27, %29
  store i64 %30, i64* %9, align 8
  %31 = call dereferenceable(8) i64* @"??$_Max_value@_K@std@@YAAEB_KAEB_K0@Z"(i64* dereferenceable(8) %8, i64* dereferenceable(8) %9) #10
  %32 = load i64, i64* %31, align 8
  store i64 %32, i64* %4, align 8
  br label %33

33:                                               ; preds = %26, %24, %15
  %34 = load i64, i64* %4, align 8
  ret i64 %34
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local i8* @"??$_Allocate@$0BA@U_Default_allocate_traits@std@@$0A@@std@@YAPEAX_K@Z"(i64 %0) #2 comdat {
  %2 = alloca i8*, align 8
  %3 = alloca i64, align 8
  store i64 %0, i64* %3, align 8
  %4 = load i64, i64* %3, align 8
  %5 = icmp uge i64 %4, 4096
  br i1 %5, label %6, label %9

6:                                                ; preds = %1
  %7 = load i64, i64* %3, align 8
  %8 = call i8* @"??$_Allocate_manually_vector_aligned@U_Default_allocate_traits@std@@@std@@YAPEAX_K@Z"(i64 %7)
  store i8* %8, i8** %2, align 8
  br label %16

9:                                                ; preds = %1
  %10 = load i64, i64* %3, align 8
  %11 = icmp ne i64 %10, 0
  br i1 %11, label %12, label %15

12:                                               ; preds = %9
  %13 = load i64, i64* %3, align 8
  %14 = call i8* @"?_Allocate@_Default_allocate_traits@std@@SAPEAX_K@Z"(i64 %13)
  store i8* %14, i8** %2, align 8
  br label %16

15:                                               ; preds = %9
  store i8* null, i8** %2, align 8
  br label %16

16:                                               ; preds = %15, %12, %6
  %17 = load i8*, i8** %2, align 8
  ret i8* %17
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i64 @"??$_Get_size_of_n@$00@std@@YA_K_K@Z"(i64 %0) #0 comdat {
  %2 = alloca i64, align 8
  %3 = alloca i8, align 1
  store i64 %0, i64* %2, align 8
  store i8 0, i8* %3, align 1
  %4 = load i64, i64* %2, align 8
  %5 = mul i64 %4, 1
  ret i64 %5
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local i8* @"??$_Allocate_manually_vector_aligned@U_Default_allocate_traits@std@@@std@@YAPEAX_K@Z"(i64 %0) #2 comdat {
  %2 = alloca i64, align 8
  %3 = alloca i64, align 8
  %4 = alloca i64, align 8
  %5 = alloca i8*, align 8
  store i64 %0, i64* %2, align 8
  %6 = load i64, i64* %2, align 8
  %7 = add i64 39, %6
  store i64 %7, i64* %3, align 8
  %8 = load i64, i64* %3, align 8
  %9 = load i64, i64* %2, align 8
  %10 = icmp ule i64 %8, %9
  br i1 %10, label %11, label %12

11:                                               ; preds = %1
  call void @"?_Throw_bad_array_new_length@std@@YAXXZ"() #11
  unreachable

12:                                               ; preds = %1
  %13 = load i64, i64* %3, align 8
  %14 = call i8* @"?_Allocate@_Default_allocate_traits@std@@SAPEAX_K@Z"(i64 %13)
  %15 = ptrtoint i8* %14 to i64
  store i64 %15, i64* %4, align 8
  br label %16

16:                                               ; preds = %12
  %17 = load i64, i64* %4, align 8
  %18 = icmp ne i64 %17, 0
  br i1 %18, label %19, label %20

19:                                               ; preds = %16
  br label %23

20:                                               ; preds = %16
  br label %21

21:                                               ; preds = %20
  call void @_invalid_parameter_noinfo_noreturn() #11
  unreachable

22:                                               ; No predecessors!
  br label %23

23:                                               ; preds = %22, %19
  br label %24

24:                                               ; preds = %23
  %25 = load i64, i64* %4, align 8
  %26 = add i64 %25, 39
  %27 = and i64 %26, -32
  %28 = inttoptr i64 %27 to i8*
  store i8* %28, i8** %5, align 8
  %29 = load i64, i64* %4, align 8
  %30 = load i8*, i8** %5, align 8
  %31 = bitcast i8* %30 to i64*
  %32 = getelementptr inbounds i64, i64* %31, i64 -1
  store i64 %29, i64* %32, align 8
  %33 = load i8*, i8** %5, align 8
  ret i8* %33
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local i8* @"?_Allocate@_Default_allocate_traits@std@@SAPEAX_K@Z"(i64 %0) #2 comdat align 2 {
  %2 = alloca i64, align 8
  store i64 %0, i64* %2, align 8
  %3 = load i64, i64* %2, align 8
  %4 = call i8* @"??2@YAPEAX_K@Z"(i64 %3)
  ret i8* %4
}

; Function Attrs: noinline noreturn optnone uwtable
define linkonce_odr dso_local void @"?_Throw_bad_array_new_length@std@@YAXXZ"() #6 comdat {
  %1 = alloca %"class.std::bad_array_new_length", align 8
  %2 = call %"class.std::bad_array_new_length"* @"??0bad_array_new_length@std@@QEAA@XZ"(%"class.std::bad_array_new_length"* %1) #10
  %3 = bitcast %"class.std::bad_array_new_length"* %1 to i8*
  call void @_CxxThrowException(i8* %3, %eh.ThrowInfo* @"_TI3?AVbad_array_new_length@std@@") #11
  unreachable
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %"class.std::bad_array_new_length"* @"??0bad_array_new_length@std@@QEAA@XZ"(%"class.std::bad_array_new_length"* returned %0) unnamed_addr #0 comdat align 2 {
  %2 = alloca %"class.std::bad_array_new_length"*, align 8
  store %"class.std::bad_array_new_length"* %0, %"class.std::bad_array_new_length"** %2, align 8
  %3 = load %"class.std::bad_array_new_length"*, %"class.std::bad_array_new_length"** %2, align 8
  %4 = bitcast %"class.std::bad_array_new_length"* %3 to %"class.std::bad_alloc"*
  %5 = call %"class.std::bad_alloc"* @"??0bad_alloc@std@@AEAA@QEBD@Z"(%"class.std::bad_alloc"* %4, i8* getelementptr inbounds ([21 x i8], [21 x i8]* @"??_C@_0BF@KINCDENJ@bad?5array?5new?5length?$AA@", i64 0, i64 0)) #10
  %6 = bitcast %"class.std::bad_array_new_length"* %3 to i32 (...)***
  store i32 (...)** bitcast (i8** @"??_7bad_array_new_length@std@@6B@" to i32 (...)**), i32 (...)*** %6, align 8
  ret %"class.std::bad_array_new_length"* %3
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %"class.std::bad_array_new_length"* @"??0bad_array_new_length@std@@QEAA@AEBV01@@Z"(%"class.std::bad_array_new_length"* returned %0, %"class.std::bad_array_new_length"* dereferenceable(24) %1) unnamed_addr #0 comdat align 2 {
  %3 = alloca %"class.std::bad_array_new_length"*, align 8
  %4 = alloca %"class.std::bad_array_new_length"*, align 8
  store %"class.std::bad_array_new_length"* %1, %"class.std::bad_array_new_length"** %3, align 8
  store %"class.std::bad_array_new_length"* %0, %"class.std::bad_array_new_length"** %4, align 8
  %5 = load %"class.std::bad_array_new_length"*, %"class.std::bad_array_new_length"** %4, align 8
  %6 = bitcast %"class.std::bad_array_new_length"* %5 to %"class.std::bad_alloc"*
  %7 = load %"class.std::bad_array_new_length"*, %"class.std::bad_array_new_length"** %3, align 8
  %8 = bitcast %"class.std::bad_array_new_length"* %7 to %"class.std::bad_alloc"*
  %9 = call %"class.std::bad_alloc"* @"??0bad_alloc@std@@QEAA@AEBV01@@Z"(%"class.std::bad_alloc"* %6, %"class.std::bad_alloc"* dereferenceable(24) %8) #10
  %10 = bitcast %"class.std::bad_array_new_length"* %5 to i32 (...)***
  store i32 (...)** bitcast (i8** @"??_7bad_array_new_length@std@@6B@" to i32 (...)**), i32 (...)*** %10, align 8
  ret %"class.std::bad_array_new_length"* %5
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %"class.std::bad_alloc"* @"??0bad_alloc@std@@QEAA@AEBV01@@Z"(%"class.std::bad_alloc"* returned %0, %"class.std::bad_alloc"* dereferenceable(24) %1) unnamed_addr #0 comdat align 2 {
  %3 = alloca %"class.std::bad_alloc"*, align 8
  %4 = alloca %"class.std::bad_alloc"*, align 8
  store %"class.std::bad_alloc"* %1, %"class.std::bad_alloc"** %3, align 8
  store %"class.std::bad_alloc"* %0, %"class.std::bad_alloc"** %4, align 8
  %5 = load %"class.std::bad_alloc"*, %"class.std::bad_alloc"** %4, align 8
  %6 = bitcast %"class.std::bad_alloc"* %5 to %"class.std::exception"*
  %7 = load %"class.std::bad_alloc"*, %"class.std::bad_alloc"** %3, align 8
  %8 = bitcast %"class.std::bad_alloc"* %7 to %"class.std::exception"*
  %9 = call %"class.std::exception"* @"??0exception@std@@QEAA@AEBV01@@Z"(%"class.std::exception"* %6, %"class.std::exception"* dereferenceable(24) %8) #10
  %10 = bitcast %"class.std::bad_alloc"* %5 to i32 (...)***
  store i32 (...)** bitcast (i8** @"??_7bad_alloc@std@@6B@" to i32 (...)**), i32 (...)*** %10, align 8
  ret %"class.std::bad_alloc"* %5
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %"class.std::exception"* @"??0exception@std@@QEAA@AEBV01@@Z"(%"class.std::exception"* returned %0, %"class.std::exception"* dereferenceable(24) %1) unnamed_addr #0 comdat align 2 personality i8* bitcast (i32 (...)* @__CxxFrameHandler3 to i8*) {
  %3 = alloca %"class.std::exception"*, align 8
  %4 = alloca %"class.std::exception"*, align 8
  store %"class.std::exception"* %1, %"class.std::exception"** %3, align 8
  store %"class.std::exception"* %0, %"class.std::exception"** %4, align 8
  %5 = load %"class.std::exception"*, %"class.std::exception"** %4, align 8
  %6 = bitcast %"class.std::exception"* %5 to i32 (...)***
  store i32 (...)** bitcast (i8** @"??_7exception@std@@6B@" to i32 (...)**), i32 (...)*** %6, align 8
  %7 = getelementptr inbounds %"class.std::exception", %"class.std::exception"* %5, i32 0, i32 1
  %8 = bitcast %struct.__std_exception_data* %7 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %8, i8 0, i64 16, i1 false)
  %9 = getelementptr inbounds %"class.std::exception", %"class.std::exception"* %5, i32 0, i32 1
  %10 = load %"class.std::exception"*, %"class.std::exception"** %3, align 8
  %11 = getelementptr inbounds %"class.std::exception", %"class.std::exception"* %10, i32 0, i32 1
  invoke void @__std_exception_copy(%struct.__std_exception_data* %11, %struct.__std_exception_data* %9)
          to label %12 unwind label %13

12:                                               ; preds = %2
  ret %"class.std::exception"* %5

13:                                               ; preds = %2
  %14 = cleanuppad within none []
  call void @__std_terminate() #12 [ "funclet"(token %14) ]
  unreachable
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"??1bad_array_new_length@std@@UEAA@XZ"(%"class.std::bad_array_new_length"* %0) unnamed_addr #0 comdat align 2 {
  %2 = alloca %"class.std::bad_array_new_length"*, align 8
  store %"class.std::bad_array_new_length"* %0, %"class.std::bad_array_new_length"** %2, align 8
  %3 = load %"class.std::bad_array_new_length"*, %"class.std::bad_array_new_length"** %2, align 8
  %4 = bitcast %"class.std::bad_array_new_length"* %3 to %"class.std::bad_alloc"*
  call void @"??1bad_alloc@std@@UEAA@XZ"(%"class.std::bad_alloc"* %4) #10
  ret void
}

declare dso_local void @_CxxThrowException(i8*, %eh.ThrowInfo*)

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %"class.std::bad_alloc"* @"??0bad_alloc@std@@AEAA@QEBD@Z"(%"class.std::bad_alloc"* returned %0, i8* %1) unnamed_addr #0 comdat align 2 {
  %3 = alloca i8*, align 8
  %4 = alloca %"class.std::bad_alloc"*, align 8
  store i8* %1, i8** %3, align 8
  store %"class.std::bad_alloc"* %0, %"class.std::bad_alloc"** %4, align 8
  %5 = load %"class.std::bad_alloc"*, %"class.std::bad_alloc"** %4, align 8
  %6 = bitcast %"class.std::bad_alloc"* %5 to %"class.std::exception"*
  %7 = load i8*, i8** %3, align 8
  %8 = call %"class.std::exception"* @"??0exception@std@@QEAA@QEBDH@Z"(%"class.std::exception"* %6, i8* %7, i32 1) #10
  %9 = bitcast %"class.std::bad_alloc"* %5 to i32 (...)***
  store i32 (...)** bitcast (i8** @"??_7bad_alloc@std@@6B@" to i32 (...)**), i32 (...)*** %9, align 8
  ret %"class.std::bad_alloc"* %5
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i8* @"??_Gbad_array_new_length@std@@UEAAPEAXI@Z"(%"class.std::bad_array_new_length"* %0, i32 %1) unnamed_addr #0 comdat align 2 {
  %3 = alloca i8*, align 8
  %4 = alloca i32, align 4
  %5 = alloca %"class.std::bad_array_new_length"*, align 8
  store i32 %1, i32* %4, align 4
  store %"class.std::bad_array_new_length"* %0, %"class.std::bad_array_new_length"** %5, align 8
  %6 = load %"class.std::bad_array_new_length"*, %"class.std::bad_array_new_length"** %5, align 8
  %7 = bitcast %"class.std::bad_array_new_length"* %6 to i8*
  store i8* %7, i8** %3, align 8
  %8 = load i32, i32* %4, align 4
  call void @"??1bad_array_new_length@std@@UEAA@XZ"(%"class.std::bad_array_new_length"* %6) #10
  %9 = icmp eq i32 %8, 0
  br i1 %9, label %12, label %10

10:                                               ; preds = %2
  %11 = bitcast %"class.std::bad_array_new_length"* %6 to i8*
  call void @"??3@YAXPEAX@Z"(i8* %11) #13
  br label %12

12:                                               ; preds = %10, %2
  %13 = load i8*, i8** %3, align 8
  ret i8* %13
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i8* @"?what@exception@std@@UEBAPEBDXZ"(%"class.std::exception"* %0) unnamed_addr #0 comdat align 2 {
  %2 = alloca %"class.std::exception"*, align 8
  store %"class.std::exception"* %0, %"class.std::exception"** %2, align 8
  %3 = load %"class.std::exception"*, %"class.std::exception"** %2, align 8
  %4 = getelementptr inbounds %"class.std::exception", %"class.std::exception"* %3, i32 0, i32 1
  %5 = getelementptr inbounds %struct.__std_exception_data, %struct.__std_exception_data* %4, i32 0, i32 0
  %6 = load i8*, i8** %5, align 8
  %7 = icmp ne i8* %6, null
  br i1 %7, label %8, label %12

8:                                                ; preds = %1
  %9 = getelementptr inbounds %"class.std::exception", %"class.std::exception"* %3, i32 0, i32 1
  %10 = getelementptr inbounds %struct.__std_exception_data, %struct.__std_exception_data* %9, i32 0, i32 0
  %11 = load i8*, i8** %10, align 8
  br label %13

12:                                               ; preds = %1
  br label %13

13:                                               ; preds = %12, %8
  %14 = phi i8* [ %11, %8 ], [ getelementptr inbounds ([18 x i8], [18 x i8]* @"??_C@_0BC@EOODALEL@Unknown?5exception?$AA@", i64 0, i64 0), %12 ]
  ret i8* %14
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %"class.std::exception"* @"??0exception@std@@QEAA@QEBDH@Z"(%"class.std::exception"* returned %0, i8* %1, i32 %2) unnamed_addr #0 comdat align 2 {
  %4 = alloca i32, align 4
  %5 = alloca i8*, align 8
  %6 = alloca %"class.std::exception"*, align 8
  store i32 %2, i32* %4, align 4
  store i8* %1, i8** %5, align 8
  store %"class.std::exception"* %0, %"class.std::exception"** %6, align 8
  %7 = load %"class.std::exception"*, %"class.std::exception"** %6, align 8
  %8 = bitcast %"class.std::exception"* %7 to i32 (...)***
  store i32 (...)** bitcast (i8** @"??_7exception@std@@6B@" to i32 (...)**), i32 (...)*** %8, align 8
  %9 = getelementptr inbounds %"class.std::exception", %"class.std::exception"* %7, i32 0, i32 1
  %10 = bitcast %struct.__std_exception_data* %9 to i8*
  call void @llvm.memset.p0i8.i64(i8* align 8 %10, i8 0, i64 16, i1 false)
  %11 = load i8*, i8** %5, align 8
  %12 = getelementptr inbounds %"class.std::exception", %"class.std::exception"* %7, i32 0, i32 1
  %13 = getelementptr inbounds %struct.__std_exception_data, %struct.__std_exception_data* %12, i32 0, i32 0
  store i8* %11, i8** %13, align 8
  ret %"class.std::exception"* %7
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i8* @"??_Gbad_alloc@std@@UEAAPEAXI@Z"(%"class.std::bad_alloc"* %0, i32 %1) unnamed_addr #0 comdat align 2 {
  %3 = alloca i8*, align 8
  %4 = alloca i32, align 4
  %5 = alloca %"class.std::bad_alloc"*, align 8
  store i32 %1, i32* %4, align 4
  store %"class.std::bad_alloc"* %0, %"class.std::bad_alloc"** %5, align 8
  %6 = load %"class.std::bad_alloc"*, %"class.std::bad_alloc"** %5, align 8
  %7 = bitcast %"class.std::bad_alloc"* %6 to i8*
  store i8* %7, i8** %3, align 8
  %8 = load i32, i32* %4, align 4
  call void @"??1bad_alloc@std@@UEAA@XZ"(%"class.std::bad_alloc"* %6) #10
  %9 = icmp eq i32 %8, 0
  br i1 %9, label %12, label %10

10:                                               ; preds = %2
  %11 = bitcast %"class.std::bad_alloc"* %6 to i8*
  call void @"??3@YAXPEAX@Z"(i8* %11) #13
  br label %12

12:                                               ; preds = %10, %2
  %13 = load i8*, i8** %3, align 8
  ret i8* %13
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i8* @"??_Gexception@std@@UEAAPEAXI@Z"(%"class.std::exception"* %0, i32 %1) unnamed_addr #0 comdat align 2 {
  %3 = alloca i8*, align 8
  %4 = alloca i32, align 4
  %5 = alloca %"class.std::exception"*, align 8
  store i32 %1, i32* %4, align 4
  store %"class.std::exception"* %0, %"class.std::exception"** %5, align 8
  %6 = load %"class.std::exception"*, %"class.std::exception"** %5, align 8
  %7 = bitcast %"class.std::exception"* %6 to i8*
  store i8* %7, i8** %3, align 8
  %8 = load i32, i32* %4, align 4
  call void @"??1exception@std@@UEAA@XZ"(%"class.std::exception"* %6) #10
  %9 = icmp eq i32 %8, 0
  br i1 %9, label %12, label %10

10:                                               ; preds = %2
  %11 = bitcast %"class.std::exception"* %6 to i8*
  call void @"??3@YAXPEAX@Z"(i8* %11) #13
  br label %12

12:                                               ; preds = %10, %2
  %13 = load i8*, i8** %3, align 8
  ret i8* %13
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"??1exception@std@@UEAA@XZ"(%"class.std::exception"* %0) unnamed_addr #0 comdat align 2 personality i8* bitcast (i32 (...)* @__CxxFrameHandler3 to i8*) {
  %2 = alloca %"class.std::exception"*, align 8
  store %"class.std::exception"* %0, %"class.std::exception"** %2, align 8
  %3 = load %"class.std::exception"*, %"class.std::exception"** %2, align 8
  %4 = bitcast %"class.std::exception"* %3 to i32 (...)***
  store i32 (...)** bitcast (i8** @"??_7exception@std@@6B@" to i32 (...)**), i32 (...)*** %4, align 8
  %5 = getelementptr inbounds %"class.std::exception", %"class.std::exception"* %3, i32 0, i32 1
  invoke void @__std_exception_destroy(%struct.__std_exception_data* %5)
          to label %6 unwind label %7

6:                                                ; preds = %1
  ret void

7:                                                ; preds = %1
  %8 = cleanuppad within none []
  call void @__std_terminate() #12 [ "funclet"(token %8) ]
  unreachable
}

; Function Attrs: nobuiltin nounwind
declare dso_local void @"??3@YAXPEAX@Z"(i8*) #7

declare dso_local void @__std_exception_destroy(%struct.__std_exception_data*) #8

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"??1bad_alloc@std@@UEAA@XZ"(%"class.std::bad_alloc"* %0) unnamed_addr #0 comdat align 2 {
  %2 = alloca %"class.std::bad_alloc"*, align 8
  store %"class.std::bad_alloc"* %0, %"class.std::bad_alloc"** %2, align 8
  %3 = load %"class.std::bad_alloc"*, %"class.std::bad_alloc"** %2, align 8
  %4 = bitcast %"class.std::bad_alloc"* %3 to %"class.std::exception"*
  call void @"??1exception@std@@UEAA@XZ"(%"class.std::exception"* %4) #10
  ret void
}

declare dso_local void @__std_exception_copy(%struct.__std_exception_data*, %struct.__std_exception_data*) #8

; Function Attrs: nobuiltin
declare dso_local noalias i8* @"??2@YAPEAX_K@Z"(i64) #9

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i8* @"?copy@?$_Narrow_char_traits@DH@std@@SAPEADQEADQEBD_K@Z"(i8* %0, i8* %1, i64 %2) #0 comdat align 2 {
  %4 = alloca i64, align 8
  %5 = alloca i8*, align 8
  %6 = alloca i8*, align 8
  store i64 %2, i64* %4, align 8
  store i8* %1, i8** %5, align 8
  store i8* %0, i8** %6, align 8
  %7 = load i8*, i8** %6, align 8
  %8 = load i8*, i8** %5, align 8
  %9 = load i64, i64* %4, align 8
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 1 %7, i8* align 1 %8, i64 %9, i1 false)
  ret i8* %7
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i8** @"??$addressof@PEAD@std@@YAPEAPEADAEAPEAD@Z"(i8** dereferenceable(8) %0) #0 comdat {
  %2 = alloca i8**, align 8
  store i8** %0, i8*** %2, align 8
  %3 = load i8**, i8*** %2, align 8
  ret i8** %3
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local dereferenceable(8) i8** @"??$forward@AEBQEAD@std@@YAAEBQEADAEBQEAD@Z"(i8** dereferenceable(8) %0) #0 comdat {
  %2 = alloca i8**, align 8
  store i8** %0, i8*** %2, align 8
  %3 = load i8**, i8*** %2, align 8
  ret i8** %3
}

declare dso_local i64 @strlen(i8*) #8

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i64 @"?size@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEBA_KXZ"(%"class.std::basic_string"* %0) #0 comdat align 2 {
  %2 = alloca %"class.std::basic_string"*, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %2, align 8
  %3 = load %"class.std::basic_string"*, %"class.std::basic_string"** %2, align 8
  %4 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %3, i32 0, i32 0
  %5 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %4, i32 0, i32 0
  %6 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %5, i32 0, i32 1
  %7 = load i64, i64* %6, align 8
  ret i64 %7
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i8* @"?c_str@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEBAPEBDXZ"(%"class.std::basic_string"* %0) #0 comdat align 2 {
  %2 = alloca %"class.std::basic_string"*, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %2, align 8
  %3 = load %"class.std::basic_string"*, %"class.std::basic_string"** %2, align 8
  %4 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %3, i32 0, i32 0
  %5 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %4, i32 0, i32 0
  %6 = call i8* @"?_Myptr@?$_String_val@U?$_Simple_types@D@std@@@std@@QEBAPEBDXZ"(%"class.std::_String_val"* %5) #10
  ret i8* %6
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local %"class.std::basic_string"* @"??0?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@U_String_constructor_concat_tag@1@AEBV01@QEBD_K23@Z"(%"class.std::basic_string"* returned %0, i8 %1, %"class.std::basic_string"* dereferenceable(32) %2, i8* %3, i64 %4, i8* %5, i64 %6) unnamed_addr #2 comdat align 2 personality i8* bitcast (i32 (...)* @__CxxFrameHandler3 to i8*) {
  %8 = alloca %"class.std::basic_string"*, align 8
  %9 = alloca %"struct.std::_String_constructor_concat_tag", align 1
  %10 = alloca i64, align 8
  %11 = alloca i8*, align 8
  %12 = alloca i64, align 8
  %13 = alloca i8*, align 8
  %14 = alloca %"class.std::basic_string"*, align 8
  %15 = alloca %"class.std::basic_string"*, align 8
  %16 = alloca %"class.std::allocator", align 1
  %17 = alloca %"struct.std::_One_then_variadic_args_t", align 1
  %18 = alloca i64, align 8
  %19 = alloca i64, align 8
  %20 = alloca %"class.std::_String_val"*, align 8
  %21 = alloca i8*, align 8
  %22 = alloca %"struct.std::_Fake_allocator"*, align 8
  %23 = alloca %"struct.std::_Fake_allocator", align 1
  %24 = alloca %"struct.std::_Fake_proxy_ptr_impl", align 1
  %25 = alloca i8*, align 8
  %26 = alloca i8, align 1
  %27 = getelementptr inbounds %"struct.std::_String_constructor_concat_tag", %"struct.std::_String_constructor_concat_tag"* %9, i32 0, i32 0
  store i8 %1, i8* %27, align 1
  store i64 %6, i64* %10, align 8
  store i8* %5, i8** %11, align 8
  store i64 %4, i64* %12, align 8
  store i8* %3, i8** %13, align 8
  store %"class.std::basic_string"* %2, %"class.std::basic_string"** %14, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %15, align 8
  %28 = load %"class.std::basic_string"*, %"class.std::basic_string"** %15, align 8
  store %"class.std::basic_string"* %28, %"class.std::basic_string"** %8, align 8
  %29 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %28, i32 0, i32 0
  %30 = load %"class.std::basic_string"*, %"class.std::basic_string"** %14, align 8
  %31 = call dereferenceable(1) %"class.std::allocator"* @"?_Getal@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEBAAEBV?$allocator@D@2@XZ"(%"class.std::basic_string"* %30) #10
  call void @"?select_on_container_copy_construction@?$_Default_allocator_traits@V?$allocator@D@std@@@std@@SA?AV?$allocator@D@2@AEBV32@@Z"(%"class.std::allocator"* sret %16, %"class.std::allocator"* dereferenceable(1) %31)
  %32 = getelementptr inbounds %"struct.std::_One_then_variadic_args_t", %"struct.std::_One_then_variadic_args_t"* %17, i32 0, i32 0
  %33 = load i8, i8* %32, align 1
  %34 = call %"class.std::_Compressed_pair"* @"??$?0V?$allocator@D@std@@$$V@?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAA@U_One_then_variadic_args_t@1@$$QEAV?$allocator@D@1@@Z"(%"class.std::_Compressed_pair"* %29, i8 %33, %"class.std::allocator"* dereferenceable(1) %16)
  %35 = load i64, i64* %12, align 8
  %36 = load i64, i64* %10, align 8
  %37 = add i64 %35, %36
  store i64 %37, i64* %18, align 8
  store i64 15, i64* %19, align 8
  %38 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %28, i32 0, i32 0
  %39 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %38, i32 0, i32 0
  store %"class.std::_String_val"* %39, %"class.std::_String_val"** %20, align 8
  %40 = load %"class.std::_String_val"*, %"class.std::_String_val"** %20, align 8
  %41 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %40, i32 0, i32 0
  %42 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %41 to [16 x i8]*
  %43 = getelementptr inbounds [16 x i8], [16 x i8]* %42, i64 0, i64 0
  store i8* %43, i8** %21, align 8
  store %"struct.std::_Fake_allocator"* %23, %"struct.std::_Fake_allocator"** %22, align 8
  %44 = load %"class.std::_String_val"*, %"class.std::_String_val"** %20, align 8
  %45 = bitcast %"class.std::_String_val"* %44 to %"struct.std::_Container_base0"*
  %46 = load %"struct.std::_Fake_allocator"*, %"struct.std::_Fake_allocator"** %22, align 8
  %47 = call %"struct.std::_Fake_proxy_ptr_impl"* @"??0_Fake_proxy_ptr_impl@std@@QEAA@AEBU_Fake_allocator@1@AEBU_Container_base0@1@@Z"(%"struct.std::_Fake_proxy_ptr_impl"* %24, %"struct.std::_Fake_allocator"* dereferenceable(1) %46, %"struct.std::_Container_base0"* dereferenceable(1) %45) #10
  %48 = load i64, i64* %19, align 8
  %49 = load i64, i64* %18, align 8
  %50 = icmp ult i64 %48, %49
  br i1 %50, label %51, label %65

51:                                               ; preds = %7
  %52 = call i64 @"?max_size@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEBA_KXZ"(%"class.std::basic_string"* %28) #10
  %53 = load i64, i64* %18, align 8
  %54 = call i64 @"?_Calculate_growth@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@CA_K_K00@Z"(i64 %53, i64 15, i64 %52) #10
  store i64 %54, i64* %19, align 8
  %55 = call dereferenceable(1) %"class.std::allocator"* @"?_Getal@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAAEAV?$allocator@D@2@XZ"(%"class.std::basic_string"* %28) #10
  %56 = load i64, i64* %19, align 8
  %57 = add i64 %56, 1
  %58 = invoke i8* @"?allocate@?$allocator@D@std@@QEAAPEAD_K@Z"(%"class.std::allocator"* %55, i64 %57)
          to label %59 unwind label %86

59:                                               ; preds = %51
  store i8* %58, i8** %25, align 8
  %60 = load i8*, i8** %25, align 8
  %61 = call i8* @"??$_Unfancy@D@std@@YAPEADPEAD@Z"(i8* %60) #10
  store i8* %61, i8** %21, align 8
  %62 = load %"class.std::_String_val"*, %"class.std::_String_val"** %20, align 8
  %63 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %62, i32 0, i32 0
  %64 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %63 to i8**
  call void @"??$_Construct_in_place@PEADAEBQEAD@std@@YAXAEAPEADAEBQEAD@Z"(i8** dereferenceable(8) %64, i8** dereferenceable(8) %25) #10
  br label %65

65:                                               ; preds = %59, %7
  %66 = load i64, i64* %18, align 8
  %67 = load %"class.std::_String_val"*, %"class.std::_String_val"** %20, align 8
  %68 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %67, i32 0, i32 1
  store i64 %66, i64* %68, align 8
  %69 = load i64, i64* %19, align 8
  %70 = load %"class.std::_String_val"*, %"class.std::_String_val"** %20, align 8
  %71 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %70, i32 0, i32 2
  store i64 %69, i64* %71, align 8
  %72 = load i64, i64* %12, align 8
  %73 = load i8*, i8** %13, align 8
  %74 = load i8*, i8** %21, align 8
  %75 = call i8* @"?copy@?$_Narrow_char_traits@DH@std@@SAPEADQEADQEBD_K@Z"(i8* %74, i8* %73, i64 %72) #10
  %76 = load i64, i64* %10, align 8
  %77 = load i8*, i8** %11, align 8
  %78 = load i8*, i8** %21, align 8
  %79 = load i64, i64* %12, align 8
  %80 = getelementptr inbounds i8, i8* %78, i64 %79
  %81 = call i8* @"?copy@?$_Narrow_char_traits@DH@std@@SAPEADQEADQEBD_K@Z"(i8* %80, i8* %77, i64 %76) #10
  store i8 0, i8* %26, align 1
  %82 = load i8*, i8** %21, align 8
  %83 = load i64, i64* %18, align 8
  %84 = getelementptr inbounds i8, i8* %82, i64 %83
  call void @"?assign@?$_Narrow_char_traits@DH@std@@SAXAEADAEBD@Z"(i8* dereferenceable(1) %84, i8* dereferenceable(1) %26) #10
  call void @"?_Release@_Fake_proxy_ptr_impl@std@@QEAAXXZ"(%"struct.std::_Fake_proxy_ptr_impl"* %24) #10
  %85 = load %"class.std::basic_string"*, %"class.std::basic_string"** %8, align 8
  ret %"class.std::basic_string"* %85

86:                                               ; preds = %51
  %87 = cleanuppad within none []
  call void @"??1?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAA@XZ"(%"class.std::_Compressed_pair"* %29) #10 [ "funclet"(token %87) ]
  cleanupret from %87 unwind to caller
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i8* @"?_Myptr@?$_String_val@U?$_Simple_types@D@std@@@std@@QEBAPEBDXZ"(%"class.std::_String_val"* %0) #0 comdat align 2 {
  %2 = alloca %"class.std::_String_val"*, align 8
  %3 = alloca i8*, align 8
  store %"class.std::_String_val"* %0, %"class.std::_String_val"** %2, align 8
  %4 = load %"class.std::_String_val"*, %"class.std::_String_val"** %2, align 8
  %5 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %4, i32 0, i32 0
  %6 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %5 to [16 x i8]*
  %7 = getelementptr inbounds [16 x i8], [16 x i8]* %6, i64 0, i64 0
  store i8* %7, i8** %3, align 8
  %8 = call zeroext i1 @"?_Large_string_engaged@?$_String_val@U?$_Simple_types@D@std@@@std@@QEBA_NXZ"(%"class.std::_String_val"* %4) #10
  br i1 %8, label %9, label %14

9:                                                ; preds = %1
  %10 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %4, i32 0, i32 0
  %11 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %10 to i8**
  %12 = load i8*, i8** %11, align 8
  %13 = call i8* @"??$_Unfancy@D@std@@YAPEADPEAD@Z"(i8* %12) #10
  store i8* %13, i8** %3, align 8
  br label %14

14:                                               ; preds = %9, %1
  %15 = load i8*, i8** %3, align 8
  ret i8* %15
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"?select_on_container_copy_construction@?$_Default_allocator_traits@V?$allocator@D@std@@@std@@SA?AV?$allocator@D@2@AEBV32@@Z"(%"class.std::allocator"* noalias sret %0, %"class.std::allocator"* dereferenceable(1) %1) #0 comdat align 2 {
  %3 = alloca i8*, align 8
  %4 = alloca %"class.std::allocator"*, align 8
  %5 = bitcast %"class.std::allocator"* %0 to i8*
  store i8* %5, i8** %3, align 8
  store %"class.std::allocator"* %1, %"class.std::allocator"** %4, align 8
  %6 = load %"class.std::allocator"*, %"class.std::allocator"** %4, align 8
  ret void
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local %"class.std::_Compressed_pair"* @"??$?0V?$allocator@D@std@@$$V@?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAA@U_One_then_variadic_args_t@1@$$QEAV?$allocator@D@1@@Z"(%"class.std::_Compressed_pair"* returned %0, i8 %1, %"class.std::allocator"* dereferenceable(1) %2) unnamed_addr #2 comdat align 2 {
  %4 = alloca %"struct.std::_One_then_variadic_args_t", align 1
  %5 = alloca %"class.std::allocator"*, align 8
  %6 = alloca %"class.std::_Compressed_pair"*, align 8
  %7 = getelementptr inbounds %"struct.std::_One_then_variadic_args_t", %"struct.std::_One_then_variadic_args_t"* %4, i32 0, i32 0
  store i8 %1, i8* %7, align 1
  store %"class.std::allocator"* %2, %"class.std::allocator"** %5, align 8
  store %"class.std::_Compressed_pair"* %0, %"class.std::_Compressed_pair"** %6, align 8
  %8 = load %"class.std::_Compressed_pair"*, %"class.std::_Compressed_pair"** %6, align 8
  %9 = bitcast %"class.std::_Compressed_pair"* %8 to %"class.std::allocator"*
  %10 = load %"class.std::allocator"*, %"class.std::allocator"** %5, align 8
  %11 = call dereferenceable(1) %"class.std::allocator"* @"??$forward@V?$allocator@D@std@@@std@@YA$$QEAV?$allocator@D@0@AEAV10@@Z"(%"class.std::allocator"* dereferenceable(1) %10) #10
  %12 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %8, i32 0, i32 0
  %13 = call %"class.std::_String_val"* @"??0?$_String_val@U?$_Simple_types@D@std@@@std@@QEAA@XZ"(%"class.std::_String_val"* %12)
  ret %"class.std::_Compressed_pair"* %8
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local dereferenceable(1) %"class.std::allocator"* @"??$forward@V?$allocator@D@std@@@std@@YA$$QEAV?$allocator@D@0@AEAV10@@Z"(%"class.std::allocator"* dereferenceable(1) %0) #0 comdat {
  %2 = alloca %"class.std::allocator"*, align 8
  store %"class.std::allocator"* %0, %"class.std::allocator"** %2, align 8
  %3 = load %"class.std::allocator"*, %"class.std::allocator"** %2, align 8
  ret %"class.std::allocator"* %3
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local dereferenceable(32) %"class.std::basic_string"* @"??$move@AEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@std@@YA$$QEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@0@AEAV10@@Z"(%"class.std::basic_string"* dereferenceable(32) %0) #0 comdat {
  %2 = alloca %"class.std::basic_string"*, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %2, align 8
  %3 = load %"class.std::basic_string"*, %"class.std::basic_string"** %2, align 8
  ret %"class.std::basic_string"* %3
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local dereferenceable(32) %"class.std::basic_string"* @"?append@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV12@AEBV12@@Z"(%"class.std::basic_string"* %0, %"class.std::basic_string"* dereferenceable(32) %1) #2 comdat align 2 {
  %3 = alloca %"class.std::basic_string"*, align 8
  %4 = alloca %"class.std::basic_string"*, align 8
  store %"class.std::basic_string"* %1, %"class.std::basic_string"** %3, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %4, align 8
  %5 = load %"class.std::basic_string"*, %"class.std::basic_string"** %4, align 8
  %6 = load %"class.std::basic_string"*, %"class.std::basic_string"** %3, align 8
  %7 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %6, i32 0, i32 0
  %8 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %7, i32 0, i32 0
  %9 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %8, i32 0, i32 1
  %10 = load i64, i64* %9, align 8
  %11 = load %"class.std::basic_string"*, %"class.std::basic_string"** %3, align 8
  %12 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %11, i32 0, i32 0
  %13 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %12, i32 0, i32 0
  %14 = call i8* @"?_Myptr@?$_String_val@U?$_Simple_types@D@std@@@std@@QEBAPEBDXZ"(%"class.std::_String_val"* %13) #10
  %15 = call dereferenceable(32) %"class.std::basic_string"* @"?append@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV12@QEBD_K@Z"(%"class.std::basic_string"* %5, i8* %14, i64 %10)
  ret %"class.std::basic_string"* %15
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %"class.std::basic_string"* @"??0?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAA@$$QEAV01@@Z"(%"class.std::basic_string"* returned %0, %"class.std::basic_string"* dereferenceable(32) %1) unnamed_addr #0 comdat align 2 personality i8* bitcast (i32 (...)* @__CxxFrameHandler3 to i8*) {
  %3 = alloca %"class.std::basic_string"*, align 8
  %4 = alloca %"class.std::basic_string"*, align 8
  %5 = alloca %"struct.std::_One_then_variadic_args_t", align 1
  %6 = alloca %"struct.std::_Fake_allocator", align 1
  %7 = alloca %"struct.std::integral_constant", align 1
  store %"class.std::basic_string"* %1, %"class.std::basic_string"** %3, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %4, align 8
  %8 = load %"class.std::basic_string"*, %"class.std::basic_string"** %4, align 8
  %9 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %8, i32 0, i32 0
  %10 = load %"class.std::basic_string"*, %"class.std::basic_string"** %3, align 8
  %11 = call dereferenceable(1) %"class.std::allocator"* @"?_Getal@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAAEAV?$allocator@D@2@XZ"(%"class.std::basic_string"* %10) #10
  %12 = call dereferenceable(1) %"class.std::allocator"* @"??$move@AEAV?$allocator@D@std@@@std@@YA$$QEAV?$allocator@D@0@AEAV10@@Z"(%"class.std::allocator"* dereferenceable(1) %11) #10
  %13 = getelementptr inbounds %"struct.std::_One_then_variadic_args_t", %"struct.std::_One_then_variadic_args_t"* %5, i32 0, i32 0
  %14 = load i8, i8* %13, align 1
  %15 = invoke %"class.std::_Compressed_pair"* @"??$?0V?$allocator@D@std@@$$V@?$_Compressed_pair@V?$allocator@D@std@@V?$_String_val@U?$_Simple_types@D@std@@@2@$00@std@@QEAA@U_One_then_variadic_args_t@1@$$QEAV?$allocator@D@1@@Z"(%"class.std::_Compressed_pair"* %9, i8 %14, %"class.std::allocator"* dereferenceable(1) %12)
          to label %16 unwind label %23

16:                                               ; preds = %2
  %17 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %8, i32 0, i32 0
  %18 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %17, i32 0, i32 0
  %19 = bitcast %"class.std::_String_val"* %18 to %"struct.std::_Container_base0"*
  call void @"?_Alloc_proxy@_Container_base0@std@@QEAAXAEBU_Fake_allocator@2@@Z"(%"struct.std::_Container_base0"* %19, %"struct.std::_Fake_allocator"* dereferenceable(1) %6) #10
  %20 = load %"class.std::basic_string"*, %"class.std::basic_string"** %3, align 8
  %21 = getelementptr inbounds %"struct.std::integral_constant", %"struct.std::integral_constant"* %7, i32 0, i32 0
  %22 = load i8, i8* %21, align 1
  call void @"?_Take_contents@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAXAEAV12@U?$integral_constant@_N$00@2@@Z"(%"class.std::basic_string"* %8, %"class.std::basic_string"* dereferenceable(32) %20, i8 %22) #10
  ret %"class.std::basic_string"* %8

23:                                               ; preds = %2
  %24 = cleanuppad within none []
  call void @__std_terminate() #12 [ "funclet"(token %24) ]
  unreachable
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local dereferenceable(32) %"class.std::basic_string"* @"?append@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV12@QEBD_K@Z"(%"class.std::basic_string"* %0, i8* %1, i64 %2) #2 comdat align 2 {
  %4 = alloca %"class.std::basic_string"*, align 8
  %5 = alloca i64, align 8
  %6 = alloca i8*, align 8
  %7 = alloca %"class.std::basic_string"*, align 8
  %8 = alloca i64, align 8
  %9 = alloca i8*, align 8
  %10 = alloca i8, align 1
  %11 = alloca %class.anon.0, align 1
  store i64 %2, i64* %5, align 8
  store i8* %1, i8** %6, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %7, align 8
  %12 = load %"class.std::basic_string"*, %"class.std::basic_string"** %7, align 8
  %13 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %12, i32 0, i32 0
  %14 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %13, i32 0, i32 0
  %15 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %14, i32 0, i32 1
  %16 = load i64, i64* %15, align 8
  store i64 %16, i64* %8, align 8
  %17 = load i64, i64* %5, align 8
  %18 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %12, i32 0, i32 0
  %19 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %18, i32 0, i32 0
  %20 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %19, i32 0, i32 2
  %21 = load i64, i64* %20, align 8
  %22 = load i64, i64* %8, align 8
  %23 = sub i64 %21, %22
  %24 = icmp ule i64 %17, %23
  br i1 %24, label %25, label %46

25:                                               ; preds = %3
  %26 = load i64, i64* %8, align 8
  %27 = load i64, i64* %5, align 8
  %28 = add i64 %26, %27
  %29 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %12, i32 0, i32 0
  %30 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %29, i32 0, i32 0
  %31 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %30, i32 0, i32 1
  store i64 %28, i64* %31, align 8
  %32 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %12, i32 0, i32 0
  %33 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %32, i32 0, i32 0
  %34 = call i8* @"?_Myptr@?$_String_val@U?$_Simple_types@D@std@@@std@@QEAAPEADXZ"(%"class.std::_String_val"* %33) #10
  store i8* %34, i8** %9, align 8
  %35 = load i64, i64* %5, align 8
  %36 = load i8*, i8** %6, align 8
  %37 = load i8*, i8** %9, align 8
  %38 = load i64, i64* %8, align 8
  %39 = getelementptr inbounds i8, i8* %37, i64 %38
  %40 = call i8* @"?move@?$_Narrow_char_traits@DH@std@@SAPEADQEADQEBD_K@Z"(i8* %39, i8* %36, i64 %35) #10
  store i8 0, i8* %10, align 1
  %41 = load i8*, i8** %9, align 8
  %42 = load i64, i64* %8, align 8
  %43 = load i64, i64* %5, align 8
  %44 = add i64 %42, %43
  %45 = getelementptr inbounds i8, i8* %41, i64 %44
  call void @"?assign@?$_Narrow_char_traits@DH@std@@SAXAEADAEBD@Z"(i8* dereferenceable(1) %45, i8* dereferenceable(1) %10) #10
  store %"class.std::basic_string"* %12, %"class.std::basic_string"** %4, align 8
  br label %53

46:                                               ; preds = %3
  %47 = load i64, i64* %5, align 8
  %48 = load i8*, i8** %6, align 8
  %49 = load i64, i64* %5, align 8
  %50 = getelementptr inbounds %class.anon.0, %class.anon.0* %11, i32 0, i32 0
  %51 = load i8, i8* %50, align 1
  %52 = call dereferenceable(32) %"class.std::basic_string"* @"??$_Reallocate_grow_by@V<lambda_1>@?0??append@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV34@QEBD_K@Z@PEBD_K@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAAEAV01@_KV<lambda_1>@?0??append@01@QEAAAEAV01@QEBD0@Z@PEBD_K@Z"(%"class.std::basic_string"* %12, i64 %49, i8 %51, i8* %48, i64 %47)
  store %"class.std::basic_string"* %52, %"class.std::basic_string"** %4, align 8
  br label %53

53:                                               ; preds = %46, %25
  %54 = load %"class.std::basic_string"*, %"class.std::basic_string"** %4, align 8
  ret %"class.std::basic_string"* %54
}

; Function Attrs: noinline optnone uwtable
define linkonce_odr dso_local dereferenceable(32) %"class.std::basic_string"* @"??$_Reallocate_grow_by@V<lambda_1>@?0??append@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV34@QEBD_K@Z@PEBD_K@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAAEAV01@_KV<lambda_1>@?0??append@01@QEAAAEAV01@QEBD0@Z@PEBD_K@Z"(%"class.std::basic_string"* %0, i64 %1, i8 %2, i8* %3, i64 %4) #2 comdat align 2 {
  %6 = alloca %class.anon.0, align 1
  %7 = alloca i64, align 8
  %8 = alloca i8*, align 8
  %9 = alloca i64, align 8
  %10 = alloca %"class.std::basic_string"*, align 8
  %11 = alloca %"class.std::_String_val"*, align 8
  %12 = alloca i64, align 8
  %13 = alloca i64, align 8
  %14 = alloca i64, align 8
  %15 = alloca i64, align 8
  %16 = alloca %"class.std::allocator"*, align 8
  %17 = alloca i8*, align 8
  %18 = alloca i8*, align 8
  %19 = alloca i8*, align 8
  %20 = getelementptr inbounds %class.anon.0, %class.anon.0* %6, i32 0, i32 0
  store i8 %2, i8* %20, align 1
  store i64 %4, i64* %7, align 8
  store i8* %3, i8** %8, align 8
  store i64 %1, i64* %9, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %10, align 8
  %21 = load %"class.std::basic_string"*, %"class.std::basic_string"** %10, align 8
  %22 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %21, i32 0, i32 0
  %23 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %22, i32 0, i32 0
  store %"class.std::_String_val"* %23, %"class.std::_String_val"** %11, align 8
  %24 = load %"class.std::_String_val"*, %"class.std::_String_val"** %11, align 8
  %25 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %24, i32 0, i32 1
  %26 = load i64, i64* %25, align 8
  store i64 %26, i64* %12, align 8
  %27 = call i64 @"?max_size@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEBA_KXZ"(%"class.std::basic_string"* %21) #10
  %28 = load i64, i64* %12, align 8
  %29 = sub i64 %27, %28
  %30 = load i64, i64* %9, align 8
  %31 = icmp ult i64 %29, %30
  br i1 %31, label %32, label %33

32:                                               ; preds = %5
  call void @"?_Xlen_string@std@@YAXXZ"() #11
  unreachable

33:                                               ; preds = %5
  %34 = load i64, i64* %12, align 8
  %35 = load i64, i64* %9, align 8
  %36 = add i64 %34, %35
  store i64 %36, i64* %13, align 8
  %37 = load %"class.std::_String_val"*, %"class.std::_String_val"** %11, align 8
  %38 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %37, i32 0, i32 2
  %39 = load i64, i64* %38, align 8
  store i64 %39, i64* %14, align 8
  %40 = load i64, i64* %13, align 8
  %41 = call i64 @"?_Calculate_growth@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEBA_K_K@Z"(%"class.std::basic_string"* %21, i64 %40) #10
  store i64 %41, i64* %15, align 8
  %42 = call dereferenceable(1) %"class.std::allocator"* @"?_Getal@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAAEAV?$allocator@D@2@XZ"(%"class.std::basic_string"* %21) #10
  store %"class.std::allocator"* %42, %"class.std::allocator"** %16, align 8
  %43 = load %"class.std::allocator"*, %"class.std::allocator"** %16, align 8
  %44 = load i64, i64* %15, align 8
  %45 = add i64 %44, 1
  %46 = call i8* @"?allocate@?$allocator@D@std@@QEAAPEAD_K@Z"(%"class.std::allocator"* %43, i64 %45)
  store i8* %46, i8** %17, align 8
  %47 = load %"class.std::_String_val"*, %"class.std::_String_val"** %11, align 8
  %48 = bitcast %"class.std::_String_val"* %47 to %"struct.std::_Container_base0"*
  call void @"?_Orphan_all@_Container_base0@std@@QEAAXXZ"(%"struct.std::_Container_base0"* %48) #10
  %49 = load i64, i64* %13, align 8
  %50 = load %"class.std::_String_val"*, %"class.std::_String_val"** %11, align 8
  %51 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %50, i32 0, i32 1
  store i64 %49, i64* %51, align 8
  %52 = load i64, i64* %15, align 8
  %53 = load %"class.std::_String_val"*, %"class.std::_String_val"** %11, align 8
  %54 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %53, i32 0, i32 2
  store i64 %52, i64* %54, align 8
  %55 = load i8*, i8** %17, align 8
  %56 = call i8* @"??$_Unfancy@D@std@@YAPEADPEAD@Z"(i8* %55) #10
  store i8* %56, i8** %18, align 8
  %57 = load i64, i64* %14, align 8
  %58 = icmp ule i64 16, %57
  br i1 %58, label %59, label %78

59:                                               ; preds = %33
  %60 = load %"class.std::_String_val"*, %"class.std::_String_val"** %11, align 8
  %61 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %60, i32 0, i32 0
  %62 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %61 to i8**
  %63 = load i8*, i8** %62, align 8
  store i8* %63, i8** %19, align 8
  %64 = load i64, i64* %7, align 8
  %65 = load i8*, i8** %8, align 8
  %66 = load i64, i64* %12, align 8
  %67 = load i8*, i8** %19, align 8
  %68 = call i8* @"??$_Unfancy@D@std@@YAPEADPEAD@Z"(i8* %67) #10
  %69 = load i8*, i8** %18, align 8
  call void @"??R<lambda_1>@?0??append@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV23@QEBD_K@Z@QEBA?A?<auto>@@QEAD0101@Z"(%class.anon.0* %6, i8* %69, i8* %68, i64 %66, i8* %65, i64 %64)
  %70 = load %"class.std::allocator"*, %"class.std::allocator"** %16, align 8
  %71 = load i64, i64* %14, align 8
  %72 = add i64 %71, 1
  %73 = load i8*, i8** %19, align 8
  call void @"?deallocate@?$allocator@D@std@@QEAAXQEAD_K@Z"(%"class.std::allocator"* %70, i8* %73, i64 %72)
  %74 = load i8*, i8** %17, align 8
  %75 = load %"class.std::_String_val"*, %"class.std::_String_val"** %11, align 8
  %76 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %75, i32 0, i32 0
  %77 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %76 to i8**
  store i8* %74, i8** %77, align 8
  br label %90

78:                                               ; preds = %33
  %79 = load i64, i64* %7, align 8
  %80 = load i8*, i8** %8, align 8
  %81 = load i64, i64* %12, align 8
  %82 = load %"class.std::_String_val"*, %"class.std::_String_val"** %11, align 8
  %83 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %82, i32 0, i32 0
  %84 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %83 to [16 x i8]*
  %85 = getelementptr inbounds [16 x i8], [16 x i8]* %84, i64 0, i64 0
  %86 = load i8*, i8** %18, align 8
  call void @"??R<lambda_1>@?0??append@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV23@QEBD_K@Z@QEBA?A?<auto>@@QEAD0101@Z"(%class.anon.0* %6, i8* %86, i8* %85, i64 %81, i8* %80, i64 %79)
  %87 = load %"class.std::_String_val"*, %"class.std::_String_val"** %11, align 8
  %88 = getelementptr inbounds %"class.std::_String_val", %"class.std::_String_val"* %87, i32 0, i32 0
  %89 = bitcast %"union.std::_String_val<std::_Simple_types<char> >::_Bxty"* %88 to i8**
  call void @"??$_Construct_in_place@PEADAEBQEAD@std@@YAXAEAPEADAEBQEAD@Z"(i8** dereferenceable(8) %89, i8** dereferenceable(8) %17) #10
  br label %90

90:                                               ; preds = %78, %59
  ret %"class.std::basic_string"* %21
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"??R<lambda_1>@?0??append@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@QEAAAEAV23@QEBD_K@Z@QEBA?A?<auto>@@QEAD0101@Z"(%class.anon.0* %0, i8* %1, i8* %2, i64 %3, i8* %4, i64 %5) #0 comdat align 2 {
  %7 = alloca i64, align 8
  %8 = alloca i8*, align 8
  %9 = alloca i64, align 8
  %10 = alloca i8*, align 8
  %11 = alloca i8*, align 8
  %12 = alloca %class.anon.0*, align 8
  %13 = alloca i8, align 1
  store i64 %5, i64* %7, align 8
  store i8* %4, i8** %8, align 8
  store i64 %3, i64* %9, align 8
  store i8* %2, i8** %10, align 8
  store i8* %1, i8** %11, align 8
  store %class.anon.0* %0, %class.anon.0** %12, align 8
  %14 = load %class.anon.0*, %class.anon.0** %12, align 8
  %15 = load i64, i64* %9, align 8
  %16 = load i8*, i8** %10, align 8
  %17 = load i8*, i8** %11, align 8
  %18 = call i8* @"?copy@?$_Narrow_char_traits@DH@std@@SAPEADQEADQEBD_K@Z"(i8* %17, i8* %16, i64 %15) #10
  %19 = load i64, i64* %7, align 8
  %20 = load i8*, i8** %8, align 8
  %21 = load i8*, i8** %11, align 8
  %22 = load i64, i64* %9, align 8
  %23 = getelementptr inbounds i8, i8* %21, i64 %22
  %24 = call i8* @"?copy@?$_Narrow_char_traits@DH@std@@SAPEADQEADQEBD_K@Z"(i8* %23, i8* %20, i64 %19) #10
  store i8 0, i8* %13, align 1
  %25 = load i8*, i8** %11, align 8
  %26 = load i64, i64* %9, align 8
  %27 = load i64, i64* %7, align 8
  %28 = add i64 %26, %27
  %29 = getelementptr inbounds i8, i8* %25, i64 %28
  call void @"?assign@?$_Narrow_char_traits@DH@std@@SAXAEADAEBD@Z"(i8* dereferenceable(1) %29, i8* dereferenceable(1) %13) #10
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local dereferenceable(1) %"class.std::allocator"* @"??$move@AEAV?$allocator@D@std@@@std@@YA$$QEAV?$allocator@D@0@AEAV10@@Z"(%"class.std::allocator"* dereferenceable(1) %0) #0 comdat {
  %2 = alloca %"class.std::allocator"*, align 8
  store %"class.std::allocator"* %0, %"class.std::allocator"** %2, align 8
  %3 = load %"class.std::allocator"*, %"class.std::allocator"** %2, align 8
  ret %"class.std::allocator"* %3
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"?_Alloc_proxy@_Container_base0@std@@QEAAXAEBU_Fake_allocator@2@@Z"(%"struct.std::_Container_base0"* %0, %"struct.std::_Fake_allocator"* dereferenceable(1) %1) #0 comdat align 2 {
  %3 = alloca %"struct.std::_Fake_allocator"*, align 8
  %4 = alloca %"struct.std::_Container_base0"*, align 8
  store %"struct.std::_Fake_allocator"* %1, %"struct.std::_Fake_allocator"** %3, align 8
  store %"struct.std::_Container_base0"* %0, %"struct.std::_Container_base0"** %4, align 8
  %5 = load %"struct.std::_Container_base0"*, %"struct.std::_Container_base0"** %4, align 8
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"?_Take_contents@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAXAEAV12@U?$integral_constant@_N$00@2@@Z"(%"class.std::basic_string"* %0, %"class.std::basic_string"* dereferenceable(32) %1, i8 %2) #0 comdat align 2 {
  %4 = alloca %"struct.std::integral_constant", align 1
  %5 = alloca %"class.std::basic_string"*, align 8
  %6 = alloca %"class.std::basic_string"*, align 8
  %7 = getelementptr inbounds %"struct.std::integral_constant", %"struct.std::integral_constant"* %4, i32 0, i32 0
  store i8 %2, i8* %7, align 1
  store %"class.std::basic_string"* %1, %"class.std::basic_string"** %5, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %6, align 8
  %8 = load %"class.std::basic_string"*, %"class.std::basic_string"** %6, align 8
  %9 = load %"class.std::basic_string"*, %"class.std::basic_string"** %5, align 8
  call void @"?_Memcpy_val_from@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAXAEBV12@@Z"(%"class.std::basic_string"* %8, %"class.std::basic_string"* dereferenceable(32) %9) #10
  %10 = load %"class.std::basic_string"*, %"class.std::basic_string"** %5, align 8
  call void @"?_Tidy_init@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAXXZ"(%"class.std::basic_string"* %10) #10
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local void @"?_Memcpy_val_from@?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@AEAAXAEBV12@@Z"(%"class.std::basic_string"* %0, %"class.std::basic_string"* dereferenceable(32) %1) #0 comdat align 2 {
  %3 = alloca %"class.std::basic_string"*, align 8
  %4 = alloca %"class.std::basic_string"*, align 8
  %5 = alloca i8*, align 8
  %6 = alloca i8*, align 8
  store %"class.std::basic_string"* %1, %"class.std::basic_string"** %3, align 8
  store %"class.std::basic_string"* %0, %"class.std::basic_string"** %4, align 8
  %7 = load %"class.std::basic_string"*, %"class.std::basic_string"** %4, align 8
  %8 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %7, i32 0, i32 0
  %9 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %8, i32 0, i32 0
  %10 = call %"class.std::_String_val"* @"??$addressof@V?$_String_val@U?$_Simple_types@D@std@@@std@@@std@@YAPEAV?$_String_val@U?$_Simple_types@D@std@@@0@AEAV10@@Z"(%"class.std::_String_val"* dereferenceable(32) %9) #10
  %11 = bitcast %"class.std::_String_val"* %10 to i8*
  %12 = getelementptr inbounds i8, i8* %11, i64 0
  store i8* %12, i8** %5, align 8
  %13 = load %"class.std::basic_string"*, %"class.std::basic_string"** %3, align 8
  %14 = getelementptr inbounds %"class.std::basic_string", %"class.std::basic_string"* %13, i32 0, i32 0
  %15 = getelementptr inbounds %"class.std::_Compressed_pair", %"class.std::_Compressed_pair"* %14, i32 0, i32 0
  %16 = call %"class.std::_String_val"* @"??$addressof@$$CBV?$_String_val@U?$_Simple_types@D@std@@@std@@@std@@YAPEBV?$_String_val@U?$_Simple_types@D@std@@@0@AEBV10@@Z"(%"class.std::_String_val"* dereferenceable(32) %15) #10
  %17 = bitcast %"class.std::_String_val"* %16 to i8*
  %18 = getelementptr inbounds i8, i8* %17, i64 0
  store i8* %18, i8** %6, align 8
  %19 = load i8*, i8** %5, align 8
  %20 = load i8*, i8** %6, align 8
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 1 %19, i8* align 1 %20, i64 32, i1 false)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %"class.std::_String_val"* @"??$addressof@V?$_String_val@U?$_Simple_types@D@std@@@std@@@std@@YAPEAV?$_String_val@U?$_Simple_types@D@std@@@0@AEAV10@@Z"(%"class.std::_String_val"* dereferenceable(32) %0) #0 comdat {
  %2 = alloca %"class.std::_String_val"*, align 8
  store %"class.std::_String_val"* %0, %"class.std::_String_val"** %2, align 8
  %3 = load %"class.std::_String_val"*, %"class.std::_String_val"** %2, align 8
  ret %"class.std::_String_val"* %3
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local %"class.std::_String_val"* @"??$addressof@$$CBV?$_String_val@U?$_Simple_types@D@std@@@std@@@std@@YAPEBV?$_String_val@U?$_Simple_types@D@std@@@0@AEBV10@@Z"(%"class.std::_String_val"* dereferenceable(32) %0) #0 comdat {
  %2 = alloca %"class.std::_String_val"*, align 8
  store %"class.std::_String_val"* %0, %"class.std::_String_val"** %2, align 8
  %3 = load %"class.std::_String_val"*, %"class.std::_String_val"** %2, align 8
  ret %"class.std::_String_val"* %3
}

attributes #0 = { noinline nounwind optnone uwtable "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #1 = { argmemonly nounwind willreturn }
attributes #2 = { noinline optnone uwtable "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #3 = { noinline norecurse optnone uwtable "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #4 = { nounwind "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "no-infs-fp-math"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #5 = { noreturn "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "no-infs-fp-math"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #6 = { noinline noreturn optnone uwtable "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #7 = { nobuiltin nounwind "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "no-infs-fp-math"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #8 = { "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "no-infs-fp-math"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #9 = { nobuiltin "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "frame-pointer"="none" "less-precise-fpmad"="false" "no-infs-fp-math"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #10 = { nounwind }
attributes #11 = { noreturn }
attributes #12 = { noreturn nounwind }
attributes #13 = { builtin nounwind }

!llvm.linker.options = !{!0, !1, !2, !3, !4}
!llvm.module.flags = !{!5, !6}
!llvm.ident = !{!7}

!0 = !{!"/FAILIFMISMATCH:\22_MSC_VER=1900\22"}
!1 = !{!"/FAILIFMISMATCH:\22_ITERATOR_DEBUG_LEVEL=0\22"}
!2 = !{!"/FAILIFMISMATCH:\22RuntimeLibrary=MT_StaticRelease\22"}
!3 = !{!"/DEFAULTLIB:libcpmt.lib"}
!4 = !{!"/FAILIFMISMATCH:\22_CRT_STDIO_ISO_WIDE_SPECIFIERS=0\22"}
!5 = !{i32 1, !"wchar_size", i32 2}
!6 = !{i32 7, !"PIC Level", i32 2}
!7 = !{!"clang version 10.0.0 "}
