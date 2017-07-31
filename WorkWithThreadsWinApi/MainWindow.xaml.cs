namespace WorkWithThreadsWinApi
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var vm = new MainVm();

            DataContext = vm;
        }
    }
}
