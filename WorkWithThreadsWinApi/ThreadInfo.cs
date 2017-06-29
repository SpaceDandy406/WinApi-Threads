using System.Diagnostics;
using MVVMLiba;
using WorkWithThreadsWinApi.Threads;

namespace WorkWithThreadsWinApi
{
    internal class ThreadInfo : BaseViewModel
    {
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

        public ThreadInfo()
        {
            _innerThread = new GThread();
            _innerThread.Start();

            Id = _innerThread.Id;
        }

        public void Destruct()
        {
            _innerThread.DestroyGThread();
        }

        public void Refresh()
        {
            Priority = _innerThread.Priority;
            ProcUsage = _innerThread.ProcUsage;
        }
    }
}
