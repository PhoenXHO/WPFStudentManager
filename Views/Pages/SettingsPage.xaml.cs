using System.Windows;
using System.Windows.Controls;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            // Set the BreadcrumbBar
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.BreadcrumbBar.ItemsSource = new[] { "Paramètres" };
        }
    }
}
