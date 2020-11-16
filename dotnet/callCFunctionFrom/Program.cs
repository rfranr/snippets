using System;
using System.Runtime.InteropServices;

namespace callCFunctionFromc_
{
public class Tester
    {
#if DEBUG
        public const string confDir = "Debug\\";
#elif RELEASE
        public const string confDir = "Release\\";
#endif

        [DllImport( confDir +  "DLL1.dll", EntryPoint="print")]
	    public static extern void print(string message);

            public static void Main(string[] args)
            {
		print("Hello World C# => C++");
            }
    }
}



