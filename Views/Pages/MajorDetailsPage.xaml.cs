using MySql.Data.MySqlClient;
using StudentManager.Models;
using StudentManager.Services;
using StudentManager.ViewModels;
using StudentManager.DataAccess;
using System.Windows;
using System.Windows.Controls;

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
                }
            }
        }

        private void UpdateMajorInDatabase(Major major)
        {
            try
            {
                using var connection = DBConnection.GetConnection();
                connection?.Open();
                var query = "UPDATE Majors SET Name = @Name, Description = @Description, Responsable = @Responsable WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", major.Name);
                command.Parameters.AddWithValue("@Description", major.Description);
                command.Parameters.AddWithValue("@Responsable", major.Responsable);
                command.Parameters.AddWithValue("@Id", major.MajorId);

                command.ExecuteNonQuery();

                // Refresh the major data in the major details page
                RefreshMajorData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the major in the database.\n" +
                    ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
