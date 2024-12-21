using Microsoft.OData.Client;
using MySql.Data.MySqlClient;
using SharpDX.Direct3D9;
using StudentManager.Models;
using StudentManager.ViewModels;
using StudentManager.ViewModels.Pages;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for MajorsPage.xaml
    /// </summary>
    public partial class MajorsPage : Page
    {
        public MajorsViewModel ViewModel { get; set; }
        private bool _isShiftPressed;
        private int _lastSelectedIndex = -1;

        public MajorsPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
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
        private void AddMajorToDatabase(Major major)
        {
            try
            {
                using var connection = DBConnection.GetConnection();
                connection?.Open();
                var query = "INSERT INTO majors (Name, Description) VALUES (@Name, @Description)";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", major.Name);
                command.Parameters.AddWithValue("@Description", major.Description);

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

        //private void SelectAllCheckBox_Checked(object sender, RoutedEventArgs e)
        //{
        //    var mainViewModel = (MainViewModel)DataContext;
        //    foreach (var major in mainViewModel.MajorsViewModel.Majors)
        //    {
        //        major.IsSelected = true;
        //    }
        //}

        //private void SelectAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    var mainViewModel = (MainViewModel)DataContext;
        //    foreach (var major in mainViewModel.MajorsViewModel.Majors)
        //    {
        //        major.IsSelected = false;
        //    }
        //}

        private void UpdateDeleteButtonState()
        {
            var mainViewModel = (MainViewModel)DataContext;
            DeleteButton.IsEnabled = mainViewModel.MajorsViewModel.Majors.Any(m => m.IsSelected);
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var major = (Major)((Button)sender).DataContext;
            MessageBox.Show($"Viewing details for {major.Name}");
        }

		private void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var major = (Major)((Button)sender).DataContext;
            var mainViewModel = (MainWindow)Window.GetWindow(this);
            mainViewModel.RootNavigation.Navigate(typeof(MajorDetailsPage), major);
        }
    }
}