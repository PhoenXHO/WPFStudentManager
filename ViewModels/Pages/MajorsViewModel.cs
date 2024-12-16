using GalaSoft.MvvmLight;
using StudentManager.Models;
using System.Collections.ObjectModel;

namespace StudentManager.ViewModels.Pages
{
    public class MajorsViewModel : ViewModelBase
    {
        public ObservableCollection<Major> Majors { get; }

        public MajorsViewModel()
        {
            //TODO: Replace with data from a database
            Majors =
            [
                new Major { Id = 1, Name = "Computer Science", Description = "Computer Science" },
                new Major { Id = 2, Name = "Mathematics", Description = "Mathematics" }
            ];
        }
    }
}
