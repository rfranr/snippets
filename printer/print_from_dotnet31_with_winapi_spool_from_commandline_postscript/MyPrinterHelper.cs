using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using static Samba.Services.Implementations.PrinterModule.Tools.PrinterHelper;

namespace tmpPrintWithSpoolC
{
    public static class MyPrinterHelper
    {
        // From: http://support.microsoft.com/kb/322091
        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document. https://msdn.microsoft.com/en-us/library/windows/desktop/dd145115(v=vs.85).aspx
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page. https://msdn.microsoft.com/en-us/library/windows/desktop/dd145117(v=vs.85).aspx
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter); // https://msdn.microsoft.com/en-us/library/windows/desktop/dd162597(v=vs.85).aspx
                    }
                    EndDocPrinter(hPrinter); // https://msdn.microsoft.com/en-us/library/windows/desktop/dd162595(v=vs.85).aspx
                }
                ClosePrinter(hPrinter); // https://msdn.microsoft.com/en-us/library/windows/desktop/dd183446(v=vs.85).aspx
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }


        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool OpenPrinter(string szPrinter, out IntPtr hPrinter, IntPtr pd);
        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool ClosePrinter(IntPtr hPrinter);
        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, DOCINFOA di);
        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);
        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);
        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);
        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);
    }
}
