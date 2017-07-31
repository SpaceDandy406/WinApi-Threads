using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WorkWithThreadsWinApi.Threads
{
    internal class GThreadPool
    {
        private readonly List<uint> _shouldDelete = new List<uint>();
        private readonly ObservableCollection<ThreadInfo> _threads = new ObservableCollection<ThreadInfo>();

        public void Add()
        {
            _threads.Add(new ThreadInfo());
        }

        public void MarkToDelete(uint id)
        {
            _shouldDelete.Add(id);
        }

        public ObservableCollection<ThreadInfo> GetEnumerable()
        {
            foreach (var id in _shouldDelete)
            {
                var thread = _threads.First(info => info.Id == id);

                if (thread == null)
                    continue;

                _threads.Remove(thread);
                thread.StopThread();
            }

            return _threads;
        }
    }
}
