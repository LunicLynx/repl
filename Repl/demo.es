//alias HANDLE = void*
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
	WriteFile(handle, text, text.Length, 0, 0)
}

struct A : A {}

//struct B : C {}
//struct C : B {}

struct Person {
    Name: string
    Age: i32
    //i32 Age

	Tell() {
		Print(Name)
		Print("\n")
	}
}

let p1 = Person
let p2 = new Person

//let p3: Person# = Person
//let p4: Person = new Person

p1
p2
//p3
//p4

p1.Name = "Kathrin"
p2.Name = "Florian"
Print(p1.Name)
Print("\n")

p2.Tell()
p1.Tell()
//p3.Tell()
//p4.Tell()

let handle = GetStdHandle(-11)
WriteFile(handle, "Hello Alma\n", 11, 0, 0)

Print("Hello World!\n")