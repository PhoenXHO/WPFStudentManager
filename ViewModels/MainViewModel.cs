using GalaSoft.MvvmLight;
using StudentManager.Models;
using StudentManager.ViewModels.Pages;

namespace StudentManager.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        // Singleton pattern
        private static MainViewModel? _instance;
        public static MainViewModel Instance => _instance ??= new MainViewModel();

        public static Session? CurrentSession { get; set; }

        public StudentsViewModel StudentsViewModel { get; set; }
        public MajorsViewModel MajorsViewModel { get; }
        public StatsViewModel StatsViewModel { get; }
        public DashboardViewModel DashboardViewModel { get; }

        public MainViewModel()
        {
            MajorsViewModel = new MajorsViewModel();
            StudentsViewModel = new StudentsViewModel(MajorsViewModel);
            StatsViewModel = new StatsViewModel();
            DashboardViewModel = new DashboardViewModel();

            // Ensure initial data load
            _ = InitializeDataAsync();
        }

        private async Task InitializeDataAsync()
        {
            await Task.WhenAll(
                MajorsViewModel.UpdateMajorsAsync(),
                StudentsViewModel.UpdateStudentsAsync()
            );
        }
    }
}
