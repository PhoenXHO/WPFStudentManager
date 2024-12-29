using StudentManager.ViewModels.Pages;
using System.Windows;
using System.Windows.Controls;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for StatsPage.xaml
    /// </summary>
    public partial class StatsPage : Page
    {
        public StatsPage()
        {
            InitializeComponent();
            Loaded += StatsPage_Loaded;

            // Set the BreadcrumbBar
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.BreadcrumbBar.ItemsSource = new[] { "Statistiques" };
        }

        private void StatsPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Instantiate the ViewModel and set as DataContext for binding
            var vm = new StatsViewModel();
            DataContext = vm;
        }
    }
}
