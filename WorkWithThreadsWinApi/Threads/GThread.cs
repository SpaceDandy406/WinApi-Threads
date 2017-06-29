using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WorkWithThreadsWinApi.Threads
{
    public class GThread
    {
        private bool _canceledFlag = true;
        private readonly ThreadDelegate _threadFuncDelegate;
        private double _lastProcTimeValue;
        private double _lastThreadTimeValue;

        public ThreadPriorityLevel Priority
        {
            get { return DiagnosticHelper.GetThreadPriority(Id); }
            set { DiagnosticHelper.SetThreadPriority(Id, value); }
        }
        public uint Id { get; private set; }
        public uint Handle { get; private set; }
        public int ProcUsage
        {
            get
            {
                return DiagnosticHelper.GetProcUsageForThread(Id, ref _lastProcTimeValue, ref _lastThreadTimeValue);
            }
        }

        public GThread()
        {
            _threadFuncDelegate += CalculatePi;
        }

        public void Start()
        {
            var func = Marshal.GetFunctionPointerForDelegate(_threadFuncDelegate);

            uint lpThreadId;
            Handle = WinApiHelper.CreateThread(IntPtr.Zero, 0, func, IntPtr.Zero, 0, out lpThreadId);
            Id = lpThreadId;

            DiagnosticHelper.SetThreadAffinity(Id);
        }

        public void DestroyGThread()
        {
            _canceledFlag = false;
            WinApiHelper.WaitForSingleObject(Handle, 10000);
            WinApiHelper.CloseHandle(Handle);
        }

        private void CalculatePi()
        {
            double i = 0;
            // ReSharper disable once NotAccessedVariable
            double pi = 0;
            while (_canceledFlag)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                pi += (1.0 / (1.0 + 2.0 * i)) * ((i % 2 == 0) ? 1 : (-1));
                i++;
            }
        }
    }
}
