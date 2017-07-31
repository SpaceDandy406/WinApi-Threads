namespace WorkWithThreadsWinApi.Threads
{
    public class ThreadParameter
    {
        private readonly object _locker = new object();

        public ThreadParameter()
        {
            _pi = 0.0;
        }

        private double _pi;

        public double Pi
        {
            get
            {
                return _pi;
            }
            set
            {
                _pi = value;
            }
        }
    }
}
