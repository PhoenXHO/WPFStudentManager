using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using StudentManager.Models;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using StudentManager.DataAccess;
using StudentManager.Services;
using System.Data;


namespace StudentManager.ViewModels.Pages
{
    public class StatsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<Stats> _majorStats;
        private Stats? _maxStudentMajor;

        public ObservableCollection<Stats> MajorStats
        {
            get => _majorStats;
            set
            {
                _majorStats = value;
                RaisePropertyChanged(nameof(MajorStats));
            }
        }

        public Stats? MaxStudentMajor
        {
            get => _maxStudentMajor;
            set
            {
                _maxStudentMajor = value;
                RaisePropertyChanged(nameof(MaxStudentMajor));
            }
        }

        public StatsViewModel()
        {
            _majorStats = [];

            CacheService.StudentsChanged += UpdateStats;
            CacheService.MajorsChanged += UpdateStats;

            UpdateStats();
        }

        private void UpdateStats()
        {
            var stats = CacheService.Majors
                .Select(m => new Stats
                {
                    Major = m.Name,
                    StudentCount = CacheService.Students.Count(s => s.Major?.Name == m.Name)
                }).ToList();

            MajorStats = new(stats);

            MaxStudentMajor = MajorStats.OrderByDescending(s => s.StudentCount).FirstOrDefault();
        }
    }
}