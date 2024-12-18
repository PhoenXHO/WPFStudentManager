using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StudentManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudentManager.ViewModels.Pages
{
    public class StudentsViewModel : ViewModelBase
    {
        public ObservableCollection<Student> Students { get; }
        public ObservableCollection<Student> SelectedStudents => new(Students.Where(s => s.IsSelected));
        public ObservableCollection<Major> MajorsWithAll { get; } 

        public ICommand ViewUsageInfoCommand { get; }

        public StudentsViewModel(MajorsViewModel majorsViewModel)
        {
            //TODO: Replace with data from a database (ids should start from 1)
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
                },
                new Student
                {
                    Id = 3,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice.johnson@example.com",
                    Major = majorsViewModel.Majors.FirstOrDefault(m => m.Id == 3),
                    DateOfBirth = new DateTime(1994, 3, 3)
                },
                new Student
                {
                    Id = 4,
                    FirstName = "Bob",
                    LastName = "Brown",
                    Email = "bob.brown@example.com",
                    Major = majorsViewModel.Majors.FirstOrDefault(m => m.Id == 4),
                    DateOfBirth = new DateTime(1996, 4, 4)
                },
                new Student
                {
                    Id = 5,
                    FirstName = "Charlie",
                    LastName = "Davis",
                    Email = "charlie.davis@example.com",
                    Major = majorsViewModel.Majors.FirstOrDefault(m => m.Id == 1),
                    DateOfBirth = new DateTime(1998, 5, 5)
                }
            ];

            MajorsWithAll = new ObservableCollection<Major>(majorsViewModel.Majors);
            MajorsWithAll.Insert(0, new Major { Id = 0, Name = "Tout", Description = "All Majors" });

            // Subscribe to the CollectionChanged event to update the SelectedStudents property
            Students.CollectionChanged += (s, e) => RaisePropertyChanged(nameof(SelectedStudents));

            ViewUsageInfoCommand = new RelayCommand(ViewUsageInfo);

        }

        private void ViewUsageInfo()
        {
            // For now, just show a message box
            MessageBox.Show("This is a message box");
        }
    }
}
