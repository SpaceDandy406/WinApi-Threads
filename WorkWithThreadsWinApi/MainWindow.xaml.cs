namespace WorkWithThreadsWinApi
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new MainVm();
        }
    }
}
