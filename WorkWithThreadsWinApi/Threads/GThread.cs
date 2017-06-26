using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace WorkWithThreadsWinApi.Threads
{
    public class GThread
    {
        private bool flag = true;

        public ThreadPriority Priority
        {
            get { return WinApiHelper.GetThreadPriority(Handle); }
            set { WinApiHelper.SetThreadPriority(Handle, (int)value); }
        }

        public uint Id { get; private set; }

        public uint Handle { get; private set; }

        public TimeSpan ThreadTime
        {
            get { return DiagnosticHelper.GetThreadTime(Id); }
        }

        public GThread()
        {
        }

        public unsafe void Start()
        {
            var threadDelegate = new ThreadDelegate(CalculatePi);
            var func = Marshal.GetFunctionPointerForDelegate(threadDelegate);

            uint lpThreadId;
            Handle = WinApiHelper.CreateThread(IntPtr.Zero, 0, func, IntPtr.Zero, 0, out lpThreadId);
            Id = lpThreadId;
        }

        public GThread(ThreadPriority priority)
            : this()
        {
            Priority = priority;
        }

        public void DestroyGThread()
        {
            flag = false;
            WinApiHelper.WaitForSingleObject(Handle, 10000);
            WinApiHelper.CloseHandle(Handle);
        }

        private void CalculatePi()
        {
            double i = 0;
            double pi = 0;
            while (flag)
            {
                pi += (1.0 / (1.0 + 2.0 * i)) * ((i % 2 == 0) ? 1 : (-1));
                i++;
            }
        }
    }
}
