using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using MVVMLiba;
using OxyPlot;
using WorkWithThreadsWinApi.Threads;
using ThreadPriority = WorkWithThreadsWinApi.Threads.ThreadPriority;

namespace WorkWithThreadsWinApi
{
    internal class MainVm : BaseViewModel
    {
        private Dispatcher _dispatcher;

        private ObservableCollection<ThreadInfo> _threads;
        private ThreadInfo _selectedthread;
        private ObservableCollection<ThreadPriority> _threadPriorities;
        private ThreadPriority _selectedThreadPriority;
        private PlotModel _threadPlotModel;

        private List<GThread> _gThreads;
        private Timer _timer;
        private readonly PlotDataFormer _dataFormer;
        private readonly PiCounter _piCounter;

        private ICommand _addThreadCommand;
        private ICommand _deleteThreadCommand;
        private ICommand _setThreadPriorityCommand;

        public ObservableCollection<ThreadInfo> Threads
        {
            get { return _threads; }
            set
            {
                _threads = value;
                OnPropertyChanged();
            }
        }

        public ThreadInfo SelectedThread
        {
            get { return _selectedthread; }
            set
            {
                _selectedthread = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ThreadPriority> ThreadPriorities
        {
            get
            {
                return _threadPriorities;
            }
            set
            {
                _threadPriorities = value;
                OnPropertyChanged();
            }
        }

        public ThreadPriority SelectedThreadPriority
        {
            get
            {
                return _selectedThreadPriority;
            }
            set
            {
                _selectedThreadPriority = value;
                OnPropertyChanged();
            }
        }

        public PlotModel ThreadPlotModel
        {
            get
            {
                return _threadPlotModel;
            }
            set
            {
                _threadPlotModel = value;
                OnPropertyChanged();
            }
        }

        public MainVm()
        {
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr) 1;

            _dispatcher = Dispatcher.CurrentDispatcher;
            _dataFormer = new PlotDataFormer();
            _gThreads = new List<GThread>();
            _piCounter = new PiCounter();

            Threads = new ObservableCollection<ThreadInfo>();
            SelectedThread = new ThreadInfo();
            ThreadPlotModel = new PlotModel();
            ThreadPriorities = GetPriorities();

            //_timer = new Timer(Loop, null, 1000, 1000);
        }

        private void Loop(object state)
        {
            _dispatcher.Invoke(Loop1);
        }

        private void Loop1()
        {
            Threads = new ObservableCollection<ThreadInfo>();

            foreach (var gThread in _gThreads)
            {
                Threads.Add(new ThreadInfo { Id = gThread.Id, Priority = gThread.Priority, ThreadTime = gThread.ThreadTime });
            }

            //Debug.WriteLine(_gThreads.Count);
            
            ThreadPlotModel = _dataFormer.GetPlotModel(Threads.ToList());
            //Thread.Sleep(500);
        }

        public ICommand AddThreadCommand
        {
            get { return _addThreadCommand ?? new RelayCommand(AddThread); }
        }

        public ICommand DeleteThreadCommand
        {
            get { return _deleteThreadCommand ?? new RelayCommand(DeleteThread); }
        }

        public ICommand SetThreadPriorityCommand
        {
            get { return _setThreadPriorityCommand ?? new RelayCommand(DeleteThread); }
        }

        private void AddThread(object obj)
        {
            var thread = new GThread();
            _gThreads.Add(thread);
            thread.Start();
            Loop1();
        }

        private void DeleteThread(object obj)
        {
            var id = SelectedThread.Id;

            var thread = _gThreads.Find(t => t.Id == id);

            if (thread == null)
                return;

            _gThreads.Remove(thread);

            thread.DestroyGThread();
            //thread = null;

            Loop1();
        }

        private void SetThreadPriority(object obj)
        {

        }

        private static ObservableCollection<ThreadPriority> GetPriorities()
        {
            var list = Enum.GetValues(typeof(ThreadPriority))
                .Cast<ThreadPriority>()
                .ToList();

            return new ObservableCollection<ThreadPriority>(list);
        }
    }
}
