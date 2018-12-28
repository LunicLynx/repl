extern int GetStdHandle(i32 nStdHandle)
extern bool WriteFile(int hFile, string lpBuffer, i32 nNumberOfBytesToWrite, i64 lpNumberOfBytesWritten, i64 lpOverlapped)

var handle = GetStdHandle(-11)
WriteFile(handle, "Hello Alma", 10, 0, 0)