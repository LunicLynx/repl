using System;
using System.Diagnostics;
using System.IO;

namespace XLang.Codegen
{
    public static class Cl
    {
        public static void InvokeCl(string filename)
        {
            Console.WriteLine("Compiling...");

            var toolset = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\VC\Tools\MSVC\14.16.27023";
            var windowskitsInclude = @"C:\Program Files (x86)\Windows Kits\10\include\10.0.17763.0";
            var windowskitsLibs = @"C:\Program Files (x86)\Windows Kits\10\lib\10.0.17763.0";
            var clExe = Path.Combine(toolset, @"bin\Hostx64\x64\cl.exe");

            var includes = new[]
            {
                Path.Combine(toolset, "include"),
                Path.Combine(windowskitsInclude, "ucrt"),
                //Path.Combine(windowskits, "shared"),
                //Path.Combine(windowskits, "um"),
                //Path.Combine(windowskits, "winrt"),
                //Path.Combine(windowskits, "cppwinrt")
            };
            var include = string.Join(";", includes);

            var libs = new[]
            {
                Path.Combine(toolset, @"lib\x64"),
                Path.Combine(windowskitsLibs, @"ucrt\x64"),
                Path.Combine(windowskitsLibs, @"um\x64"),
            };

            var lib = string.Join(";", libs);

            var startInfo = new ProcessStartInfo();
            startInfo.EnvironmentVariables.Add("INCLUDE", include);
            startInfo.EnvironmentVariables.Add("LIB", lib);
            startInfo.FileName = clExe;
            startInfo.Arguments = "/nologo /EHsc main.cpp " + filename;
            var process = Process.Start(startInfo);
            process.WaitForExit();
        }

        public static void InvokeMain()
        {
            Console.WriteLine("Executing...");
            var fileName = "main.exe";
            if (File.Exists(fileName))
            {
                var process = Process.Start(fileName);
                process.WaitForExit();
            }
        }
    }
}

