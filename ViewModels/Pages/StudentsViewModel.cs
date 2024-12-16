using GalaSoft.MvvmLight;
using StudentManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager.ViewModels.Pages
{
    public class StudentsViewModel : ViewModelBase
    {
        public ObservableCollection<Student> Students { get; }

        public StudentsViewModel(MajorsViewModel majorsViewModel)
        {
            //TODO: Replace with data from a database
            Students =
            [
                new Student
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Major = majorsViewModel.Majors.FirstOrDefault(m => m.Id == 1),
                    DateOfBirth = new DateTime(1990, 1, 1)
                },
                new Student
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Major = majorsViewModel.Majors.FirstOrDefault(m => m.Id == 2),
                    DateOfBirth = new DateTime(1992, 2, 2)
                }
            ];
        }
    }
}
