using StudentManager.Models;
using StudentManager.DataAccess;
using System.Windows;
using System.Windows.Controls;
using StudentManager.ViewModels;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for MajorDetailsPage.xaml
    /// </summary>
    public partial class MajorDetailsPage : Page
    {
        private Major CurrentMajor { get; set; }

        public MajorDetailsPage()
        {
            InitializeComponent();

            Loaded += MajorDetailsPage_Loaded;
        }

        private void MajorDetailsPage_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentMajor = (Major)DataContext;

            // Set the BreadcrumbBar
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.BreadcrumbBar.ItemsSource = new[] { "Gestion des filières", CurrentMajor.Name };
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialogs.EditMajorDialog
            {
                DataContext = CurrentMajor
            };
            if (dialog.ShowDialog() == true)
            {
                if (await DatabaseRepository.UpdateMajorAsync(CurrentMajor))
                {
                    RefreshMajorData();
                    await MainViewModel.Instance.StudentsViewModel.UpdateStudentsAsync();
                }
            }
        }

        private void RefreshMajorData()
        {
            DataContext = null;
            DataContext = CurrentMajor;

            // Refresh the BreadcrumbBar
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.BreadcrumbBar.ItemsSource = new[] { "Gestion des filières", CurrentMajor.Name };
        }
    }
}
