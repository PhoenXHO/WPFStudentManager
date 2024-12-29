using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
<<<<<<< Updated upstream
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
=======
using StudentManager.ViewModels.Pages;
using System.Collections.ObjectModel;
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
=======
            DataContext = new DashboardViewModel();

            Loaded += DashboardPage_Loaded;
        }

        private void DashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the BreadcrumbBar
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.BreadcrumbBar.ItemsSource = new[] { "Tableau de bord" };

            // Load logs for the current user
            var viewModel = (DashboardViewModel)DataContext;
            viewModel.LoadUserLogs();
>>>>>>> Stashed changes
        }
    }
}
