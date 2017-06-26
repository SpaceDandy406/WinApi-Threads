using System;
using MVVMLiba;
using WorkWithThreadsWinApi.Threads;

namespace WorkWithThreadsWinApi
{
    internal class ThreadInfo : BaseViewModel
    {
        private uint _id;
        private ThreadPriority _priority;
        private TimeSpan _threadTime;

        public uint Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public ThreadPriority Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan ThreadTime
        {
            get { return _threadTime; }
            set
            {
                _threadTime = value;
                OnPropertyChanged();
            }
        }
    }
}
