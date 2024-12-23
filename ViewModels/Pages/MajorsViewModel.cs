using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MySql.Data.MySqlClient;
using StudentManager.Models;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace StudentManager.ViewModels.Pages
{
    public class MajorsViewModel : ViewModelBase
    {

        public ObservableCollection<Major> Majors { get; }
        public ObservableCollection<Major> SelectedMajors => new(Majors.Where(m => m.IsSelected));

        public MajorsViewModel()
        {

            Majors = [];
            _ = LoadMajorsAsync();
        }

        private async Task LoadMajorsAsync()
        {
            try
            {
                using var connection = DBConnection.GetConnection();
                await connection.OpenAsync();

                string query = "SELECT majors.Id,  majors.Name,  majors.Description,majors.Responsable as Responsable FROM majors";

                using var command = new MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var major = new Major
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Description = reader.GetString("Description"),
                        Responsable = reader.GetString("Responsable")
                    };

                    Majors.Add(major);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des majeures: {ex.Message}");
            }
        }
    }
}