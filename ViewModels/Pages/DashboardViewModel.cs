using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudentManager.ViewModels.Pages
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private string _greetingMessage;
        private string _appIntroduction;
        private string _recentActivities;
        private string _notifications;
        private int _totalMajors;
        private int _totalStudents;

        public string GreetingMessage
        {
            get => _greetingMessage;
            set
            {
                _greetingMessage = value;
                OnPropertyChanged();
            }
        }

        public string AppIntroduction
        {
            get => _appIntroduction;
            set
            {
                _appIntroduction = value;
                OnPropertyChanged();
            }
        }

        public string RecentActivities
        {
            get => _recentActivities;
            set
            {
                _recentActivities = value;
                OnPropertyChanged();
            }
        }

        public string Notifications
        {
            get => _notifications;
            set
            {
                _notifications = value;
                OnPropertyChanged();
            }
        }

        public int TotalMajors
        {
            get => _totalMajors;
            set
            {
                _totalMajors = value;
                OnPropertyChanged();
            }
        }

        public int TotalStudents
        {
            get => _totalStudents;
            set
            {
                _totalStudents = value;
                OnPropertyChanged();
            }
        }

        public DashboardViewModel()
        {
            // Initialize properties with sample data
            var currentUser = MainViewModel.CurrentSession.Username;
            GreetingMessage = $"Bonjour {currentUser}, Bienvenue dans notre application de gestion des étudiants!";
            AppIntroduction = "Bienvenue dans Student Manager, votre assistant intelligent pour gérer et explorer les données des étudiants ! Avec une interface conviviale, l'application vous permet de visualiser facilement des statistiques essentielles, comme le nombre d'étudiants par filière, grâce à des graphiques interactifs 3D. Que vous souhaitiez suivre les informations académiques ou obtenir des insights utiles, Student Manager est là pour vous simplifier la vie et vous aider à prendre les meilleures décisions.";
            
            TotalMajors = 5; // Replace with actual data from the database
            TotalStudents = 200; // Replace with actual data from the database
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
