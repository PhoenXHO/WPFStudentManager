﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StudentManager.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace StudentManager.ViewModels.Pages
{
    public class MajorsViewModel : ViewModelBase
    {
        public ObservableCollection<Major> Majors { get; }
        public ObservableCollection<Major> SelectedMajors => new(Majors.Where(m => m.IsSelected));

        public ICommand ViewDetailsCommand { get; }

        public MajorsViewModel()
        {





            //TODO: Replace with data from a database (start ids from 1 so that the `All` item can have an id of 0)
            Majors =
            [
                new Major { Id = 1, Name = "Computer Science", Description = "Computer Science" },
                new Major { Id = 2, Name = "Mathematics", Description = "Mathematics" },
                new Major { Id = 3, Name = "Physics", Description = "Physics" },
                new Major { Id = 4, Name = "Chemistry", Description = "Chemistry" }
            ];

            Majors.CollectionChanged += (m, e) => RaisePropertyChanged(nameof(SelectedMajors)); // Update SelectedMajors when Majors changes

            ViewDetailsCommand = new RelayCommand<Major>(ViewDetails);
        }

        private void ViewDetails(Major major)
        {
            // For now, just show a message box
            MessageBox.Show($"Viewing details for {major.Name}");
        }
    }
}
