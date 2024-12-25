using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using StudentManager.Models;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using StudentManager.Services;


namespace StudentManager.ViewModels.Pages
{
    public class StatsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<Stats> _majorStats;
        public Stats MaxStudentMajor { get; set; }


        public ObservableCollection<Stats> MajorStats
        {
            get => _majorStats;
            set
            {
                _majorStats = value;
                RaisePropertyChanged(nameof(MajorStats));
            }
        }

       
        public StatsViewModel()
        {
            LoadMajorStats();
            LoadMaxStudentMajorStats();
        }

        private void LoadMajorStats()
        {
            using var connection = DBConnection.GetConnection();
            connection?.Open();
            var query = @"SELECT M.Name AS Major, COUNT(S.Id) AS StudentCount
                          FROM Majors M
                          LEFT JOIN Students S ON M.Id = S.MajorId
                          GROUP BY M.Name";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            var statsList = new ObservableCollection<Stats>();
            while (reader.Read())
            {
                statsList.Add(new Stats
                {
                    Major = reader.GetString(0),
                    StudentCount = reader.GetInt32(1)
                });
            }
            MajorStats = statsList;
        }

        private void LoadMaxStudentMajorStats()
        {
            using var connection = DBConnection.GetConnection();
            connection?.Open();

            var query = @" SELECT M.Name AS Major, COUNT(S.Id) AS StudentCount
                        FROM Majors M
                        LEFT JOIN Students S ON M.Id = S.MajorId
                        GROUP BY M.Name
                        ORDER BY StudentCount DESC
                        LIMIT 1";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                MaxStudentMajor = new Stats
                {
                    Major = reader.GetString(0),
                    StudentCount = reader.GetInt32(1)
                };
            }
        }


    }
}