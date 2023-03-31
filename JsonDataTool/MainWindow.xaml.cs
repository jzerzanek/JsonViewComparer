using System.Windows;

namespace JsonDataTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowPresenter presenter;

        public MainWindow()
        {
            InitializeComponent();

            presenter = new();
            this.DataContext = presenter;
        }
    }
}
