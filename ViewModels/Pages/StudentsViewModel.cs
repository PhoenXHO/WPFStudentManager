using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MySql.Data.MySqlClient;
using StudentManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudentManager.ViewModels.Pages
{
    public class StudentsViewModel : ViewModelBase
    {
        public ObservableCollection<Student> Students { get; }
        public ObservableCollection<Student> SelectedStudents => new(Students.Where(s => s.IsSelected));
        public ObservableCollection<Major> MajorsWithAll { get; } 

        public ICommand ViewUsageInfoCommand { get; }

        public StudentsViewModel(MajorsViewModel majorsViewModel)
        {
            // Créer une liste de majeures avec l'option "Tout" au début
            MajorsWithAll = new ObservableCollection<Major>(majorsViewModel.Majors);
            MajorsWithAll.Insert(0, new Major { Id = 0, Name = "Tout", Description = "All Majors" });

            // Initialisation de la liste des étudiants
            Students = new ObservableCollection<Student>();
            LoadStudentsAsync();  // Charger les étudiants depuis la base de données

            // Souscrire à l'événement CollectionChanged pour mettre à jour la propriété SelectedStudents
            Students.CollectionChanged += (s, e) => RaisePropertyChanged(nameof(SelectedStudents));

            // Commande pour afficher des informations supplémentaires
            ViewUsageInfoCommand = new RelayCommand(ViewUsageInfo);

        }

        // Méthode pour charger les étudiants depuis la base de données
        private async Task LoadStudentsAsync()
        {
            try
            {
                using (var connection = DBConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    // Requête SQL pour récupérer les étudiants et leurs majeures associées
                    string query = "SELECT students.Id, students.FirstName, students.LastName, students.Email, students.MajorId, students.DateOfBirth, majors.Name as MajorName, majors.Description as MajorDescription " +
                                   "FROM students LEFT JOIN majors ON students.MajorId = majors.Id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            // Boucle pour lire chaque ligne des résultats de la requête
                            while (await reader.ReadAsync())
                            {
                                // Création d'un étudiant à partir des données lues
                                var student = new Student
                                {
                                    Id = reader.GetInt32("Id"),
                                    FirstName = reader.GetString("FirstName"),
                                    LastName = reader.GetString("LastName"),
                                    Email = reader.GetString("Email"),
                                    DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? (DateTime?)null : reader.GetDateTime("DateOfBirth"),
                                    Major = new Major
                                    {
                                        Id = reader.GetInt32("MajorId"), // Récupérer l'Id de la majeure
                                        Name = reader.GetString("MajorName"), // Récupérer le nom de la majeure
                                        Description = reader.GetString("MajorDescription") // Récupérer la description de la majeure
                                    }
                                };

                                // Ajouter l'étudiant à la collection
                                Students.Add(student);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Afficher un message d'erreur si la récupération échoue
                MessageBox.Show($"Erreur lors du chargement des étudiants: {ex.Message}");
            }
        }


        // Méthode pour afficher des informations supplémentaires (peut être utilisée pour d'autres actions)
        private void ViewUsageInfo()
        {
            MessageBox.Show("This is a message box");
        }
    }
}
