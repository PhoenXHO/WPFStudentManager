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
        public ObservableCollection<Major> MajorsWithAll { get; set; }

        public ICommand ViewUsageInfoCommand { get; }

        public StudentsViewModel(MajorsViewModel majorsViewModel)
        {
            // Create a new ObservableCollection with the 'All' item at the beginning
            MajorsWithAll = new ObservableCollection<Major>(majorsViewModel.Majors);
            MajorsWithAll.Insert(0, new Major { Id = 0, Name = "Tout", Description = "All Majors" });

            // Initialisation de la liste des étudiants
            Students = [];
            _ = LoadStudentsAsync(); // Load students asynchronously
            _ = LoadMajorsAsync(); // Load majors asynchronously

            // Subscribe to the CollectionChanged event to update the SelectedStudents property
            Students.CollectionChanged += (s, e) => RaisePropertyChanged(nameof(SelectedStudents));

            // Command to view usage info
            ViewUsageInfoCommand = new RelayCommand(ViewUsageInfo);
        }

        private async Task LoadStudentsAsync()
        {
            try
            {
                using var connection = DBConnection.GetConnection();
                await connection.OpenAsync();

                string query = @"SELECT students.Id, students.FirstName, students.LastName, students.Email, students.MajorId, students.DateOfBirth, majors.Name as MajorName, majors.Description as MajorDescription
                                 FROM students LEFT JOIN majors ON students.MajorId = majors.Id";

                using var command = new MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var student = new Student
                    {
                        Id = reader.GetInt32("Id"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        Email = reader.GetString("Email"),
                        DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ?
                                      null : reader.GetDateTime("DateOfBirth"),
                        Major = new Major
                        {
                            Id = reader.GetInt32("MajorId"),
                            Name = reader.GetString("MajorName"),
                            Description = reader.GetString("MajorDescription")
                        }
                    };

                    Students.Add(student);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des étudiants: {ex.Message}");
            }
        }

        private void ViewUsageInfo()
        {
            MessageBox.Show("This is a message box");
        }

        private async Task LoadMajorsAsync()
        {
            try
            {
                using var connection = DBConnection.GetConnection();
                await connection.OpenAsync();

                string query = "SELECT majors.Id,  majors.Name,  majors.Description FROM majors";

                using var command = new MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var major = new Major
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Description = reader.GetString("Description")
                    };

                    MajorsWithAll.Add(major);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des fiil: {ex.Message}");
            }
        }
    }
}