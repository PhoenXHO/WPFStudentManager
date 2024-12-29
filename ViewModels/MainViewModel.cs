using GalaSoft.MvvmLight;
using StudentManager.Models;
using StudentManager.ViewModels.Pages;

namespace StudentManager.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public static Session? CurrentSession { get; set; }

        public StudentsViewModel StudentsViewModel { get; set; }
        public MajorsViewModel MajorsViewModel { get; }
        public StatsViewModel StatsViewModel { get; }

        public MainViewModel()
        {
            MajorsViewModel = new MajorsViewModel();
            StudentsViewModel = new StudentsViewModel(MajorsViewModel);
            StatsViewModel = new StatsViewModel();
        }
    }
}
