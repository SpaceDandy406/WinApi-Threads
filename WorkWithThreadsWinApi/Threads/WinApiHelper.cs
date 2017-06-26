using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace WorkWithThreadsWinApi.Threads
{
    public enum ThreadPriority
    {
        BackgroundBegin = 0x00010000,
        BackgroundEnd = 0x00020000,
        Idle = -15,
        Lowest = -2,
        BelowNormal = -1,
        Normal = 0,
        AboveNormal = 1,
        Highest = 2,
        TimeCritical = 15
    }

    internal static class WinApiHelper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern unsafe uint CreateThread(
                IntPtr lpThreadAttributes,
                uint dwStackSize,
                IntPtr lpStartAddress,
                IntPtr lpParameter,
                uint dwCreationFlags,
                out uint lpThreadId);

        [DllImport("kernel32.dll")]
        public static extern bool SetThreadPriority(uint hThread, int priority);

        [DllImport("kernel32.dll")]
        public static extern bool TerminateThread(uint hThread, uint dwExitCode);

        [DllImport("kernel32.dll")]
        public static extern ThreadPriority GetThreadPriority(uint hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(uint hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern UInt32 WaitForSingleObject(uint hHandle, UInt32 dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern void ExitThread(uint hHandle);
    }
}
