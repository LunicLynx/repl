﻿//alias HANDLE void*
//alias DWORD i32
//alias LPDWORD DWORD*
//alias LPCVOID /*const*/ void*
//alias BOOL int

//const STD_OUTPUT_HANDLE:DWORD = (DWORD)-11

//extern GetStdHandle(nStdHandle: DWORD): HANDLE
extern i64 GetStdHandle(i32 nStdHandle)
//extern WriteFile(hFile: HANDLE, lpBuffer: LPCVOID, nNumberOfBytesToWrite: DWORD, lpNumberOfBytesWritten: LPDWORD, lpOverlapped: i64): BOOL
extern int WriteFile(i64 hFile, string lpBuffer, i32 nNumberOfBytesToWrite, i64 lpNumberOfBytesWritten, i64 lpOverlapped)

//Print(text: string) {
//	let handle = GetStdHandle(STD_OUTPUT_HANDLE);
//	WriteFile(handle, text, text.Length, 0, 0)
//}

struct Person {
    //Age:i32
    i32 Age

//	Tell() {
//		Print("");
//	}
}

//let p1 = Person
//let p2 = new Person

//let p3: Person# = Person
//let p4: Person = new Person

//p1
//p2
//p3
//p4

//p1.Tell()
//p2.Tell()
//p3.Tell()
//p4.Tell()

let handle = GetStdHandle(-11);
WriteFile(handle, "Hello Alma", 10, 0, 0)