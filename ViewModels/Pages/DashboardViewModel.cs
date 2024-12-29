using System.ComponentModel;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using StudentManager.Models;
using StudentManager.Services;
using System.Collections.ObjectModel;
using StudentManager.DataAccess;

namespace StudentManager.ViewModels.Pages
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private string _studentsPerMajorText;
        private int _totalMajors;
        private int _totalStudents;
        private string _welcomeMessage;
        private ObservableCollection<LogEntry> _userLogs;

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

        public ObservableCollection<LogEntry> UserLogs
        {
            get => _userLogs;
            set
            {
                if (_userLogs != value)
                {
                    _userLogs = value;
                    OnPropertyChanged(nameof(UserLogs));
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
            WelcomeMessage = $"Bienvenue, {MainViewModel.CurrentSession?.Username}!";
            UserLogs = [];

            CacheService.StudentsChanged += UpdateStudentStats;
            CacheService.MajorsChanged += UpdateMajorStats;

            UpdateStats();
        }

        private void UpdateStats()
        {
            UpdateStudentStats();
            UpdateMajorStats();
        }

        private void UpdateStudentStats()
        {
            var students = CacheService.Students;
            var studentsPerMajor = students
                .GroupBy(s => s.Major?.Name ?? "Non attribué")
                .ToDictionary(g => g.Key, g => g.Count());

            // First 5 only
            TotalStudents = students.Count;
            StudentsPerMajorText = string.Join("\n",
                studentsPerMajor.Take(5).Select(kv =>
                    $"- {kv.Key}: {kv.Value} étudiant{(kv.Value > 1 ? "s" : "")}"));
        }

        private void UpdateMajorStats()
        {
            TotalMajors = CacheService.Majors.Count;
        }

        public async void LoadUserLogs()
        {
            var logs = await DatabaseRepository.GetUserLogsAsync(MainViewModel.CurrentSession.UserId);
            UserLogs.Clear();
            // First 7 only
            foreach (var log in logs.Take(7))
            {
                UserLogs.Add(log);
            }
        }
    }
}
