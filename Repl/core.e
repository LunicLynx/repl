// TODO define HANDLE
// TODO define DWORD
extern int GetStdHandle(int nStdHandle);
// TODO define BOOL
// TODO define LPCVOID
// TODO define LPDWORD
// TODO define LPOVERLAPPED
// TODO define struct OVERLAPPED
extern bool WriteFile(int hFile, void* lpBuffer, int nNumberOfBytesToWrite, int* nNumberOfBytesWritten, void* lpOverlapped )
