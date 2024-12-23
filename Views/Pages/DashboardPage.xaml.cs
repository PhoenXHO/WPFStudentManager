using System.ComponentModel;
using MySql.Data.MySqlClient;
using StudentManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page, INotifyPropertyChanged
    {
        private string _studentsPerMajorText;
        private int _totalMajors;
        private int _totalStudents;

        public string StudentsPerMajorText
        {
            get => _studentsPerMajorText;
            set
            {
                if (_studentsPerMajorText != value)
                {
                    _studentsPerMajorText = value;
                    OnPropertyChanged(nameof(StudentsPerMajorText));
                }
            }
        }

        public int TotalMajors
        {
            get => _totalMajors;
            set
            {
                if (_totalMajors != value)
                {
                    _totalMajors = value;
                    OnPropertyChanged(nameof(TotalMajors));
                }
            }
        }

        public int TotalStudents
        {
            get => _totalStudents;
            set
            {
                if (_totalStudents != value)
                {
                    _totalStudents = value;
                    OnPropertyChanged(nameof(TotalStudents));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DashboardPage()
        {
            InitializeComponent();
            DataContext = this; // Définit le DataContext
            LoadAndProcessStudentData();
        }

        private async Task<IEnumerable<Student>> LoadStudentDataFromDbAsync()
        {
            var students = new List<Student>();
            try
            {
                using var connection = DBConnection.GetConnection();
                await connection.OpenAsync();

                string query = @"SELECT students.Id, students.FirstName, students.LastName, students.Email, students.MajorId, students.DateOfBirth, majors.Name as MajorName, majors.Description as MajorDescription,majors.Responsable as Responsable 
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
                        DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? null : reader.GetDateTime("DateOfBirth"),
                        Major = new Major
                        {
                            Id = reader.GetInt32("MajorId"),
                            Name = reader.GetString("MajorName"),
                            Description = reader.GetString("MajorDescription"),
                            Responsable = reader.GetString("Responsable")

                        }
                    };

                    students.Add(student);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des étudiants: {ex.Message}");
            }
            return students;
        }

        private async void LoadAndProcessStudentData()
        {
            var students = await LoadStudentDataFromDbAsync();
            var studentsPerMajor = students
                .GroupBy(s => s.Major?.Name ?? "Non attribué")
                .ToDictionary(g => g.Key, g => g.Count());

            
            TotalMajors = studentsPerMajor.Count; // Nombre total de filières
            TotalStudents = students.Count(); // Nombre total d'étudiants

           
            StudentsPerMajorText = string.Join("\n", studentsPerMajor.Select(kv => $"- {kv.Key}: {kv.Value} étudiant{(kv.Value > 1 ? "s" : "")}"));
        }
    }

}
