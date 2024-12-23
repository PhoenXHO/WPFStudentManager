using System.ComponentModel;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using StudentManager.Models;

namespace StudentManager.ViewModels.Pages
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private string _studentsPerMajorText;
        private int _totalMajors;
        private int _totalStudents;
        private string _welcomeMessage;

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

        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set
            {
                if (_welcomeMessage != value)
                {
                    _welcomeMessage = value;
                    OnPropertyChanged(nameof(WelcomeMessage));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DashboardViewModel()
        {
            LoadAndProcessStudentData();
            WelcomeMessage = $"Bienvenue, {MainViewModel.CurrentSession?.Username}!";
        }

        private async void LoadAndProcessStudentData()
        {
            var students = await LoadStudentDataFromDbAsync();
            var studentsPerMajor = students
                .GroupBy(s => s.Major?.Name ?? "Non attribué")
                .ToDictionary(g => g.Key, g => g.Count());

            TotalMajors = studentsPerMajor.Count;
            TotalStudents = students.Count();

            StudentsPerMajorText = string.Join("\n",
                studentsPerMajor.Select(kv => $"- {kv.Key}: {kv.Value} étudiant{(kv.Value > 1 ? "s" : "")}"));
        }

        private async Task<IEnumerable<Student>> LoadStudentDataFromDbAsync()
        {
            var students = new List<Student>();
            try
            {
                using var connection = DBConnection.GetConnection();
                await connection.OpenAsync();

                string query = @"SELECT students.Id, students.FirstName, students.LastName, students.Email, students.MajorId, students.DateOfBirth, majors.Name as MajorName, majors.Description as MajorDescription, majors.Responsable as Responsable 
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
    }
}
