using StudentManager.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui;
using Wpf.Ui.Controls;

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
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate(typeof(Pages.DashboardPage));

            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(
                this,
                WindowBackdropType.Mica,
                true
            );
        }

        private void OnNavigationSelectionChanged(NavigationView sender, RoutedEventArgs args)
        {
            if (sender.SelectedItem is not NavigationView navigationView)
            {
                return;
            }

            if (navigationView.SelectedItem is NavigationViewItem selectedItem && selectedItem.TargetPageType != null)
            {
                RootNavigation.Navigate(selectedItem.TargetPageType);
            }
        }
    }
}
