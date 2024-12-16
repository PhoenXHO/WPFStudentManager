using GalaSoft.MvvmLight;
using StudentManager.ViewModels.Pages;

namespace StudentManager.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public StudentsViewModel StudentsViewModel { get; }
        public MajorsViewModel MajorsViewModel { get; }

        public MainViewModel()
        {
            MajorsViewModel = new MajorsViewModel();
            StudentsViewModel = new StudentsViewModel(MajorsViewModel);
        }
    }
}
