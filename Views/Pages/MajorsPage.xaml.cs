using MySql.Data.MySqlClient;
using StudentManager.Models;
using StudentManager.ViewModels;
using StudentManager.ViewModels.Pages;
using System.Windows;
using System.Windows.Controls;
using StudentManager.Views.Windows;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for MajorsPage.xaml
    /// </summary>
    public partial class MajorsPage : Page
    {
        public MajorsViewModel ViewModel { get; set; }

        public MajorsPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            // Set the BreadcrumbBar
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.BreadcrumbBar.ItemsSource = new[] { "Gestion des filières" };
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            var dialog = new Dialogs.AddMajorDialog
            {
                DataContext = mainViewModel.MajorsViewModel
            };
            if (dialog.ShowDialog() == true)
            {
                var major = dialog.NewMajor;
                AddMajorToDatabase(major);

                mainViewModel.MajorsViewModel.Majors.Add(major);
            }
        }
        private static void AddMajorToDatabase(Major major)
        {
            try
            {
                using var connection = DBConnection.GetConnection();
                connection?.Open();
                var query = "INSERT INTO majors (Name, Description, Responsable) VALUES (@Name, @Description, @Responsable)";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", major.Name);
                command.Parameters.AddWithValue("@Description", major.Description);
                command.Parameters.AddWithValue("@Responsable", major.Responsable);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout de l fill : {ex.Message}");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Prompt the user to confirm the deletion (in French)
            var result = MessageBox.Show("Voulez-vous vraiment supprimer les filières sélectionnées ? " +
                "Vous allez perdre toutes les données des étudiants associés. " +
                "(Cette action est irréversible)",
                "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            var mainViewModel = (MainViewModel)DataContext;
            var selectedMajors = mainViewModel.MajorsViewModel.Majors.Where(m => m.IsSelected).ToList();
            foreach (var major in selectedMajors)
            {
                DeleteMajorFromDatabase(major);
                mainViewModel.MajorsViewModel.Majors.Remove(major);
            }
        }

        private static void DeleteMajorFromDatabase(Major major)
        {
            try
            {
                using var connection = DBConnection.GetConnection();
                connection?.Open();
                var query = "DELETE FROM Majors WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", major.Id);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression de la fil : {ex.Message}");
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var major = (Major)((CheckBox)sender).DataContext;
            major.IsSelected = true;
            ((MainViewModel)DataContext).MajorsViewModel.RaisePropertyChanged(nameof(MajorsViewModel.SelectedMajors));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var major = (Major)((CheckBox)sender).DataContext;
            major.IsSelected = false;
            ((MainViewModel)DataContext).MajorsViewModel.RaisePropertyChanged(nameof(MajorsViewModel.SelectedMajors));
        }

        private void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var major = (Major)((Button)sender).DataContext;
            var mainViewModel = (MainWindow)Window.GetWindow(this);
            mainViewModel.RootNavigation.Navigate(typeof(MajorDetailsPage), major);
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var major = (Major)((Button)sender).DataContext;
            MessageBox.Show($"Viewing details for {major.Name}");
        }

        private void ViewUsageInfoButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MajorsUsageInfoDialog();
            dialog.ShowDialog();
        }
    }
}