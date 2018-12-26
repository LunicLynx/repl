extern int GetStdHandle(int nStdHandle)
extern bool WriteFile(int hFile, string lpBuffer, int nNumberOfBytesToWrite, int nNumberOfBytesWritten, int lpOverlapped)

var handle = GetStdHandle(-11)
WriteFile(handle, "Hello Alma", 10, 0, 0)