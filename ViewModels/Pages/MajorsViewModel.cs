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

            // Update SelectedMajors when Majors changes
            Majors.CollectionChanged += (m, e) => RaisePropertyChanged(nameof(SelectedMajors));
        }

        private async Task LoadMajorsAsync()
        {
            try
            {
                using var connection = DBConnection.GetConnection();
                await connection.OpenAsync();

                // Requête SQL pour récupérer les étudiants et leurs majeures associées
                string query = "SELECT majors.Id,  majors.Name,  majors.Description FROM majors";

                using var command = new MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();
                // Boucle pour lire chaque ligne des résultats de la requête
                while (await reader.ReadAsync())
                {
                    // Création d'un étudiant à partir des données lues
                    var major = new Major
                    {
                        Id = reader.GetInt32("Id"), // Récupérer l'Id de la majeure
                        Name = reader.GetString("Name"), // Récupérer le nom de la majeure
                        Description = reader.GetString("Description") // Récupérer la description de la majeure
                    };

                    // Ajouter l'étudiant à la collection
                    Majors.Add(major);
                }
            }
            catch (Exception ex)
            {
                // Afficher un message d'erreur si la récupération échoue
                MessageBox.Show($"Erreur lors du chargement des fiil: {ex.Message}");
            }

        }
    }
}
