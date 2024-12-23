using StudentManager.ViewModels.Windows;
using System.Windows;
using Wpf.Ui.Controls;
using System.IO;
using System.Text.Json;
using StudentManager.Models;
using StudentManager.Views.Windows;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;
using MessageBoxResult = System.Windows.MessageBoxResult;
using StudentManager.ViewModels;
using StudentManager.Views.Pages;

namespace StudentManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            Loaded += MainWindow_Loaded;
            
            // Add navigation handling
            RootNavigation.Navigating += RootNavigation_NavigationRequested;

            // Check for active session
            if (File.Exists("session.json"))
            {
                var session = JsonSerializer.Deserialize<Session>(File.ReadAllText("session.json"));
                if (session != null && session.Expiry > DateTime.Now)
                {
                    // Renew session
                    session.Expiry = Session.DefaultExpiry;

                    // Save session
                    MainViewModel.CurrentSession = session;
                }
                else
                {
                    File.Delete("session.json");
                    // Redirect to login
                    LoginWindow loginWindow = new();
                    loginWindow.Show();
                    Close();
                }
            }
            else
            {
                // Redirect to login
                LoginWindow loginWindow = new();
                loginWindow.Show();
                Close();
            }
        }

        private void RootNavigation_NavigationRequested(NavigationView sender, NavigatingCancelEventArgs args)
        {
            if (args.Page.GetType() == typeof(LogoutPage))
            {
                args.Cancel = true; // Prevent automatic navigation

                MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment vous déconnecter?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Pages.LogoutPage.Logout(this);
                }
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate(typeof(DashboardPage));

            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(
                this,
                WindowBackdropType.Mica,
                true
            );
        }
    }
}
