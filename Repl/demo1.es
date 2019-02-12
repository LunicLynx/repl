extern StringLength(str: string): int

struct String {
    Length:int => StringLength(this)
}

struct Int32 {
}

struct Int64 {
}

struct UInt32 {
}

struct UInt64 {
}

struct Int16 {
}

struct UInt16 {
}

struct Int8 {
}

struct UInt8 {
}

struct Void {
}

struct Boolean {
}

alias HANDLE = i64
alias DWORD = i32
//alias LPDWORD = DWORD*
alias LPDWORD = i64
//alias LPCVOID = /*const*/ void*
alias LPCVOID = /*const*/ string
alias BOOL = int

const STD_OUTPUT_HANDLE: DWORD = (DWORD)(-11)

// should also work
//const STD_OUTPUT_HANDLE2: DWORD = (DWORD)11
//const STD_OUTPUT_HANDLE3 = (int)11
//const STD_OUTPUT_HANDLE4 = 11

extern GetStdHandle(nStdHandle: DWORD): HANDLE
//extern i64 GetStdHandle(i32 nStdHandle)
extern WriteFile(hFile: HANDLE, lpBuffer: LPCVOID, nNumberOfBytesToWrite: DWORD, lpNumberOfBytesWritten: LPDWORD, lpOverlapped: i64): BOOL
//extern int WriteFile(i64 hFile, string lpBuffer, i32 nNumberOfBytesToWrite, i64 lpNumberOfBytesWritten, i64 lpOverlapped)

func Print(text: string) {
	let handle = GetStdHandle(STD_OUTPUT_HANDLE);
	WriteFile(handle, text, (DWORD)text.Length, 0, 0)
}

struct Person {
    Name: string
    Age: i32

	Tell() {
		//Print(Name)
		//Print("\n")
	}
}

//let p1 = Person()
//p1.Name = "Peter"
//Print(p1.Name)
//Print("\n")
//
//p1.Tell()
