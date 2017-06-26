namespace WorkWithThreadsWinApi
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //говно потоки на винде!!!
            DataContext = new MainVm();
        }
    }
}
