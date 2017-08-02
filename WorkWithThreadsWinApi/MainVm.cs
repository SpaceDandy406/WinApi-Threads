using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using MVVMLiba;
using OxyPlot;
using WorkWithThreadsWinApi.Threads;

namespace WorkWithThreadsWinApi
{
    internal class MainVm : BaseViewModel
    {
        private readonly object _locker = new object();
        private ObservableCollection<ThreadInfo> _threads;
        private ThreadInfo _selectedthread;
        private ObservableCollection<ThreadPriorityLevel> _threadPriorities;
        private ThreadPriorityLevel _selectedThreadPriority;
        private PlotModel _threadPlotModel;
        private double _commonPi;

        private Timer _timer;
        private readonly PlotDataFormer _dataFormer;

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

        public ObservableCollection<ThreadPriorityLevel> ThreadPriorities
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

        public ThreadPriorityLevel SelectedThreadPriority
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

        public double CommonPi
        {
            get { return _commonPi; }
            set
            {
                _commonPi = value;
                OnPropertyChanged();
            }
        }

        public MainVm()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;

            DiagnosticHelper.SetNativeThreadAffinity();

            _dataFormer = new PlotDataFormer();

            Threads = new ObservableCollection<ThreadInfo>();
            ThreadPlotModel = new PlotModel();
            ThreadPriorities = GetPriorities();

            _timer = new Timer(Loop, null, 1000, 1000);
        }

        private void Loop(object obj)
        {
            lock (_locker)
            {
                foreach (var threadInfo in Threads)
                {
                    threadInfo.Refresh();
                }

                _dataFormer.RefreshPlotModel(Threads.ToList(), ThreadPlotModel);

                var commonPi = 0.0;
                foreach (var threadInfo in Threads)
                    commonPi += threadInfo.Pi;

                CommonPi = commonPi / Threads.Count;
            }
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
            get { return _setThreadPriorityCommand ?? new RelayCommand(SetThreadPriority); }
        }

        private void AddThread(object obj)
        {
            lock (_locker)
            {
                Threads.Add(new ThreadInfo());
            }
        }

        private void DeleteThread(object obj)
        {
            lock (_locker)
            {
                if (SelectedThread == null)
                    return;

                var id = SelectedThread.Id;

                var thread = Threads.First(info => info.Id == id);

                if (thread == null)
                    return;

                Threads.Remove(thread);

                thread.StopThread();
                thread.Dispose();
            }
        }

        private void SetThreadPriority(object obj)
        {
            lock (_locker)
            {
                if (SelectedThread == null)
                    return;

                var id = SelectedThread.Id;

                var thread = Threads.First(info => info.Id == id);

                thread.Priority = SelectedThreadPriority;
            }
        }

        private static ObservableCollection<ThreadPriorityLevel> GetPriorities()
        {
            var list = Enum.GetValues(typeof(ThreadPriorityLevel))
                .Cast<ThreadPriorityLevel>()
                .ToList();

            return new ObservableCollection<ThreadPriorityLevel>(list);
        }
    }
}
