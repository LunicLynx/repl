using System;
using System.Runtime.InteropServices;

namespace Repl.CodeAnalysis
{
    /*
     *
     *alias HANDLE = i64
//alias DWORD = i32
////alias LPDWORD = DWORD*
//alias LPDWORD = i64
//alias LPVOID = string*
////alias LPCVOID = /*const*/
    //    void*
    //    alias LPCVOID = /*const*/ string
    //    alias BOOL = int
    //    alias LPOVERLAPPED = i64

    //    const STD_OUTPUT_HANDLE: DWORD = (DWORD) (-11)
    //    const STD_INPUT_HANDLE: DWORD = (DWORD) (-10)

    //// should also work
    ////const STD_OUTPUT_HANDLE2: DWORD = (DWORD)11
    ////const STD_OUTPUT_HANDLE3 = (int)11
    ////const STD_OUTPUT_HANDLE4 = 11

    //    extern GetStdHandle(nStdHandle: DWORD): HANDLE
    ////extern i64 GetStdHandle(i32 nStdHandle)
    //    extern WriteFile(hFile: HANDLE, lpBuffer: LPCVOID, nNumberOfBytesToWrite: DWORD, lpNumberOfBytesWritten: LPDWORD, lpOverlapped: LPOVERLAPPED): BOOL
    ////extern int WriteFile(i64 hFile, string lpBuffer, i32 nNumberOfBytesToWrite, i64 lpNumberOfBytesWritten, i64 lpOverlapped)

    ////extern ReadFile2(hFile: HANDLE, lpBuffer: LPVOID, nNumberOfBytesToRead: DWORD, lpNumberOfBytesRead: LPDWORD, lpOverlapped: LPOVERLAPPED): BOOL;
    //    extern ReadFile2(hFile: HANDLE, lpBuffer: LPVOID, nNumberOfBytesToRead: DWORD, lpNumberOfBytesRead: LPDWORD, lpOverlapped: LPOVERLAPPED): BOOL;
    //*
    //*/

    /*
     func Print(text: string) {
   let handle = GetStdHandle(STD_OUTPUT_HANDLE);
   WriteFile(handle, text, (DWORD)text.Length, 0, 0)
}

func Read() : string {
   let handle = GetStdHandle(STD_INPUT_HANDLE);
   var buffer = "                                                                                             ";
   var x = (DWORD)0;
   ReadFile2(handle, &buffer, (DWORD)buffer.Length, 0, 0)
   return ""
}
     */


    public static class Functions
    {
        public static void putc()
        {
            Console.WriteLine("Hello world");
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int x);

        [DllImport("kernel32.dll")]
        public static extern bool WriteFile(long handle, string text, int len, long p, long p2);

        [DllImport("kernel32.dll")]
        public static extern bool ReadFile(IntPtr handle, [Out] byte[] text, uint len, out uint p, IntPtr p2);

        public static long StringLength(string str)
        {
            return str.Length;
        }


        public static void Print(string s)
        {
            Console.WriteLine(s);
        }

        public static string Input()
        {
            return Console.ReadLine();
        }
    }
}