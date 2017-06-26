using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithThreadsWinApi.Threads
{
    internal class GThreadPool
    {
        private List<uint> _ids;
        private List<ThreadPriority> _priorities;
        private List<uint> _handles;

        public GThreadPool()
        {
            _ids = new List<uint>();
            _priorities = new List<ThreadPriority>();
            _handles = new List<uint>();
        }

        public void CreateThread(Action action)
        {
            var threadDelegate = new ThreadDelegate(action);
            var prtFunc = Marshal.GetFunctionPointerForDelegate(threadDelegate);
            uint lpThreadId;
            //var handle = WinApiHelper.CreateThread(IntPtr.Zero, 0, prtFunc, IntPtr.Zero, 0, out lpThreadId);
            
            //_ids.Add(lpThreadId);
            //_priorities.Add(ThreadPriority.Normal);
            //_handles.Add(handle);
        }

        public void DeleteThread(uint id)
        {
            var index = _ids.IndexOf(id);
            WinApiHelper.TerminateThread(_handles[index], 0);
            _handles.RemoveAt(index);
            _priorities.RemoveAt(index);
        }

        public List<ThreadInfo> GetIds()
        {
            var temp = new List<ThreadInfo>();

            for (int i = 0; i < _ids.Count; i++)
            {
                temp.Add(new ThreadInfo {Id = _ids[i], Priority = _priorities[i]});
            }

            return temp;
        }
    }
}
