using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using StudentManager.Models;
using System.DirectoryServices;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System.ComponentModel;
using MySql.Data.MySqlClient;


namespace StudentManager.ViewModels.Pages
{
    public class StatsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<Stats> _majorStats;

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
        }

        private void LoadMajorStats()
        {
            using (var connection = DBConnection.GetConnection())
            {
                connection.Open();
                var query = "SELECT M.Name AS Major, COUNT(S.Id) AS StudentCount " +
                            "FROM Majors M " +
                            "LEFT JOIN Students S ON M.Id = S.MajorId " +
                "GROUP BY M.Name";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
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
                }
            }
        }

    }
}