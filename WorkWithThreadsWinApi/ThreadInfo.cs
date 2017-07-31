using System;
using System.Diagnostics;
using MVVMLiba;
using WorkWithThreadsWinApi.Threads;

namespace WorkWithThreadsWinApi
{
    internal class ThreadInfo : BaseViewModel, IDisposable
    {
        private bool _disposed;
        private readonly GThread _innerThread;

        private uint _id;
        private ThreadPriorityLevel _priority;
        private int _procUsage;

        public uint Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        public ThreadPriorityLevel Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                _innerThread.Priority = value;
                OnPropertyChanged();
            }
        }
        public int ProcUsage
        {
            get { return _procUsage; }
            set
            {
                _procUsage = value;
                OnPropertyChanged();
            }
        }

        private double _pi;
        public double Pi
        {
            get { return _pi; }
            set
            {
                _pi = value;
                OnPropertyChanged();
            }
        }

        public ThreadInfo()
        {
            _innerThread = new GThread(new ThreadParameter(), ThreadPriorityLevel.Idle);
            _innerThread.Start();

            Id = _innerThread.Id;
        }

        public void StopThread()
        {
            _innerThread.Stop();
        }

        public void Refresh()
        {
            Priority = _innerThread.Priority;
            ProcUsage = _innerThread.ProcUsage;
            Pi = _innerThread.ThreadParameter.Pi;
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
                _innerThread.Dispose();
            }

            _disposed = true;
        }

        ~ThreadInfo()
        {
            Dispose(false);
        }
    }
}
