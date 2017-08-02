using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace WorkWithThreadsWinApi.Threads
{
    public class GThread : IDisposable
    {
        private bool _disposed;
        private readonly object _locker = new object();

        private ThreadParameter _threadParameter;
        private bool _canceledFlag = true;
        private ThreadDelegate _threadFuncDelegate;
        private double _lastProcTimeValue;
        private double _lastThreadTimeValue;
        private readonly ThreadPriorityLevel _startPriorityLevel;

        public uint Id { get; private set; }
        public uint Handle { get; private set; }
        public ThreadPriorityLevel Priority
        {
            get
            {
                return DiagnosticHelper.GetThreadPriority(Id);
            }
            set
            {
                DiagnosticHelper.SetThreadPriority(Id, value);
            }
        }
        public int ProcUsage
        {
            get
            {
                return DiagnosticHelper.GetProcUsageForThread(Id, ref _lastProcTimeValue, ref _lastThreadTimeValue);
            }
        }
        public bool CanceledFlag
        {
            get
            {
                lock (_locker)
                {
                    return _canceledFlag;
                }
            }
            set
            {
                lock (_locker)
                {
                    _canceledFlag = value;
                }
            }
        }

        public ThreadParameter ThreadParameter
        {
            get
            {
                lock (_locker)
                {
                    return _threadParameter;
                }
            }
            set
            {
                lock (_locker)
                {
                    _threadParameter = value;
                }
            }
        }

        public GThread()
        {
            _threadFuncDelegate += CalculatePi;
        }

        public GThread(ThreadPriorityLevel priorityLevel)
            : this()
        {
            _startPriorityLevel = priorityLevel;
        }

        public GThread(ThreadParameter param, ThreadPriorityLevel priorityLevel)
            : this(priorityLevel)
        {
            _threadParameter = param;
        }

        public void Start()
        {
            var func = Marshal.GetFunctionPointerForDelegate(_threadFuncDelegate);
            var handlePt = GCHandle.Alloc(_threadParameter);
            var ptrPt = GCHandle.ToIntPtr(handlePt);

            uint lpThreadId;

            Handle = WinApiHelper.CreateThread(IntPtr.Zero, 0, func, ptrPt, 0, out lpThreadId);

            WinApiHelper.SetThreadPriority(Handle, (int)_startPriorityLevel);
            Id = lpThreadId;

            DiagnosticHelper.SetThreadAffinity(Id);
        }

        public void Stop()
        {
            CanceledFlag = false;
        }

        private void CalculatePi(IntPtr param)
        {
            var gcHandle = GCHandle.FromIntPtr(param);
            var threadParam = (ThreadParameter)gcHandle.Target;

            double i = 0;
            // ReSharper disable once NotAccessedVariable
            //double pi = 0;
            while (CanceledFlag)
            {
                var notMatter = 0.0;
                for (double j = 0; j < 100000; j++)
                    notMatter += (j*j)/j;

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                threadParam.Pi += ((1.0 / (1.0 + 2.0 * i)) * ((i % 2 == 0) ? 1 : (-1))) * 4;
                i++;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _threadFuncDelegate = null;
            }

            WinApiHelper.WaitForSingleObject(Handle, 10000);
            WinApiHelper.CloseHandle(Handle);

            _disposed = true;
        }

        ~GThread()
        {
            Dispose(false);
        }
    }
}
