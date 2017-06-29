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

        public static int GetProcUsageForThread(uint id, ref double lastProcTime, ref double lastThreadTime)
        {
            var process = Process.GetCurrentProcess();

            var thread = process.Threads
                .Cast<ProcessThread>()
                .FirstOrDefault(t => t.Id == id);

            if (thread == null)
                throw new ArgumentOutOfRangeException();

            var threadTime = thread.TotalProcessorTime.TotalMilliseconds;
            var procTime = process.TotalProcessorTime.TotalMilliseconds;

            var threadDeltaTime = threadTime - lastThreadTime;
            var procDeltaTime = procTime - lastProcTime;

            lastThreadTime = threadTime;
            lastProcTime = procTime;

            var proc = threadDeltaTime / procDeltaTime * 100;
            proc = Math.Round(proc, 0);

            return (int)proc;
        }

        public static void SetThreadAffinity(uint id)
        {
            var process = Process.GetCurrentProcess();

            var thread = process.Threads
                .Cast<ProcessThread>()
                .FirstOrDefault(t => t.Id == id);

            if (thread == null)
                throw new ArgumentOutOfRangeException();

            thread.ProcessorAffinity = (IntPtr) 1;
        }
    }
}
