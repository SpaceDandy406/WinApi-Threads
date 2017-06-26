using System;
using System.Diagnostics;
using System.Linq;

namespace WorkWithThreadsWinApi.Threads
{
    internal static class DiagnosticHelper
    {
        public static TimeSpan GetThreadTime(uint id)
        {
            var process = Process.GetCurrentProcess();

            var thread = process.Threads
                .Cast<ProcessThread>()
                .FirstOrDefault(t => t.Id == id);

            if (thread == null)
                throw new ArgumentOutOfRangeException();

            return thread.TotalProcessorTime;
        }
    }
}
