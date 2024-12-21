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
            MajorStats =
            [
                new Stats { Major = "Computer Science", StudentCount = 120 },
                new Stats { Major = "Mathematics", StudentCount = 80 },
                new Stats { Major = "Physics", StudentCount = 60 }
            ];
        }
    }
}