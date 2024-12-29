using System.Windows;
using System.Windows.Controls;
using StudentManager.ViewModels;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
            DataContext = MainViewModel.Instance;

            Loaded += DashboardPage_Loaded;
        }

        private void DashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the BreadcrumbBar
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.BreadcrumbBar.ItemsSource = new[] { "Tableau de bord" };

            // Load logs for the current user
            MainViewModel.Instance.DashboardViewModel.LoadUserLogs();
        }

        private void CardControl_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = (MainWindow)Window.GetWindow(this);
            mainViewModel.RootNavigation.Navigate(typeof(StatsPage));
        }
    }
}
